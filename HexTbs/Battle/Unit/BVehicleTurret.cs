using Assets;
using HexTbs.Battle.Map;
using HexTbs.Battle.Unit.SquadModels;
using HexTbs.Battle.Unit.Weapons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HexTbs.Battle.Unit
{
   public class BVehicleTurret
   {
      public Texture2D Texture { get; protected set; }
      public HexDirection Direction { get; protected set; }
      public BVehicleWeapon Weapon { get; protected set; }
      public Vector2 AxisPoint { get; protected set; }
      public BVehicleSquad Squad { get; protected set; }

      public BVehicleTurret(BVehicleSquad squad, HexDirection direction, BVehicleWeapon weapon)
      {
         Squad = squad;
         Texture = Statics.Textures["Squads//tank_turret"];
         Direction = direction;
         Weapon = weapon;
         AxisPoint = new Vector2(25, 17);
      }

      public BVehicleTurret(BVehicleSquad squad, HexDirection direction, VehicleTurretModel model)
      {
         Squad = squad;
         Direction = direction;
         AxisPoint = new Vector2(model.AxisPoint.X, model.AxisPoint.Y);
         Texture = Statics.Textures[model.Texture];
         Weapon = new BVehicleWeapon(model.Weapon);
      }

      public void TurnTurret(HexDirection direction)
      {
         Direction = direction;
      }

      public void TurnTurret(Vector2 enemy)
      {
         float radians = (float)Math.Atan2(enemy.Y - Squad.Position.Y, enemy.X - Squad.Position.X);
         float degree = MathHelper.ToDegrees(radians);
         Direction = BHex.AngleToDirection(degree);
      }
   }
}
