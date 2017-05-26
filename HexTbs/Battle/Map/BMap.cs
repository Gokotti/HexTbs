using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Assets;
using System.Xml.Serialization;
using System.Xml;
using System.IO;

namespace HexTbs.Battle.Map
{
   public class BMap
   {
      public Rectangle Bounds { get; protected set; }
      public Rectangle HexBounds { get; protected set; }
      private BHex[,] hexes;
      private int hexWidth;
      public BCamera Camera { get; set; }
      public List<Point> Occupieds { get; protected set; }

      public BMap()
      {
         Occupieds = new List<Point>();
      }

      public void Init(int width, int height)
      {
         hexWidth = 64;
         float margin = 32;

         Bounds = new Rectangle(0, 0, 
            width * hexWidth + (hexWidth / 2), 
            (int)((float)((height-1) * (0.85 * (float)hexWidth)) +
            (1.15f * (float)hexWidth)));

         HexBounds = new Rectangle((int)(Bounds.X + margin),
             (int)(Bounds.Y + margin),
             (int)(Bounds.Width + 2 * margin),
             (int)(Bounds.Height + 2 * margin));

         // Väliaikasesti lätkitään jotain tekstuuroita
         List<Texture2D> imgs = new List<Texture2D>();
         imgs.Add(Statics.Textures["Terrain/asphalt"]);
         imgs.Add(Statics.Textures["Terrain/forest"]);
         imgs.Add(Statics.Textures["Terrain/grass"]);
         imgs.Add(Statics.Textures["Terrain/gravel"]);
         imgs.Add(Statics.Textures["Terrain/water"]);
         imgs.Add(Statics.Textures["Terrain/field"]);
         imgs.Add(Statics.Textures["Terrain/building"]);

         Texture2D road1 = Statics.Textures["Terrain/road"];
         Texture2D roadCenter1 = Statics.Textures["Terrain/roadcenter"];

         Texture2D wallimg = Statics.Textures["Terrain/wall"];

         hexes = new BHex[width, height];
         for (int y = 0; y < height; y++)
         {
            for (int x = 0; x < width; x++)
            {
               BHex hex = new BHex();
               hex.InitHex(x, y, hexWidth);
               hexes[x, y] = hex;

               hex.Image = imgs[2];
               hex.RoadImage = road1;
               hex.RoadCenterImage = roadCenter1;
               hex.WallImage = wallimg;
            }
         }

         SetNeighBours();
      }

      public void Init(BMapModel model)
      {
         int width = model.Width;
         int height = model.Height;

         int x = 0;
         int y = 0;

         hexWidth = 64; // 64 tai 96
         float margin = 32;

         Bounds = new Rectangle(0, 0,
            width * hexWidth + (hexWidth / 2),
            (int)((float)((height - 1) * (0.85 * (float)hexWidth)) +
            (1.15f * (float)hexWidth)));

         HexBounds = new Rectangle((int)(Bounds.X + margin),
             (int)(Bounds.Y + margin),
             (int)(Bounds.Width + 2 * margin),
             (int)(Bounds.Height + 2 * margin));

         // Väliaikasesti lätkitään jotain tekstuuroita
         List<BHex> tileset = new List<BHex>();
         tileset.Add(new BHex() { Image = Statics.Textures["Terrain/grass"] });

         BHex forest = new BHex() { Image = Statics.Textures["Terrain/forest"], BlockFov = true, TrackedMoveCost = 3 };
         forest.TreeImage = Statics.Textures["Terrain/tree1"];
         forest.TreePositions.Add(new Vector2(0, 0));
         forest.TreePositions.Add(new Vector2(0, 0));
         forest.TreePositions.Add(new Vector2(0, 0));
         tileset.Add(forest);

         tileset.Add(new BHex() { Image = Statics.Textures["Terrain/gravel"] });
         tileset.Add(new BHex() { Image = Statics.Textures["Terrain/water"] });
         tileset.Add(new BHex() { Image = Statics.Textures["Terrain/field"] });
         tileset.Add(new BHex() { Image = Statics.Textures["Terrain/asphalt"] });

         // Set blank hexes
         hexes = new BHex[width, height];
         for (y = 0; y < height; y++)
         {
            for (x = 0; x < width; x++)
            {
               BHex hex = new BHex();
               hex.InitHex(x, y, hexWidth);
               hexes[x, y] = hex;
               hex.Image = tileset[0].Image;
            }
         }

         y = 0;
         foreach (String str in model.Hexes)
         {
            string[] blocks = str.Split(new char[] { ',' });

            x = 0;
            foreach (string b in blocks)
            {
               BHex copyHex = tileset[Convert.ToInt32(b)];
               hexes[x, y].Image = copyHex.Image;
               hexes[x, y].BlockFov = copyHex.BlockFov;
               hexes[x, y].TrackedMoveCost = copyHex.TrackedMoveCost;
               
               hexes[x, y].TreeImage = copyHex.TreeImage;
               //hexes[x, y].TreePositions.AddRange(copyHex.TreePositions);
               for (int i = 0; i < copyHex.TreePositions.Count; i++)
               {
                  Vector2 pos = new Vector2(Statics.Random.Next(32) - Statics.Random.Next(32), Statics.Random.Next(32) - Statics.Random.Next(32));
                  hexes[x, y].TreePositions.Add(pos);
               }

               x++;
            }

            y++;
         }

         SetNeighBours();
      }

