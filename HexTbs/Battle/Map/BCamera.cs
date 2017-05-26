using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HexTbs.Battle.Map
{
   public class BCamera
   {
      public Rectangle Screen { get; protected set; }
      public Rectangle CamRekt { get; protected set; }
      public BMap Map { get; protected set; }

      public BCamera(Rectangle screenSize, BMap map)
      {
         Screen = screenSize;
         CamRekt = screenSize;
         Map = map;
      }

      public void MoveCamera(Vector2 step)
      {
         CamRekt = new Rectangle(CamRekt.X + (int)step.X, CamRekt.Y + (int)step.Y, CamRekt.Width, CamRekt.Height);
         int mapWidth = Map.HexBounds.Width;
         int mapHeight = Map.HexBounds.Height;

         if (CamRekt.X < Map.Bounds.X)
            CamRekt = new Rectangle(Map.Bounds.X, CamRekt.Y, CamRekt.Width, CamRekt.Height);
         if (CamRekt.X + CamRekt.Width > mapWidth)
            CamRekt = new Rectangle(mapWidth - CamRekt.Width, CamRekt.Y, CamRekt.Width, CamRekt.Height);

         if (CamRekt.Y < Map.Bounds.Y)
            CamRekt = new Rectangle(CamRekt.X, Map.Bounds.Y, CamRekt.Width, CamRekt.Height);
         if (CamRekt.Y + CamRekt.Height > mapHeight)
            CamRekt = new Rectangle(CamRekt.X, mapHeight - CamRekt.Height, CamRekt.Width, CamRekt.Height);
      }

      public Rectangle GetRect(Rectangle r)
      {
         float x = r.X;
         float y = r.Y;
         float w = r.Width + 1;
         float h = r.Height + 1;

         x -= CamRekt.X;
         y -= CamRekt.Y;

         return new Rectangle((int)x, (int)y, (int)w, (int)h);
      }

      public Vector2 GetVector(Vector2 v)
      {
         return new Vector2(v.X - CamRekt.X, v.Y - CamRekt.Y);
      }

      public Vector2 GetCameraVector(Vector2 v)
      {
         return new Vector2(v.X + CamRekt.X, v.Y + CamRekt.Y);
      }
   }
}
