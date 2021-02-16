using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace DealerOn.Core
{
    public class Trains
    {
        /// <summary>
        /// set the routes
        /// </summary>
        /// <param name="input">all the routes, separate by comma like: "AB5, BC4, CD8, DC8, DE6, AD5, CE2, EB3, AE7"</param>
        /// <returns>all the routes for each towns with distance</returns>
        public List<TownDistance> SetInputs(string input)
        {
            List<string> entryData = input.Replace(" ", "").Split(",").ToList();
            List<TownDistance> townsDistance = new List<TownDistance>();
        
            for (int i = 0; i < entryData.Count; i++)
            {
                townsDistance.Add(
                    new TownDistance
                    {
                        From = entryData[i].Substring(0, 1),
                        To = entryData[i].Substring(1, 1),
                        Distance = Convert.ToInt32(entryData[i].Substring(2, 1))
                    });
            }
            return townsDistance;
        }

        /// <summary>
        /// set the specific criteria for 1 to 10: describing in the file
        /// </summary>
        /// <param name="townsDistance">the list of all the routes for each towns with distance</param>
        /// <returns>the result for print after do all operations</returns>
        public string Caculate(List<TownDistance> townsDistance)
        {
            string result = "";
            var ab = townsDistance.FirstOrDefault(t => t.From == "A" && t.To == "B");
            var bc = townsDistance.FirstOrDefault(t => t.From == "B" && t.To == "C");

            result += $"Ouput #1: {GetMsj(ab?.Distance, bc?.Distance)} \n";

            var ad = townsDistance.FirstOrDefault(t => t.From == "A" && t.To == "D");
            result += $"Ouput #2: {GetMsj(ad?.Distance)} \n";

            var dc = townsDistance.FirstOrDefault(t => t.From == "D" && t.To == "C");
            result += $"Ouput #3: {GetMsj(ad?.Distance, dc?.Distance)} \n";

            var ae = townsDistance.FirstOrDefault(t => t.From == "A" && t.To == "E");
            var eb = townsDistance.FirstOrDefault(t => t.From == "E" && t.To == "B");
            var cd = townsDistance.FirstOrDefault(t => t.From == "C" && t.To == "D");
            result += $"Ouput #4: {GetMsj(ae?.Distance, eb?.Distance, bc?.Distance, cd?.Distance)} \n";

            var ed = townsDistance.FirstOrDefault(t => t.From == "E" && t.To == "D");
            result += $"Ouput #5: {GetMsj(ae?.Distance, ed?.Distance)} \n";


            var nodesStart = townsDistance.Where(t => t.From == "C").ToList(); 
            List<string> tripsCC = new List<string>();
            tripsCC = GetPath(nodesStart, townsDistance);
            int MAX_STOP=3;
            result += $"Ouput #6: {tripsCC.Where(t=>t.IndexOf('C',1, MAX_STOP) != -1 ).Count() } \n";

            nodesStart = townsDistance.Where(t => t.From == "A").ToList();
            List<string> tripsAC = new List<string>();
            tripsAC = GetPath(nodesStart, townsDistance);
            MAX_STOP = 4;
            result += $"Ouput #7: {tripsAC.Where(t => t.IndexOf('C', MAX_STOP, 1) != -1).Count() } \n";


            int distance = GetShorterDistance(tripsAC, "C" , townsDistance);
            result += $"Ouput #8: {distance} \n";


            nodesStart = townsDistance.Where(t => t.From == "B").ToList();
            List<string> tripsBB = new List<string>();
            tripsBB = GetPath(nodesStart, townsDistance);
            distance = GetShorterDistance(tripsBB, "B", townsDistance);
            result += $"Ouput #9: {distance} \n";

            nodesStart = townsDistance.Where(t => t.From == "C").ToList();
            tripsCC = new List<string>() { "CDC", "CEBC", "CEBCDC", "CDCEBC", "CDEBC", "CEBCEBC", "CEBCEBCEBC" };
            var nRoutesCC = GetPathLessThan(tripsCC, "C", townsDistance, 30);
            result += $"Ouput #10: {nRoutesCC.Count} \n";

            return result;
        }

        /// <summary>
        /// get routes with less than certain distance
        /// </summary>
        /// <param name="trips">List of all trips</param>
        /// <param name="To">the direction that need to arrive the train</param>
        /// <param name="townsDistance">all the towns' list with their distances</param>
        /// <param name="maxPath">distance should be less that this</param>
        /// <returns>all the trips that are less than X</returns>
        private List<int> GetPathLessThan(List<string> trips, string To, List<TownDistance> townsDistance, int maxPath)
        {
            var sTrips = trips.Where(t => t.IndexOf(To, 1) != -1);
            var result = new List<int>();
            foreach (var sTrip in sTrips)
            {
                var sumDistance = 0;
                for (int i = 0; i < sTrip.IndexOf(To); i++)
                {
                    sumDistance += townsDistance.Where(td => td.From == sTrip.Substring(i, 1) && td.To == sTrip.Substring(i + 1, 1)).FirstOrDefault().Distance;
                }
                if(sumDistance< maxPath) {
                    result.Add(sumDistance);
                }
            }
            return result;
        }

        /// <summary>
        /// get the shorter distance
        /// </summary>
        /// <param name="trips">all the trips than applies</param>
        /// <param name="To">the direction to be arrive</param>
        /// <param name="townsDistance">all the routes town distance</param>
        /// <returns>the shorter distance</returns>
        private int GetShorterDistance(List<string> trips, string To, List<TownDistance> townsDistance)
        {
            var sTrips = trips.Where(t => t.IndexOf(To, 1) != -1);
            var shoterDistance = Int32.MaxValue;
            foreach (var sTrip in sTrips)
            {
                var sumDistance = 0;
                for (int i = 0; i < sTrip.IndexOf(To,1); i++)
                {
                    sumDistance += townsDistance.Where(td => td.From == sTrip.Substring(i, 1) && td.To == sTrip.Substring(i + 1, 1)).FirstOrDefault().Distance;
                }
                if (shoterDistance > sumDistance)
                {
                    shoterDistance = sumDistance;
                }
            }
            return shoterDistance;
        }

        /// <summary>
        /// get the all posible paths
        /// </summary>
        /// <param name="nodesStart">nodes than apply to the criteria</param>
        /// <param name="townsDistance">all the routes town distance</param>
        /// <returns>all posible paths </returns>
        private List<string> GetPath(List<TownDistance> nodesStart, List<TownDistance> townsDistance)
        {

            List<string> result = new List<string>();
            foreach (var nodeStart in nodesStart)
            {
                int countLoop = 0;
                var nTowns = townsDistance.Where(t => t.From == nodeStart.To);
                foreach (var town in nTowns)
                {
                    result.Add(nodeStart.From + Get(town, townsDistance, ref countLoop));
                    countLoop = 0;
                }
            }
            return result;
        }

        /// <summary>
        /// get recursively, the next node
        /// </summary>
        /// <param name="townDistance">the current route town</param>
        /// <param name="townsDistance">all the routes town distance</param>
        /// <param name="nTry">number of tries, for avoiding infinite loop</param>
        /// <returns>the entire path until nTry</returns>
        private string Get(TownDistance townDistance, List<TownDistance> townsDistance, ref int nTry)
        {
            ++nTry;
            if (nTry > townsDistance.Count)
                return townDistance.From;
            var town = townsDistance.FirstOrDefault(td => td.From == townDistance.To);
            return townDistance.From + $"{Get(town, townsDistance, ref nTry)}";
        }


        /// <summary>
        /// get the msj when there aren't routes
        /// </summary>
        /// <param name="list">all nodes with its disntances</param>
        /// <returns>msj whether exist or not</returns>
        private string GetMsj(params int?[] list)
        {
            if (list.Any(x => x == null))
                return "NO SUCH ROUTE";
            else
                return $"{list.Sum(x => x)}";
        }

    }

    /// <summary>
    /// class for using TownDistances
    /// </summary>
    public class TownDistance
    {
        public string From { get; set; }
        public string To { get; set; }
        public int Distance { get; set; }
    }
}