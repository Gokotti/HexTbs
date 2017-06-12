using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HexTbs.Battle.Unit.SquadModels
{
   [Serializable]
   public class VehicleSquadModel : SquadModel
   {
      public VehicleTurretModel Turret { get; set; }

      public string Texture { get; set; }

      // Panssarointi
      public int FrontArmor { get; set; }
      public int SideArmor { get; set; }
      public int RearArmor { get; set; }
   }

   [Serializable]
   public class VehicleTurretModel
   {
      public VehicleWeaponModel Weapon { get; set; }
      public ModelCoordinate AxisPoint { get; set; }
      public string Texture { get; set; }
      //public BVehicleSquad Squad { get; protected set; }
   }
}
