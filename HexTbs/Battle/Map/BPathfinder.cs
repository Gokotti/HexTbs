using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Threading;

namespace HexTbs.Battle.Map
{
   public static class BPathfinder
   {
      private static bool PrintEnabled = true;
      private static int maxPathLength = 50;

      public static void Init()
      {
      }

      private static void PrintGrid(int[,] grid)
      {
         if (!PrintEnabled)
            return;

         Console.WriteLine("Haun tulos: ");
         for (int y = 0; y < grid.GetLength(1); y++)
         {
            if (y % 2 != 0)
               Console.Write(" ");
            for (int x = 0; x < grid.GetLength(0); x++)
            {
               int arvo = grid[x, y];
               if (arvo == OBSTACLE)
                  Console.Write("# ");
               else if (arvo == UNSEARCHED)
                  Console.Write("? ");
               else
               {
                  //Console.Write(". ");

                  if (arvo > 10 || arvo < 0)
                     Console.Write(arvo);
                  else
                     Console.Write(arvo + " ");
               }
            }
            Console.WriteLine();
         }
         Console.WriteLine();
      }

      private const int UNSEARCHED = 9999;
      private const int OBSTACLE = -1;
      public static List<Vector2> GetPath(BMap map, Vector2 _start, Vector2 _end)
      {
         if (_start == _end)
            return null;

         List<Vector2> path = new List<Vector2>();

         Point start = new Point(map.GetHex(_start).X, map.GetHex(_start).Y);
         Point end = new Point(map.GetHex(_end).X, map.GetHex(_end).Y);

         PathNode[,] grid = new PathNode[map.Width, map.Height];

         for (int y = 0; y < map.Height; y++)
         {
            for (int x = 0; x < map.Width; x++)
            {
               grid[x, y] = new PathNode(x,y,UNSEARCHED, null);
               if (map.GetHex(x, y).obs)
                  grid[x, y] = new PathNode(x,y,OBSTACLE, null);
            }
         }

         foreach (Point p in map.Occupieds)
            grid[p.X, p.Y] = new PathNode(p.X, p.Y, OBSTACLE, null);

         bool pathFound = false;
         List<PathNode> nodes = new List<PathNode>();
         nodes.Add(new PathNode(start.X, start.Y, 0, null));

         PathNode endNode = null;

         while (nodes.Count > 0 && !pathFound)
         {
            List<PathNode> newNodes = new List<PathNode>();
            foreach (PathNode node in nodes)
            {
               foreach (BHex hex in map.GetHex(node.X, node.Y).neighbours)
               {
                  int x = hex.X;
                  int y = hex.Y;
                  int val = node.Value + hex.TrackedMoveCost;

                  if (grid[x, y].Value > val)
                  {
                     grid[x, y] = new PathNode(x, y, val, node);
                     newNodes.Add(grid[x, y]);

                     if (end.X == x && end.Y == y)
                     {
                        //pathFound = true;
                        endNode = grid[x, y];
                     }
                  }
               }
            }

            nodes.Clear();
            nodes.AddRange(newNodes);
         }

         if (endNode != null)
         {
            path.Add(map.GetCoordinates(endNode.X, endNode.Y));

            PathNode node = endNode.Parent;
            while (node != null)
            {
               path.Add(map.GetCoordinates(node.X, node.Y));
               node = node.Parent;
            }

            path.Reverse();
            return path;
         }

         return null;
      }

      private static Point ClosestNeighbour(int[,] grid, Point p)
      {
         List<Point> neighbours = new List<Point>();

         neighbours.Add(new Point(p.X, p.Y - 1));
         neighbours.Add(new Point(p.X, p.Y + 1));
         neighbours.Add(new Point(p.X + 1, p.Y));
         neighbours.Add(new Point(p.X - 1, p.Y));

         if (p.Y % 2 == 0)
         {
            neighbours.Add(new Point(p.X - 1, p.Y - 1));
            neighbours.Add(new Point(p.X - 1, p.Y + 1));
         }
         else
         {
            neighbours.Add(new Point(p.X + 1, p.Y - 1));
            neighbours.Add(new Point(p.X + 1, p.Y + 1));
         }

         Point nearest = p;
         int value = UNSEARCHED;

         // Find the best candidate
         foreach (Point c in neighbours)
         {
            if (c.X >= 0 && c.X < grid.GetLength(0) && c.Y >= 0 && c.Y < grid.GetLength(1))
            {
               int gridval = grid[c.X, c.Y];
               if (gridval >= 0 && gridval < value)
               {
                  value = gridval;
                  nearest = c;
               }
            }
         }

         return nearest;
      }

