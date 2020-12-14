using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Torpedo.GameResult
{
    class GameResultControler : IGameResultContoler
    {
        protected IGameResultsView View;

        public GameResultControler(IGameResultsView view)
        {
            View = view;
        }

        public void FillDataGridWithResult()
        {
            List<GameResultModel> gameResults = ReadJson();
            View.shareGrid.ItemsSource = gameResults;
        }

        public void WriteJson(string winner, string loser, int numOfRounds, int p1HitCount, int p2HitCount, List<Ship> p1Ships, List<Ship> p2Ships)
        {
            GameResultModel gameResult = new GameResultModel
            {
                winner = winner,
                loser = loser,
                numOfRounds = numOfRounds,
                p1HitCount = p1HitCount,
                p2HitCount = p2HitCount,

                p1ShipsSats = "Ships alive: "
            };
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
                gameResult.p2ShipsSats += ship.name.ToString() + " ";
            }



            string path = @"..\..\..\json\stats.json";

            var jsonData = File.ReadAllText(path);

            var gamesResultsList = JsonConvert.DeserializeObject<List<GameResultModel>>(jsonData)
                      ?? new List<GameResultModel>();

            string JSONresult = JsonConvert.SerializeObject(gameResult);

            gamesResultsList.Add(gameResult);
            jsonData = JsonConvert.SerializeObject(gamesResultsList);
            File.WriteAllText(path, jsonData);
        }


        public List<GameResultModel> ReadJson()
        {
            string path = @"..\..\..\json\stats.json";
            // read file into a string and deserialize JSON to a type
            var reader = new JsonTextReader(
                new StreamReader(path));
            var serializer = new JsonSerializer();
            List<GameResultModel> list;

            list = (List<GameResultModel>)serializer.Deserialize(reader, typeof(List<GameResultModel>));
            return list;
        }


    }
}
