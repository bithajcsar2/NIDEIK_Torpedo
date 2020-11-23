using System;
using System.Collections.Generic;
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
    /// Interaction logic for StatsWindow.xaml
    /// </summary>
    public partial class StatsWindow : Window
    {
        public int roundCounter = 1;
        public int p1HitCount = 0, p2HitCount = 0;
        string player1Name, player2Name;

        public void incRound()
        {
            roundCounter++;
            roundCounterLabel.Content = "Round: " + roundCounter;
        }

        public void incP1HitCount()
        {
            p1HitCount++;
            p1HitCounterLabel.Content = player1Name+ " hit count: " + p1HitCount;
        }

        public void incP2HitCount()
        {
            p2HitCount++;
            p2HitCounterLabel.Content = player2Name+" hit count: " + p2HitCount;
        }

        public void listP1ShipStats(List<Ship> p1Ships)
        {
            p1ShipsSatusLabel.Content = player1Name + "s ships alive: ";
            foreach (var ship in p1Ships.Where(ship => ship.isDead == false))
            {
                p1ShipsSatusLabel.Content += ship.name.ToString() + " ";
            }
            p1ShipsSatusLabel.Content += "\n and ships sunken: ";
            foreach (var ship in p1Ships.Where(ship => ship.isDead == true))
            {
                p1ShipsSatusLabel.Content += ship.name.ToString() + " ";
            }
        }
        
        public void listP2ShipStats(List<Ship> p2Ships)
        {
            p2ShipsSatusLabel.Content = player2Name + "s ships alive: ";
            foreach (var ship in p2Ships.Where(ship=>ship.isDead==false))
            {
                p2ShipsSatusLabel.Content += ship.name.ToString()+" ";
            }
            p2ShipsSatusLabel.Content += "\n and ships sunken: ";
            foreach (var ship in p2Ships.Where(ship => ship.isDead == true))
            {
                p2ShipsSatusLabel.Content += ship.name.ToString()+" ";
            }
        }

        public StatsWindow(string p1Name, string p2Name)
        {
            player1Name = p1Name;
            player2Name = p2Name;
            InitializeComponent();
            p1ShipsSatusLabel.Content = "All of "+player1Name+"s ships are alive.";
            p2ShipsSatusLabel.Content = "All of " + player2Name + "s ships are alive.";

            p1HitCounterLabel.Content = player1Name + " hit count: " + p1HitCount;
            p2HitCounterLabel.Content = player2Name + " hit count: " + p2HitCount;

            roundCounterLabel.Content += roundCounter.ToString();
        }
    }
}