      public static PathNode[,] GetMoveGrid(BMap map, Vector2 _position)
      {
         List<Vector2> path = new List<Vector2>();

         Point start = new Point(map.GetHex(_position).X, map.GetHex(_position).Y);

         PathNode[,] grid = new PathNode[map.Width, map.Height];

         for (int y = 0; y < map.Height; y++)
         {
            for (int x = 0; x < map.Width; x++)
            {
               grid[x, y] = new PathNode(x, y, UNSEARCHED, null);
               if (map.GetHex(x, y).obs)
                  grid[x, y] = new PathNode(x, y, OBSTACLE, null);
            }
         }

         foreach (Point p in map.Occupieds)
            grid[p.X, p.Y] = new PathNode(p.X, p.Y, OBSTACLE, null);

         bool pathFound = false;
         List<PathNode> nodes = new List<PathNode>();
         nodes.Add(new PathNode(start.X, start.Y, 0, null));

         while (nodes.Count > 0 && !pathFound)
         {
            List<PathNode> newNodes = new List<PathNode>();
            foreach (PathNode node in nodes)
            {
               foreach (BHex hex in map.GetHex(node.X, node.Y).neighbours)
               {
                  int x = hex.X;
                  int y = hex.Y;
                  int val = node.Value + hex.TrackedMoveCost;

                  if (grid[x, y].Value > val)
                  {
                     grid[x, y] = new PathNode(x, y, val, node);
                     newNodes.Add(grid[x, y]);
                  }
               }
            }

            nodes.Clear();
            nodes.AddRange(newNodes);
         }

         return grid;

         /*int[,] moveGrid = new int[map.Width, map.Height];
         for (int y = 0; y < map.Height; y++)
         {
            for (int x = 0; x < map.Width; x++)
            {
               moveGrid[x, y] = grid[x, y].Value;
            }
         }*/
      }

      public static int[,] GetRangeGrid(BMap map, Vector2 _position, int range)
      {
         Point position = new Point(map.GetHex(_position).X, map.GetHex(_position).Y);

         int[,] grid = new int[map.Width, map.Height];

         for (int y = 0; y < map.Height; y++)
         {
            for (int x = 0; x < map.Width; x++)
               grid[x, y] = UNSEARCHED;
         }

         List<Point> searchNext = new List<Point>();
         searchNext.Add(new Point(position.X, position.Y));
         int value = 0;

         while (searchNext.Count > 0)
         {
            List<Point> addNext = new List<Point>();
            foreach (Point p in searchNext)
            {
               if (grid[p.X, p.Y] == UNSEARCHED)
                  grid[p.X, p.Y] = value;
               foreach (BHex hex in map.GetHex(p.X, p.Y).neighbours)
               {
                  Point np = new Point(hex.X, hex.Y);
                  if (grid[hex.X, hex.Y] == UNSEARCHED && !addNext.Contains(np)) // Search conditions
                     addNext.Add(np);
               }
            }
            searchNext.Clear();
            searchNext.AddRange(addNext);

            if (value > range)
               return grid;

            value++;
         }

         return grid;
      }

