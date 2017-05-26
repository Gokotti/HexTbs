using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Assets;
using HexTbs.Battle.Map;
using HexTbs.Battle.Player;
using HexTbs.Battle.Unit;
using Microsoft.Xna.Framework.Input;
using HexTbs.Battle.Effects;

namespace HexTbs.Battle
{
   class BattleFrame : Frame
   {
      private Rectangle screen;
      public bool drawDirection = false;

      private BPlayer p1;
      private BPlayer p2;
      private BPlayer currentPlayer = null;

      public BMap Map { get; set; }
      public BCamera Camera { get; set; }
      private Texture2D background;

      public List<BVisualEffect> effects = new List<BVisualEffect>();

      public BattleFrame()
      {
         Map = new BMap();
         Init();
      }

      public void Init()
      {
         screen = new Rectangle(0, 0, Statics.ScreenSize.X, Statics.ScreenSize.Y);
         BMapModel bmm = Map.Load();
         Map.Init(bmm); // 16 11
         Camera = new BCamera(screen, Map);
         Map.Camera = Camera;

         Map.Save();

         p1 = new HumanBPlayer();
         p2 = new HumanBPlayer();
         p1.Init(Map, p2);
         p2.Init(Map, p1);

         foreach (BMapModelCoordinate bmmc in bmm.P1Starts)
            (p1 as HumanBPlayer).AddDummy(bmmc.X, bmmc.Y);
         foreach (BMapModelCoordinate bmmc in bmm.P2Starts)
            (p2 as HumanBPlayer).AddDummy(bmmc.X, bmmc.Y);

         p1.InitPhase(BattlePhase.Move);
         currentPlayer = p1;
         currentPlayer.CurrentSquad.SelectSquad(Map, p1, p2);

         background = Statics.Textures["BattleBg//marstest"];
      }

      public override void Draw(SpriteBatch sp)
      {
         BHex pointedHex = Map.PointedHex();
         BSquad csqd = currentPlayer.CurrentSquad;
         HexDirection hexDir = Map.GetMouseSector();

         List<BHex> visibles = new List<BHex>();
         if (pointedHex != null)
         {
            visibles.Add(pointedHex);
         }

         sp.Begin();

         sp.Draw(Statics.Textures["pixel"], screen, Color.Gray);
         Map.Draw(sp, Camera, csqd.SightGrid);

         if (currentPlayer != null && csqd != null && !csqd.IsDead)
         {
            BHex hex = Map.GetHex(csqd.Position);
            if (hex != null)
               sp.Draw(Statics.Textures["hexfill"], Camera.GetRect(Map.GetHexRekt(hex)), Color.Red * 0.75f);

            // Liikkuma-alue
            if (csqd.MoveGrid != null && !csqd.IsActive())
            {
               PathNode[,] grid = csqd.MoveGrid;
               for (int y = 0; y < grid.GetLength(0); y++)
               {
                  for (int x = 0; x < grid.GetLength(1); x++)
                  {
                     if (grid[x, y].Value <= csqd.MoveRange && grid[x, y].Value > 0)
                     {
                        BHex gridHex = Map.GetHex(x, y);

                        //visibles.Add(gridHex);

                        BHex parentHex = Map.GetHex(grid[x, y].Parent.X, grid[x, y].Parent.Y);

                        sp.Draw(Statics.Textures["hexfill"], Camera.GetRect(Map.GetHexRekt(gridHex)), Color.YellowGreen * 0.25f);
                        sp.DrawString(Statics.Fonts["arial"], grid[x, y].Value.ToString(), Camera.GetVector(Map.GetCoordinates(x, y)), Color.Yellow);

                        PrimitiveLine pl = new PrimitiveLine();
                        pl.Color = Color.Black * 0.25f;
                        pl.AddVector(Camera.GetVector(Map.GetCoordinates(gridHex.X, gridHex.Y)));
                        pl.AddVector(Camera.GetVector(Map.GetCoordinates(parentHex.X, parentHex.Y)));
                        pl.Render(sp);

                     }
                     else if (grid[x, y].Value <= csqd.MoveRange + 3 && grid[x, y].Value > 0)
                     {
                        sp.DrawString(Statics.Fonts["arial"], grid[x, y].Value.ToString(), Camera.GetVector(Map.GetCoordinates(x, y)), Color.Red);
                     }
                  }
               }
            }
         }

         // Tulilinja
         if (pointedHex != null && csqd != null && !csqd.IsActive())
         {
            BHex cHex = Map.GetHex(csqd.Position);
            PrimitiveLine pl = new PrimitiveLine();
            pl.Color = Color.Red;
            pl.AddVector(Camera.GetVector(Map.GetCoordinates(pointedHex.X, pointedHex.Y)));
            pl.AddVector(Camera.GetVector(Map.GetCoordinates(cHex.X, cHex.Y)));
            pl.Render(sp);
         }

         foreach (BSquad sqd in p1.Squads)
         {
            BHex sqdHex = Map.GetHex(sqd.Position);

            bool teamCanSee = false;
            if (currentPlayer == p1)
               teamCanSee = true;
            else
            {
               foreach (BSquad s in p2.Squads)
               {
                  if (!s.IsDead && s.VisibleEnemies != null && s.VisibleEnemies.Contains(sqd))
                     teamCanSee = true;
               }
            }

            bool canSee = false;
            if (csqd.VisibleEnemies != null && csqd.VisibleEnemies.Contains(sqd))
               canSee = true;

            Color color = Color.White;
            if (sqd.IsDead)
               color = Color.Blue;
            else if (currentPlayer == p1)
               color = Color.White;
            else if (!canSee)
               color = Color.DarkGray;

            if (teamCanSee || sqd.IsDead)
            {
               sqd.Draw(sp, Camera, color);

               string debugTxt = BHex.DirectionToAngle(sqd.Direction).ToString();
               //sp.DrawString(Statics.Fonts["arial"], debugTxt, Camera.GetVector(sqd.Position), Color.Yellow);

               if (sqdHex != null)
               {
                  sp.Draw(Statics.Textures["hexlines"], Camera.GetRect(Map.GetHexRekt(sqdHex)), Color.Red);
                  if (!sqd.IsDead)
                     visibles.Add(sqdHex);
               }
            }
            else if (sqdHex != null)
            {
               sp.Draw(Statics.Textures["unknown"], Camera.GetRect(Map.GetHexRekt(sqdHex)), Color.White * 0.5f);
               visibles.Add(sqdHex);
            }
         }

         foreach (BSquad sqd in p2.Squads)
         {
            BHex sqdHex = Map.GetHex(sqd.Position);

            bool teamCanSee = false;
            if (currentPlayer == p2)
               teamCanSee = true;
            else
            {
               foreach (BSquad s in p1.Squads)
               {
                  if (!s.IsDead && s.VisibleEnemies != null && s.VisibleEnemies.Contains(sqd))
                     teamCanSee = true;
               }
            }

            bool canSee = false;
            if (csqd.VisibleEnemies != null && csqd.VisibleEnemies.Contains(sqd))
               canSee = true;

            Color color = Color.White;
            if (sqd.IsDead)
               color = Color.Blue;
            else if (currentPlayer == p2)
               color = Color.White;
            else if (!canSee)
               color = Color.DarkGray;

            if (teamCanSee || sqd.IsDead)
            {
               sqd.Draw(sp, Camera, color);

               string debugTxt = BHex.DirectionToAngle(sqd.Direction).ToString();
               sp.DrawString(Statics.Fonts["arial"], debugTxt, Camera.GetVector(sqd.Position), Color.Yellow);

               if (sqdHex != null)
               {
                  sp.Draw(Statics.Textures["hexlines"], Camera.GetRect(Map.GetHexRekt(sqdHex)), Color.Blue);
                  if (!sqd.IsDead)
                     visibles.Add(sqdHex);
               }
            }
            else if (sqdHex != null)
            {
               sp.Draw(Statics.Textures["unknown"], Camera.GetRect(Map.GetHexRekt(sqdHex)), Color.White * 0.5f);
               visibles.Add(sqdHex);
            }
         }

         sp.Draw(Statics.Textures["hexlines"], Camera.GetRect(Map.GetHexRekt(pointedHex)), Color.Black);

         if (pointedHex != null && drawDirection)
         {
            Texture2D txt = Statics.Textures["direction"];
            float angle = MathRoutines.DegreeToRadian(BHex.DirectionToAngle(hexDir));
            Vector2 dirOrigin = new Vector2(txt.Width / 2, txt.Height / 2);
            sp.Draw(txt, Camera.GetVector(Map.GetCoordinates(pointedHex.X, pointedHex.Y)), null, Color.White, angle, dirOrigin, 0.65f, SpriteEffects.None, 0);
         }

         Map.DrawProps(sp, Camera, visibles);

         foreach (BVisualEffect e in effects)
         {
            e.Draw(sp, Camera);
         }

         sp.End();
      }