      public void Save()
      {
         BMapModel model = new BMapModel();
         model.Width = hexes.GetLength(0);
         model.Height= hexes.GetLength(1);
         model.Name = "Testing testing";

         model.Hexes = new List<string>();
         /*for (int y = 0; y < hexes.GetLength(0); y++)
         {
            string str = "";

            for (int x = 0; x < hexes.GetLength(1); x++)
            {
               if (x == hexes.GetLength(0) - 1)
                  str += "0";
               else
                  str += "0,";
            }
            model.Hexes.Add(str);
         }*/

         model.P1Starts = new List<BMapModelCoordinate>();
         model.P1Starts.Add(new BMapModelCoordinate() { X = 1, Y = 1 });
         model.P1Starts.Add(new BMapModelCoordinate() { X = 1, Y = 2 });
         model.P1Starts.Add(new BMapModelCoordinate() { X = 1, Y = 3 });
         model.P1Starts.Add(new BMapModelCoordinate() { X = 1, Y = 4 });
         model.P1Starts.Add(new BMapModelCoordinate() { X = 1, Y = 5 });

         model.P2Starts = new List<BMapModelCoordinate>();
         model.P2Starts.Add(new BMapModelCoordinate() { X = 18, Y = 14 });
         model.P2Starts.Add(new BMapModelCoordinate() { X = 18, Y = 15 });
         model.P2Starts.Add(new BMapModelCoordinate() { X = 18, Y = 16 });
         model.P2Starts.Add(new BMapModelCoordinate() { X = 18, Y = 17 });
         model.P2Starts.Add(new BMapModelCoordinate() { X = 18, Y = 18 });

         XmlSerializer xsSubmit = new XmlSerializer(typeof(BMapModel));
         var xml = "";

         using (var sww = new StringWriter())
         {
            using (XmlWriter writer = XmlWriter.Create(sww))
            {
               xsSubmit.Serialize(writer, model);
               xml = sww.ToString(); // Your XML
            }
         }

         Console.WriteLine(xml);
      }

      public BMapModel Load()
      {
         BMapModel model = null;
         string path = "Content/Maps/dummy.xml";

         XmlSerializer serializer = new XmlSerializer(typeof(BMapModel));
         StreamReader reader = new StreamReader(path);
         model = (BMapModel)serializer.Deserialize(reader);
         reader.Close();

         return model;
      }
      
      #region Properties
      public int Height
      {
         get { return hexes.GetLength(1); }
      }

      public int Width
      {
         get { return hexes.GetLength(0); }
      }

      public int HexSize
      {
         get { return hexWidth; }
      }

      #endregion

      #region Methods

