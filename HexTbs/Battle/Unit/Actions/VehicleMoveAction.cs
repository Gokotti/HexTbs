using HexTbs.Battle.Map;
using HexTbs.Battle.Player;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HexTbs.Battle.Unit.Actions
{
   public class VehicleMoveAction : BSquadAction
   {
      protected BVehicleSquad squad;
      protected List<Vector2> Path { get; set; }
      protected Vector2 moveTarget;
      protected bool pathReady;
      protected HexDirection endDirection;

      // Interruptiokohtaiset
      protected BPlayer enemy;
      protected BPlayer own;
      protected BMap map;

      public VehicleMoveAction(BVehicleSquad _squad, List<Vector2> _path, HexDirection dir, BPlayer _own, BPlayer _enemy, BMap _map)
      {
         Cost = 1;

         squad = _squad;
         moveTarget = Vector2.Zero;
         Path = _path;
         pathReady = false;
         endDirection = dir;

         // Interruptiot
         own = _own;
         enemy = _enemy;
         map = _map;
      }

      public VehicleMoveAction(VehicleMoveAction vma) : this(vma.squad, vma.Path, vma.endDirection, vma.own, vma.enemy, vma.map)
      {
         
      }

      public override bool IsDone()
      {
         bool moveDone = moveTarget == Vector2.Zero;
         return pathReady && moveDone;
      }

      public override void Update(GameTime gt) 
      {
         float speed = 75f / (float)gt.ElapsedGameTime.Milliseconds;

         // Normaali pysähtyminen, kun polku kuljettu
         if (Path.Count == 0 && moveTarget == Vector2.Zero)
         {
            //squad.Direction = endDirection;
            pathReady = true;
         }

         // Haetaan polusta askel
         if (moveTarget == Vector2.Zero && Path.Count > 0)
         {
            moveTarget = Path.First();
            Path.RemoveAt(0);

            float radians = (float)Math.Atan2(moveTarget.Y - squad.Position.Y, moveTarget.X - squad.Position.X);
            float degree = MathHelper.ToDegrees(radians);
            squad.Direction = BHex.AngleToDirection(degree);
            squad.MainTurret.TurnTurret(squad.Direction);
         }

         // Liikutaan kohti askelta
         if (moveTarget != Vector2.Zero && !pathReady)
         {
            if (squad.Position != moveTarget)
            {
               Vector2 step = moveTarget - squad.Position;
               step.Normalize();
               step *= speed;

               if (step.Length() > Vector2.Distance(squad.Position, moveTarget))
               {
                  squad.Position = moveTarget;
                  Interrupt();
               }
               else
               {
                  squad.Position += step;
               }
            }
            else
            {
               squad.Position = moveTarget;
               moveTarget = Vector2.Zero;
            }
         }
      }

      public void Interrupt()
      {
         foreach (BSquad e in enemy.Squads)
         {
            if (e.CanShootEnemy(map, enemy, own, squad) && !e.FireCompleted)
            {
               if (e is BVehicleSquad)
               {
                  e.AddInterruptAction(new VehicleInterruptAttackAction(enemy, (e as BVehicleSquad), squad, (e as BVehicleSquad).MainTurret.Weapon));
                  Path.Clear();
               }
            }
         }
      }
   }
}
