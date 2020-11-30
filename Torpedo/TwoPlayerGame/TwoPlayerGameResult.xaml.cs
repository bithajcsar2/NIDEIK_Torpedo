using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Torpedo
{
    /// <summary>
    /// Interaction logic for TwoPlayerGameResult.xaml
    /// </summary>
    public partial class TwoPlayerGameResult : Window
    {
        public class Gameresult
        {
            public string winner { get; set; }
            public string loser{ get; set; }
            public int numOfRounds { get; set; }
            public int p1HitCount{ get; set; }
            public int p2HitCount{ get; set; }
            public string p1ShipsSats { get; set; }
            public string p2ShipsSats { get; set; }
    }
        public TwoPlayerGameResult()
        {
            InitializeComponent();
        }

        private void ExitGame(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void NewGame(object sender, RoutedEventArgs e)
        {
            MainWindow mainwindow = new MainWindow();
            mainwindow.Show();
            this.Close();
        }

        public void FillDataGridWithResult()
        {
            List<Gameresult> gameResults = ReadJson();
            Results.ItemsSource = gameResults;
        }

        public void WriteJson(string winner, string loser, int numOfRounds, int p1HitCount, int p2HitCount, List<Ship> p1Ships, List<Ship> p2Ships)
        {
            Gameresult gameResult = new Gameresult();
            gameResult.winner = winner;
            gameResult.loser = loser;
            gameResult.numOfRounds = numOfRounds;
            gameResult.p1HitCount = p1HitCount;
            gameResult.p2HitCount = p2HitCount;

            gameResult.p1ShipsSats = "Ships alive: ";
            foreach (var ship in p1Ships.Where(ship => ship.isDead == false))
            {
                gameResult.p1ShipsSats += ship.name.ToString() + " ";
            }
            gameResult.p1ShipsSats += " ships sunken: ";
            foreach (var ship in p1Ships.Where(ship => ship.isDead == true))
            {
                gameResult.p1ShipsSats += ship.name.ToString() + " ";
            }

            gameResult.p2ShipsSats = "Ships alive: ";
            foreach (var ship in p2Ships.Where(ship => ship.isDead == false))
            {
                gameResult.p2ShipsSats += ship.name.ToString() + " ";
            }
            gameResult.p2ShipsSats += " ships sunken: ";
            foreach (var ship in p2Ships.Where(ship => ship.isDead == true))
            {
                gameResult.p2ShipsSats += ship.name.ToString()+ " ";
            }


            
            string path = @"..\..\..\json\stats.json";

            var jsonData = File.ReadAllText(path);

            var gamesResultsList = JsonConvert.DeserializeObject<List<Gameresult>>(jsonData)
                      ?? new List<Gameresult>();

            string JSONresult = JsonConvert.SerializeObject(gameResult);

            gamesResultsList.Add(gameResult);
            jsonData = JsonConvert.SerializeObject(gamesResultsList);
            File.WriteAllText(path, jsonData);
        }
        public List<Gameresult> ReadJson()
        {
            string path = @"..\..\..\json\stats.json";
            // read file into a string and deserialize JSON to a type
            var reader = new JsonTextReader(
                new StreamReader(path));
            var serializer = new JsonSerializer();
            List<Gameresult> list;
            
            list = (List<Gameresult>)serializer.Deserialize(reader, typeof(List<Gameresult>));
            return list;
        }
    }
}
