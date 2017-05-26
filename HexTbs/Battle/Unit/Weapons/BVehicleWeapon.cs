using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets;

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
         ShotsAmount = shots;
         Repeat = repeat;
      }
   }
}
