using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace AStarPathfinding
{
    class Location
    {
        public int X;
        public int Y;
        public int F;
        public int G;
        public int H;
        public Location Parent;
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "A* Pathfinding";

            // draw map

            var path = Path.Combine(Directory.GetCurrentDirectory(), "assets/maps/map.txt");
            //string[] map = System.IO.File.ReadAllLines(path);
            string[] lines = System.IO.File.ReadAllLines(path);
            string[] map = new string[lines.Length - 2];
            int offsetX = Convert.ToInt16(lines[0]) * -1;
            int offsetY = Convert.ToInt16(lines[1]) * -1;
            for (var i = 2; i < lines.Length; i++)
            {
                map[i - 2] = lines[i];
                Console.WriteLine(map[i - 2]);
            }
            /* string[] map = new string[]
            {
                "+------+",
                "|      |",
                "|A X   |",
                "|XXX   |",
                "|   X  |",
                "| B    |",
                "|      |",
                "+------+",
            }; */
            foreach (var line in map)
                Console.WriteLine(line);

            // algorithm

            Location current = null;
            var start = new Location { X = 1 + offsetX, Y = 2 + offsetY };
            var target = new Location { X = 2 + offsetX, Y = 5 + offsetY };
            var openList = new List<Location>();
            var closedList = new List<Location>();
            int g = 0;

            Console.WriteLine("start" + start.X.ToString() + ":" + start.Y.ToString());

            // start by adding the original position to the open list
            openList.Add(start);
            while (openList.Count > 0)
            {
                // get the square with the lowest F score
                var lowest = openList.Min(l => l.F);
                current = openList.First(l => l.F == lowest);

                // add the current square to the closed list
                closedList.Add(current);

                // show current square on the map
                Console.SetCursorPosition(current.X, current.Y);
                Console.Write('.');
                Console.SetCursorPosition(current.X, current.Y);
                System.Threading.Thread.Sleep(1000);

                // remove it from the open list
                openList.Remove(current);

                // if we added the destination to the closed list, we've found a path
                if (closedList.FirstOrDefault(l => l.X == target.X && l.Y == target.Y) != null)
                    break;

                var adjacentSquares = GetWalkableAdjacentSquares(current.X, current.Y, map);
                g++;

                foreach(var adjacentSquare in adjacentSquares)
                {
                    // if this adjacent square is already in the closed list, ignore it
                    if (closedList.FirstOrDefault(l => l.X == adjacentSquare.X
                            && l.Y == adjacentSquare.Y) != null)
                        continue;

                    // if it's not in the open list...
                    if (openList.FirstOrDefault(l => l.X == adjacentSquare.X
                            && l.Y == adjacentSquare.Y) == null)
                    {
                        // compute its score, set the parent
                        adjacentSquare.G = g;
                        adjacentSquare.H = ComputeHScore(adjacentSquare.X, adjacentSquare.Y, target.X, target.Y);
                        adjacentSquare.F = adjacentSquare.G + adjacentSquare.H;
                        adjacentSquare.Parent = current;

                        // and add it to the open list
                        openList.Insert(0, adjacentSquare);
                    }
                    else
                    {
                        // test if using the current G score makes the adjacent square's F score
                        // lower, if yes update the parent because it means it's a better path
                        if (g + adjacentSquare.H < adjacentSquare.F)
                        {
                            adjacentSquare.G = g;
                            adjacentSquare.F = adjacentSquare.G + adjacentSquare.H;
                            adjacentSquare.Parent = current;
                        }
                    }
                }
            }

            // assume path was found; let's show it
            while (current != null)
            {
                Console.SetCursorPosition(current.X, current.Y);
                Console.Write('_');
                Console.SetCursorPosition(current.X, current.Y);
                current = current.Parent;
                //System.Threading.Thread.Sleep(100);
            }

            // end

            Console.ReadLine();
        }

        static List<Location> GetWalkableAdjacentSquares(int x, int y, string[] map)
        {
            var proposedLocations = new List<Location>()
            {
                new Location { X = x, Y = y - 1 },
                new Location { X = x, Y = y + 1 },
                new Location { X = x - 1, Y = y },
                new Location { X = x + 1, Y = y },
            };

            return proposedLocations.Where(l => map[l.Y][l.X] == ' ' || map[l.Y][l.X] == 'B').ToList();
        }

        static int ComputeHScore(int x, int y, int targetX, int targetY)
        {
            return Math.Abs(targetX - x) + Math.Abs(targetY - y);
        }
    }
}
