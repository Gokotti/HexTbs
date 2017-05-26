using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HexTbs.Battle.Unit.Actions
{
   public abstract class BSquadAction
   {
      public int Cost { get; protected set; }

      public abstract void Update(GameTime gt);
      public abstract bool IsDone();
   }
}
