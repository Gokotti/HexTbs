using Assets;
using HexTbs.Battle.Map;
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

      public BVehicleTurret(HexDirection direction, BVehicleWeapon weapon)
      {
         Texture = Statics.Textures["Squads//tank_turret"];
         Direction = direction;
         Weapon = weapon;
         AxisPoint = new Vector2(25, 17);
      }
   }
}