      public void Draw(SpriteBatch sp, BCamera cam, int[,] grid)
      {
         PrimitiveLine brush = new PrimitiveLine();
         brush.Color = Color.Yellow;

         for (int y = 0; y < Height; y++)
         {
            for (int x = 0; x < Width; x++)
            {
               BHex hex = GetHex(x, y);

               Rectangle rekt = hex.Rekt;
               rekt.X += HexBounds.X;
               rekt.Y += HexBounds.Y;

               // Kuva
               if (hex.Image != null)
                  sp.Draw(hex.Image, cam.GetRect(rekt), Color.White);

               if (hex.obs)
                  sp.Draw(Statics.Textures["hexfill"], cam.GetRect(rekt), hex.color * 0.5f);

               brush.Color = Color.Black * 0.25f; //hex.color;
               brush.ClearVectors();

               // Tiet
               if (hex.RoadDirections.Count > 0)
               {
                  Vector2 hexpos = new Vector2(HexBounds.X + hex.Rekt.X + hex.Rekt.Width / 2, HexBounds.Y + hex.Rekt.Y + hex.Rekt.Height / 2);
                  Vector2 origoCenter = new Vector2(hex.RoadCenterImage.Width / 2, hex.RoadCenterImage.Height / 2);
                  sp.Draw(hex.RoadCenterImage, cam.GetVector(hexpos), null, Color.White, 0, origoCenter, 0.75f, SpriteEffects.None, 0);

                  foreach (HexDirection hd in hex.RoadDirections)
                  {
                     Vector2 origo = new Vector2(0, hex.RoadImage.Height / 2);
                     float angle = MathHelper.ToRadians(BHex.DirectionToAngle(hd));
                     sp.Draw(hex.RoadImage, cam.GetVector(hexpos), null, Color.White, angle, origo, 0.75f, SpriteEffects.None, 0);
                  }
               }

               Vector2 addp;
               foreach (Vector2 point in hex.Corners)
               {
                  addp = new Vector2(point.X + HexBounds.X, point.Y + HexBounds.Y);
                  brush.AddVector(cam.GetVector(addp));
               }
               addp = new Vector2(hex.Corners[0].X + HexBounds.X, hex.Corners[0].Y + HexBounds.Y);
               brush.AddVector(cam.GetVector(addp));

               brush.Render(sp);

               // Fog
               if (grid != null)
               {
                  Color color = Color.Black * 0.25f;
                  if (grid[x, y] > 0 && grid[x, y] < 9999)
                     color = Color.White * 0f;
                  sp.Draw(Statics.Textures["hexfill"], cam.GetRect(rekt), color);
               }

               //sp.DrawString(Statics.Fonts["arial"], "[" + (hex.X + 1) + "," + (hex.Y + 1) + "]", cam.GetVector(new Vector2(rekt.X + 32, rekt.Y + 32)), Color.Yellow);
               //sp.Draw(Statics.Textures["pixel"], rekt, Color.Blue * 0.5f);
            }
         }

         foreach (BHex hex in hexes)
         {
            if (hex.WallDirections.Count > 0)
            {
               foreach (HexDirection hd in hex.WallDirections)
               {
                  Vector2 hexpos = new Vector2(HexBounds.X + hex.Rekt.X + hex.Rekt.Width / 2, HexBounds.Y + hex.Rekt.Y + hex.Rekt.Height / 2);
                  Vector2 origo = new Vector2(hexWidth * 0.75f, hex.WallImage.Height / 2);
                  float angle = MathHelper.ToRadians(BHex.DirectionToAngle(hd));
                  sp.Draw(hex.WallImage, cam.GetVector(hexpos), null, Color.White, angle, origo, 0.75f, SpriteEffects.None, 0);
               }
            }
         }
      }

      public void DrawProps(SpriteBatch sp, BCamera cam, List<BHex> visibles)
      {
         for (int y = 0; y < Height; y++)
         {
            for (int x = 0; x < Width; x++)
            {
               BHex hex = GetHex(x, y);

               Rectangle rekt = hex.Rekt;
               rekt.X += HexBounds.X;
               rekt.Y += HexBounds.Y;

               // Varjot
               if (hex.TreeImage != null && hex.TreePositions.Count > 0)
               {
                  foreach (Vector2 treePos in hex.TreePositions)
                  {
                     Vector2 hexpos = new Vector2(HexBounds.X + hex.Rekt.X + hex.Rekt.Width / 2, HexBounds.Y + hex.Rekt.Y + hex.Rekt.Height / 2);
                     Vector2 origo = new Vector2(hex.TreeImage.Bounds.Width * 0, hex.TreeImage.Bounds.Height / 2);
                     hexpos += treePos;

                     Color color = Color.Black * 0.5f;
                     if (visibles.Contains(hex))
                        color = Color.White * 0.20f;

                     sp.Draw(hex.TreeImage, cam.GetVector(hexpos), null, color, (float)(Math.PI / 4), origo, 1f, SpriteEffects.None, 0);
                  }
               }

               if (hex.TreeImage != null && hex.TreePositions.Count > 0)
               {
                  foreach (Vector2 treePos in hex.TreePositions)
                  {
                     Vector2 hexpos = new Vector2(HexBounds.X + hex.Rekt.X + hex.Rekt.Width / 2, HexBounds.Y + hex.Rekt.Y + hex.Rekt.Height / 2);
                     Vector2 origo = new Vector2(hex.TreeImage.Bounds.Width / 2, hex.TreeImage.Bounds.Height / 2);
                     hexpos += treePos;

                     Color color = Color.White;
                     if (visibles.Contains(hex))
                        color = Color.White * 0.20f;

                     sp.Draw(hex.TreeImage, cam.GetVector(hexpos), null, color, 0, origo, 1f, SpriteEffects.None, 0);
                  }
               }
            }
         }
      }