      public static int[,] GetSightGrid(BMap map, Vector2 _position, int range)
      {
         Point position = new Point(map.GetHex(_position).X, map.GetHex(_position).Y);
         int[,] fireGrid = GetRangeGrid(map, _position, range);
         int[,] sightGrid = new int[map.Width, map.Height];

         for (int y = 0; y < map.Height; y++)
         {
            for (int x = 0; x < map.Width; x++)
               sightGrid[x, y] = UNSEARCHED;
         }

         foreach (Point p in map.Occupieds)
            sightGrid[p.X, p.Y] = OBSTACLE;

         List<Point> corners = new List<Point>();

         // Kerätään nurkkapalat
         for (int y = -range; y <= range; y++)
         {
            for (int x = -range; x <= range; x++)
            {
               BHex cHex = map.GetHex(position.X + x, position.Y + y);
               if (cHex != null && cHex.X >= 0 && cHex.X < map.Width && cHex.Y >= 0 && cHex.Y < map.Height)
               {
                  BHex aHex = map.GetHex(cHex.X, cHex.Y);
                  int hexRange = fireGrid[cHex.X, cHex.Y];

                  if (hexRange == range)
                     corners.Add(new Point(cHex.X, cHex.Y));
                  else if (hexRange != UNSEARCHED && hexRange < range && (cHex.X == 0 || cHex.Y == 0  || cHex.X == map.Width-1 || cHex.Y == map.Height-1))
                     corners.Add(new Point(cHex.X, cHex.Y));
               }
            }
         }

         var watch = System.Diagnostics.Stopwatch.StartNew();
         
         foreach (Point c in corners)
         {
            Vector2 endPosition = map.GetCoordinates(c.X, c.Y);
            Vector2 currentPosition = _position;

            Vector2 step = endPosition - _position;
            step.Normalize();
            step *= map.HexSize / 4;

            bool obstacleFound = false;
            float distanceTravelled = 0;
            float stepLength = Vector2.Distance(Vector2.Zero, step);
            float travelLength = Vector2.Distance(_position, endPosition) + stepLength;

            while (distanceTravelled < travelLength)
            {
               BHex cpHex = map.GetHex(currentPosition);

               if (cpHex != null) {
                  int value = sightGrid[cpHex.X, cpHex.Y];

                  if (obstacleFound)
                  {
                     if (value == UNSEARCHED)
                        sightGrid[cpHex.X, cpHex.Y] = OBSTACLE;
                  }
                  else
                  {
                     if (value == OBSTACLE || cpHex.BlockFov)
                     {
                        obstacleFound = true;
                        sightGrid[cpHex.X, cpHex.Y] = fireGrid[cpHex.X, cpHex.Y];
                     }
                     else if (value == UNSEARCHED)
                     {
                        sightGrid[cpHex.X, cpHex.Y] = fireGrid[cpHex.X, cpHex.Y];
                     }
                  }
               }

               distanceTravelled += stepLength;
               currentPosition += step;
            }
         }

         watch.Stop();
         var elapsedMs = watch.ElapsedMilliseconds;
         Console.WriteLine("tähän meni: " + elapsedMs);

         return sightGrid;
      }

      public static bool CanSeeHex(BMap map, Vector2 _position, BHex target)
      {
         Vector2 endPosition = map.GetCoordinates(target.X, target.Y);
         Vector2 currentPosition = _position;

         Vector2 step = endPosition - _position;
         step.Normalize();
         step *= map.HexSize / 4;

         float distanceTravelled = 0;
         float stepLength = Vector2.Distance(Vector2.Zero, step);
         float travelLength = Vector2.Distance(_position, endPosition) + stepLength;

         BHex ownHex = map.GetHex(_position);
         BHex oneHexRule = null; // Ensimmäisen blockin läpi nähtävä, että näkee esim metsään tai talon sisälle.

         while (distanceTravelled < travelLength)
         {
            BHex cpHex = map.GetHex(currentPosition);

            if (oneHexRule != null && cpHex != oneHexRule)
               return false;

            if (cpHex != null && cpHex != ownHex)
            {
               if (map.Occupieds.Contains(new Point(cpHex.X, cpHex.Y)) || cpHex.BlockFov)
               {
                  if (oneHexRule == null)
                     oneHexRule = cpHex;
               }
            }

            distanceTravelled += stepLength;
            currentPosition += step;
         }

         return true;
      }
   }

   public class PathNode
   {
      public int X { get; set; }
      public int Y { get; set; }
      public int Value = 0;
      public PathNode Parent { get; set; }

      public PathNode(int x, int y, int val, PathNode parent)
      {
         X = x;
         Y = y;
         Value = val;
         Parent = parent;
      }

   }

   public class FogOfWar
   {
      private BMap map;
      private Vector2 position;
      private int range;
      private int[,] grid;
      bool calcReady = false;
      private Thread t;

      public FogOfWar()
      {
      }

      public void FindSightGrid(BMap _map, Vector2 _position, int _range)
      {
         map = _map;
         position = _position;
         range = _range;
         calcReady = false;

         t = new Thread(CalculateSightGrid);
         t.IsBackground = true;
         t.Start();
         t.Join(10);
      }

      protected void CalculateSightGrid()
      {
         while (!calcReady)
         {
            grid = BPathfinder.GetSightGrid(map, position, range);
            calcReady = true;
         }
      }

      public int[,] GetSightGrid()
      {
         if (!calcReady)
            return null;

         int[,] returnGrid = new int[grid.GetLength(0), grid.GetLength(1)];
         for (int y = 0; y < grid.GetLength(1); y++)
         {
            for (int x = 0; x < grid.GetLength(0); x++)
            {
               returnGrid[x, y] = grid[x, y];
            }
         }

         return returnGrid;
      }
   }
}
