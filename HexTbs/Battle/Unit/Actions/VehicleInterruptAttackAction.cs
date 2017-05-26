using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Assets;
using HexTbs.Battle.Player;
using HexTbs.Battle.Effects;
using HexTbs.Battle.Unit.Weapons;
using HexTbs.Battle.Map;

namespace HexTbs.Battle.Unit.Actions
{
   public class VehicleInterruptAttackAction : VehicleAttackAction
   {
      protected BProjectileAnimation projectile;
      protected BExplosionEffect explosion;

      public VehicleInterruptAttackAction(BPlayer player, BVehicleSquad squad, BSquad enemy, BVehicleWeapon weapon) : base(player, squad, enemy,weapon)
      {
      }

      public override bool IsDone()
      {
         return over;
      }

      public override void Update(GameTime gt)
      {
         base.Update(gt);
      }
   }
}
