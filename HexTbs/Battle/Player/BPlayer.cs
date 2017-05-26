using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HexTbs.Battle.Unit;
using Microsoft.Xna.Framework;
using HexTbs.Battle.Map;
using Microsoft.Xna.Framework.Graphics;
using HexTbs.Battle.Effects;

namespace HexTbs.Battle.Player
{
   public enum BattlePhase { Move }

   public abstract class BPlayer
   {
      public BattlePhase Phase { get; protected set; }
      public bool PlayerTurn { get; protected set; }

      public List<BSquad> Squads = new List<BSquad>();
      protected BPlayer enemy;
      protected BMap map;

      protected int CurrentSquadIndex { get; set; }
      public BSquad CurrentSquad { get; set; }

      public List<BVisualEffect> Effects;

      public abstract void Init(BMap _map, BPlayer _enemy);
      public abstract void Update(GameTime gt, bool current);

      public void InitPhase(BattlePhase phase)
      {
         Phase = phase;

         if (Phase == BattlePhase.Move)
         {
            PlayerTurn = true;
            foreach (BSquad sqd in Squads)
            {
               sqd.InitPhase(phase, map, this, enemy);
            }
            CurrentSquad = Squads[0];
            CurrentSquad.SelectSquad(map, this, enemy);
         }
      }

      public bool IsInterrupting()
      {
         foreach (BSquad s in Squads)
         {
            if (s.InterruptActions.Count > 0)
               return true;
         }

         return false;
      }

      /// <summary>
      /// Select next squad in player squads
      /// </summary>
      /// <returns>True, if last squad</returns>
      public bool NextSquad()
      {
         if (CurrentSquad != null)
         {
            CurrentSquad.InitSights(map, this, enemy);
         }

         bool lastSquad = false;
         CurrentSquadIndex++;
         if (CurrentSquadIndex >= Squads.Count)
         {
            CurrentSquadIndex = 0;
            lastSquad = true;
         }
         CurrentSquad = Squads[CurrentSquadIndex];
         CurrentSquad.SelectSquad(map, this, enemy);

         return lastSquad;
      }
   }
}
