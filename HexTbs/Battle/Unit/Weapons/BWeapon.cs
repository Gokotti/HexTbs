using Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HexTbs.Battle.Unit.Weapons
{
   public abstract class BWeapon
   {
      public int Range { get; protected set; }
      public int Accuracy { get; protected set; }
      public int FirePower { get; protected set; }
      public int Penetration { get; protected set; }
      public int ShotsAmount { get; protected set; }
      public int Repeat { get; protected set; } // Räjähdyksiä varten
   }
}
