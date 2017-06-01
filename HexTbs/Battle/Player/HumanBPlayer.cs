using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using HexTbs.Battle.Unit;
using HexTbs.Battle.Map;
using Microsoft.Xna.Framework.Graphics;
using Assets;
using Microsoft.Xna.Framework.Input;
using HexTbs.Battle.Unit.Actions;
using HexTbs.Battle.Effects;
using HexTbs.Battle.Unit.SquadModels;

namespace HexTbs.Battle.Player
{
   public class HumanBPlayer : BPlayer
   {
      public override void Init(BMap _map, BPlayer _enemy)
      {
         Effects = new List<BVisualEffect>();
         map = _map;
         enemy = _enemy;
         CurrentSquadIndex = 0;
      }

      VehicleSquadModel vsm = BVehicleSquad.LoadSquadModel();

      public void AddDummies(int x)
      {
         // Lisää ukkoja x-linjalle
         for (int i = 0; i < 3; i++)
         {
            BVehicleSquad sq = new BVehicleSquad(map.GetCoordinates(x, 3 + i), vsm);
            Squads.Add(sq);
         }
      }

      public void AddDummy(int x, int y)
      {
         BVehicleSquad sq = new BVehicleSquad(map.GetCoordinates(x, y), vsm);
         Squads.Add(sq);
      }

      public List<Vector2> Path = null;

      protected KeyboardState oldState;

      public override void Update(GameTime gt, bool current)
      {
         BSquad csq = CurrentSquad; // Lyhempi nimi, ni ei oo nii vaikee lukee

         // Katsotaan, että onko joku kuollut
         if (current && csq != null && csq.IsDead)
         {
            if (NextSquad())
               PlayerTurn = false;
            return;
         }

         KeyboardState newState = Keyboard.GetState();
         if (current && newState.IsKeyDown(Keys.Space) && oldState.IsKeyUp(Keys.Space))
         {
            if (csq != null && !csq.IsActive() && !enemy.IsInterrupting())
               csq.AddAction(new SkipAction());
         }

         // Hiiri
         bool click = MouseController.LeftClicked();
         BHex phex = map.PointedHex();
         HexDirection hexDir = map.GetMouseSector();

         // Viholaiset
         List<BHex> enemyHexes = new List<BHex>();
         BHex eHex = null;
         foreach (BSquad enemy in enemy.Squads)
            enemyHexes.Add(map.GetHex(enemy.Position));

         BSquad enemyTarget = null;
         foreach (BSquad e in enemy.Squads)
         {
            if (map.GetHex(e.Position) == phex)
            {
               enemyTarget = e;
               eHex = phex;
            }
         }

         // Toiminnot
         if (current)
         {
            if (csq != null && !csq.IsActive() && !enemy.IsInterrupting())
            {
               if (click && phex != null)
               {
                  // Haetaan klikatusta ruudusta vihulainen
                  if (enemyTarget != null && !enemyTarget.IsDead && !csq.FireCompleted)
                  {
                     int range = 0;
                     if (csq is BVehicleSquad)
                        range = (csq as BVehicleSquad).MainGun.Range;

                     if (csq.VisibleEnemies != null && csq.VisibleEnemies.Contains(enemyTarget) && 
                        csq.CanShootEnemy(map, this, enemy, enemyTarget))
                     {
                        if (csq is BVehicleSquad)
                        {
                           BVehicleSquad vcsq = csq as BVehicleSquad;
                           if (enemyTarget is BVehicleSquad)
                           {
                              BVehicleSquad evsqd = enemyTarget as BVehicleSquad;
                              csq.AddAction(new VehicleAttackAction(this, vcsq, evsqd, vcsq.MainGun));
                           }
                        }
                     }
                  }
                  else
                  {
                     if (csq.MoveRange >= csq.MoveGrid[phex.X, phex.Y].Value && csq.MoveGrid[phex.X, phex.Y].Value > 0 && !csq.MoveCompleted)
                     {
                        List<Vector2> occupieds = new List<Vector2>();
                        foreach (BSquad sqd in Squads)
                           occupieds.Add(sqd.Position);
                        foreach (BSquad sqd in enemy.Squads)
                           occupieds.Add(sqd.Position);

                        map.InitOccupieds(occupieds);
                        List<Vector2> path = BPathfinder.GetPath(map, csq.Position, map.GetCoordinates(phex.X, phex.Y));
                        if (path != null)
                        {
                           List<Vector2> sPath = new List<Vector2>();
                           Path = path;

                           foreach (Vector2 p in path)
                              sPath.Add(p);

                           csq.AddAction(new VehicleMoveAction(csq as BVehicleSquad, sPath, hexDir, this, enemy, map));
                        }
                     }
                  }
               }
            }

            if (csq != null)
            {
               bool actionOver = csq.ActionsOver();

               csq.Update(gt, current);

               if (!csq.IsActive() && actionOver)
               {
                   if (NextSquad())
                  {
                     // Vika yksikkö liikku
                     PlayerTurn = false;
                  }
               }
               else if (!csq.IsActive() && !actionOver && csq.MoveCompleted && !csq.SightCalculated)
               {
                  csq.InitSights(map, this, enemy);
               }
            }

         }

         // Päivitetään ei muut squadit
         foreach (BSquad s in Squads)
         {
            if ((current && s != CurrentSquad) || !current)
               s.Update(gt, false);
         }

         oldState = newState;
      }
   }
}