      public BHex GetHex(int x, int y)
      {
         if (x < 0 || x >= Width || y < 0 || y >= Height) return null;
         return hexes[x, y];
      }

      /// <summary>
      /// Helper to get hex's rectangle, including origin 
      /// </summary>
      public Rectangle GetHexRekt(BHex hex)
      {
         if (hex == null)
            return new Rectangle();

         Rectangle rekt = hex.Rekt;
         rekt.X += HexBounds.X;
         rekt.Y += HexBounds.Y;
         return rekt;
      }

      public float GetWidthPxl()
      {
         float hexWidth = hexes[0, 0].Rekt.Width;
         return hexWidth * Width;
      }

      public float GetHeightPxl()
      {
         float hexHeight = hexes[0, 0].Rekt.Height;
         return hexHeight * Height;
      }

      public Vector2 GetHexOrigin()
      {
         return new Vector2(HexBounds.X, HexBounds.Y);
      }

      public BHex GetHex(Vector2 coordinates)
      {
         coordinates.X -= HexBounds.X;
         coordinates.Y -= HexBounds.Y;

         int x = (int)coordinates.X;
         int y = (int)coordinates.Y;

         x = x / hexWidth;
         y = (int)(y / (1.5f * hexWidth / Math.Sqrt(3)));

         // Then we correct
         int minx = Math.Max(0, x - 1);
         int maxx = Math.Min(Width, x + 1);
         int miny = Math.Max(0, y - 1);
         int maxy = Math.Min(Height, y + 1);

         for (int _x = minx; _x < maxx; _x++)
         {
            for (int _y = miny; _y < maxy; _y++)
            {
               if (GetHex(_x, _y).CoordInHex(coordinates))
                  return GetHex(_x, _y); // Found the real hex
            }
         }

         return null; // Found nothing...
      }

      public Vector2 GetCoordinates(int x, int y)
      {
         if (x < 0 || x >= Width || y < 0 || y >= Height)
         {
            return new Vector2();
         }

         BHex hex = GetHex(x, y);
         Vector2 coords = new Vector2(hex.Rekt.X + hex.Rekt.Width / 2 + HexBounds.X, hex.Rekt.Y + hex.Rekt.Height / 2 + HexBounds.Y);
         return coords;
      }

      public BHex PointedHex()
      {
         Vector2 mv;
         if (Camera != null)
            mv = Camera.GetCameraVector(MouseController.GetMouseVector());
         else
            mv = MouseController.GetMouseVector();
         return GetHex(mv);

      }

      public HexDirection GetMouseSector()
      {
         if (Camera != null)
            return GetSector(Camera.GetCameraVector(MouseController.GetMouseVector()));

         return GetSector(MouseController.GetMouseVector());
      }

      public HexDirection GetSector(Vector2 pos)
      {
         BHex hex = GetHex(pos);
         if (hex == null)
            return HexDirection.East;

         Vector2 center = new Vector2(hex.Rekt.X + hex.Rekt.Width, hex.Rekt.Y + hex.Rekt.Height);
         float angle = (float)Math.Atan2(pos.Y - center.Y, pos.X - center.X);
         angle = MathHelper.ToDegrees(angle);
         return BHex.AngleToDirection(angle);
      }

      public void InitOccupieds(List<Vector2> positions)
      {
         Occupieds.Clear();
         foreach (Vector2 p in positions)
         {
            BHex hex = GetHex(p);
            Occupieds.Add(new Point(hex.X, hex.Y));
         }
      }

      private void SetNeighBours()
      {
         for (int y = 0; y < hexes.GetLength(1); y++)
         {
            for (int x = 0; x < hexes.GetLength(0); x++)
            {
               BHex hex = hexes[x, y];
               hex.neighbours.Add(GetHex(x - 1, y));
               hex.neighbours.Add(GetHex(x + 1, y));
               hex.neighbours.Add(GetHex(x, y - 1));
               hex.neighbours.Add(GetHex(x, y + 1));

               if (y % 2 == 0)
               {
                  hex.neighbours.Add(GetHex(x - 1, y - 1));
                  hex.neighbours.Add(GetHex(x - 1, y + 1));
               }
               else
               {
                  hex.neighbours.Add(GetHex(x + 1, y - 1));
                  hex.neighbours.Add(GetHex(x + 1, y + 1));
               }

               while (hex.neighbours.Contains(null))
                  hex.neighbours.Remove(null);
            }
         }
      }
      #endregion
   }
}