      public override void Update(GameTime gt)
      {
         if (Keyboard.GetState().IsKeyDown(Keys.Right))
            Camera.MoveCamera(new Vector2(15, 0));
         if (Keyboard.GetState().IsKeyDown(Keys.Left))
            Camera.MoveCamera(new Vector2(-15, 0));
         if (Keyboard.GetState().IsKeyDown(Keys.Up))
            Camera.MoveCamera(new Vector2(0, -15));
         if (Keyboard.GetState().IsKeyDown(Keys.Down))
            Camera.MoveCamera(new Vector2(0, 15));

         if (currentPlayer == p1)
         {
            p1.Update(gt, true);
            p2.Update(gt, false);
         }
         else if (currentPlayer == p2)
         {
            p2.Update(gt, true);
            p1.Update(gt, false);
         }

         if (currentPlayer == p1 && !p1.PlayerTurn)
         {
            Console.WriteLine("VUORON VAIHTO tokalle!");
            p2.InitPhase(BattlePhase.Move);
            currentPlayer = p2;
         }
         else if (currentPlayer == p2 && !p2.PlayerTurn)
         {
            Console.WriteLine("VUORON VAIHTO ekalle!");
            p1.InitPhase(BattlePhase.Move);
            currentPlayer = p1;
         }

         if (p1.Effects.Count > 0)
         {
            effects.AddRange(p1.Effects);
            p1.Effects.Clear();
         }
         if (p2.Effects.Count > 0)
         {
            effects.AddRange(p2.Effects);
            p2.Effects.Clear();
         }


         List<BVisualEffect> removes = new List<BVisualEffect>();
         foreach (BVisualEffect e in effects)
         {
            e.Update(gt);
            if (e.IsOver())
               removes.Add(e);
         }
         foreach (BVisualEffect e in removes)
            effects.Remove(e);
      }

   }
}
