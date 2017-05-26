using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Assets;

namespace HexTbs.Battle.Unit.Actions
{
   public class SkipAction : BSquadAction
   {
      private Timer timer;

      public SkipAction()
      {
         Cost = 10000;
         timer = new Timer(100);
         timer.Start();
      }

      public override bool IsDone()
      {
         return timer.IsTimedOut();
      }

      public override void Update(GameTime gt)
      {
         timer.Update(gt);
      }
   }
}
