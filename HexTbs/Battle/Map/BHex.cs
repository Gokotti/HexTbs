using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HexTbs.Battle.Map
{
   public enum HexDirection { East, SouthEast, SouthWest, West, NorthWest, NorthEast }
   public enum HexMove { Foot, Wheeled, Tracked }

   public class BHex
   {
      private Rectangle _rekt;
      public Rectangle Rekt { get { return _rekt; } }
      public List<Vector2> Corners { get; set; }
      public List<BHex> neighbours = new List<BHex>();

      public Texture2D Image { get; set; }

      // Tiet
      public Texture2D RoadImage { get; set; }
      public Texture2D RoadCenterImage { get; set; }
      public List<HexDirection> RoadDirections { get; protected set; }

      // Seinät
      public Texture2D WallImage { get; set; }
      public List<HexDirection> WallDirections { get; protected set; }

      // Puut
      public Texture2D TreeImage { get; set; }
      public List<Vector2> TreePositions { get; set; }

      public Color color = Color.White;

      int xc;
      int yc;

      // Statsit
      public bool BlockFov { get; set; }
      public int FootMoveCost { get; set; }
      public int WheeledMoveCost { get; set; }
      public int TrackedMoveCost { get; set; }
      public int DirectCover { get; set; }
      public int InDirectCover { get; set; }

      public BHex()
      {
         RoadDirections = new List<HexDirection>();
         WallDirections = new List<HexDirection>();

         BlockFov = false;
         FootMoveCost = 1;
         WheeledMoveCost = 1;
         TrackedMoveCost = 1;
         DirectCover = 0;
         InDirectCover = 0;

         TreePositions = new List<Vector2>();
      }

      public bool obs = false;

      public int X { get { return xc; } }
      public int Y { get { return yc; } }

      public void InitHex(int x, int y, int hw)
      {
         // Coords
         xc = x;
         yc = y;

         // Rectangle
         int _x = hw * x;
         int _h = (int)(2 * hw / Math.Sqrt(3));
         int _y = (int)((float)_h * 0.75) * y;
         int _w = hw;

         if (y % 2 != 0)
            _x += hw / 2;

         _rekt = new Rectangle(_x, _y, _w, _h);

         // Corners
         List<Vector2> corners = new List<Vector2>();
         corners.Add(new Vector2(_x + _w / 2, _y));
         corners.Add(new Vector2(_x + _w, _y + _h / 4));
         corners.Add(new Vector2(_x + _w, _y + (0.75f * _h)));
         corners.Add(new Vector2(_x + _w / 2, _y + _h));
         corners.Add(new Vector2(_x, _y + (0.75f * _h)));
         corners.Add(new Vector2(_x, _y + (_h / 4)));
         Corners = corners;
      }

      public bool CoordInHex(Vector2 coord)
      {
         System.Drawing.Drawing2D.GraphicsPath triangle = new System.Drawing.Drawing2D.GraphicsPath();

         System.Drawing.Point cp = new System.Drawing.Point((int)coord.X, (int)coord.Y);

         triangle.AddLine(Corners[0].X, Corners[0].Y, Corners[1].X, Corners[1].Y);
         triangle.AddLine(Corners[1].X, Corners[1].Y, Corners[2].X, Corners[2].Y);
         triangle.AddLine(Corners[2].X, Corners[2].Y, Corners[3].X, Corners[3].Y);
         triangle.AddLine(Corners[3].X, Corners[3].Y, Corners[4].X, Corners[4].Y);
         triangle.AddLine(Corners[4].X, Corners[4].Y, Corners[5].X, Corners[5].Y);
         triangle.AddLine(Corners[5].X, Corners[5].Y, Corners[0].X, Corners[0].Y);

         if (triangle.IsVisible(cp))
         {
            return true;
         }

         return false;
      }

      public void SetRoad(List<HexDirection> road)
      {
         RoadDirections.AddRange(road);
      }

      public void SetWall(List<HexDirection> wall)
      {
         WallDirections.AddRange(wall);
      }

      #region Statics
      public static float GetRange(BHex a, BHex b)
      {
         if (a == null || b == null) return -1;
         return (int)Vector2.Distance(new Vector2(a.X, a.Y), new Vector2(b.X, b.Y));
      }

      public static float DirectionToAngle(HexDirection dir)
      {
         float angle = 0;
         switch (dir)
         {
            case HexDirection.East: angle = 0; break;
            case HexDirection.SouthEast: angle = 60; break;
            case HexDirection.SouthWest: angle = 120; break;
            case HexDirection.West: angle = 180; break;
            case HexDirection.NorthWest: angle = 240; break;
            case HexDirection.NorthEast: angle = 300; break;
         }

         return angle;
      }

      public static HexDirection AngleToDirection(float angle)
      {
         float a = angle;
         while (a < 0)
            a += 360;


         if (a >= 330 || a < 30)
            return HexDirection.East;
         else if (a >= 30 && a < 90)
            return HexDirection.SouthEast;
         else if (a >= 90 && a < 150)
            return HexDirection.SouthWest;
         else if (a >= 150 && a < 210)
            return HexDirection.West;
         else if (a >= 210 && a < 270)
            return HexDirection.NorthWest;

         return HexDirection.NorthEast;
      }

      public static int DirectionDifference(HexDirection a, HexDirection b)
      {
         if (a == b)
            return 0;

         int diff = a - b;

         if (diff > 3)
            diff = 6 - diff;
         else if (diff < 0 && diff >= -3)
            diff = Math.Abs(diff);
         else if (diff < 0 && diff < -3)
            diff = 6 - Math.Abs(diff);

         return diff;
      }
      #endregion
   }
}
