using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HexTbs.Battle.Unit.SquadModels
{
   [Serializable]
   public class WeaponModel
   {
      public int Range { get; set; }
      public int Accuracy { get; set; }
      public int FirePower { get; set; }
      public int Penetration { get; set; }
      public int ShotsAmount { get; set; }
      public int Repeat { get; set; }
   }
}
