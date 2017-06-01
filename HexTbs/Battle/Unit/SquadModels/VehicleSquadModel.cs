using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HexTbs.Battle.Unit.SquadModels
{
   [Serializable]
   public class VehicleSquadModel : SquadModel
   {
      //public BVehicleWeapon MainGun { get; protected set; }
      //public List<BVehicleWeapon> Weapons { get; protected set; }

      // Panssarointi
      public int FrontArmor { get; set; }
      public int SideArmor { get; set; }
      public int RearArmor { get; set; }
   }
}
