using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets;
using HexTbs.Battle.Unit.SquadModels;

namespace HexTbs.Battle.Unit.Weapons
{
   public class BVehicleWeapon : BWeapon
   {
      public BVehicleWeapon(int range, int accuracy, int firepower, int penetration, int shots, int repeat)
      {
         Range = range;
         Accuracy = accuracy;
         FirePower = firepower;
         Penetration = penetration;
         Repeat = repeat;

         ShotsAmount = shots;
      }

      public BVehicleWeapon(VehicleWeaponModel model)
      {
         Range = model.Range;
         Accuracy = model.Accuracy;
         FirePower = model.FirePower;
         Penetration = model.Penetration;
         Repeat = model.Repeat;
         ShotsAmount = model.ShotsAmount;
      }
   }
}
