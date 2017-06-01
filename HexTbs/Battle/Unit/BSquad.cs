using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Assets;
using Microsoft.Xna.Framework;
using HexTbs.Battle.Unit.Actions;
using HexTbs.Battle.Player;
using HexTbs.Battle.Map;

namespace HexTbs.Battle.Unit
{
   public class BSquad
   {
      public Vector2 Position { get; set; }
      public HexDirection Direction { get; set; }
      protected Texture2D texture;

      public bool IsDead { get; protected set; }
      public bool MoveCompleted { get; protected set; }

      // Actions related
      public List<BSquadAction> Actions { get; protected set; }
      public List<BSquadAction> InterruptActions { get; protected set; }
      public int actionCount { get; protected set; }
      public int actionsUsed { get; protected set; } 

      // Movement related
      public PathNode[,] MoveGrid { get; set; }
      public int MoveRange { get; protected set; }
      public HexMove MoveType { get; protected set; }

      // Weapon related
      public int[,] FireGrid { get; set; }
      public bool FireCompleted { get; protected set; }

      // Visibility and spotting related
      public int SightRange { get; protected set; }
      public int[,] SightGrid { get; set; }
      public List<BSquad> VisibleEnemies { get; protected set; }
      public bool SightCalculated { get; protected set; }

      // Defensive related
      public int Toughness { get; protected set; }
      public int Defence { get; protected set; }

      // Todo
      public FogOfWar fow;
      public bool fowFound = false;
      public FogOfWar fire;
      public bool fireFound = false;

      public BSquad(Vector2 _position)
      {
         Position = _position;
         Direction = (HexDirection)DieRoll.RollDice() - 1;

         List<Texture2D> txts = new List<Texture2D>();
         txts.Add(Statics.Textures["Squads//tank"]);
         txts.Add(Statics.Textures["Squads//ifv"]);
         txts.Add(Statics.Textures["Squads//lighttank"]);

         texture = txts[DieRoll.RollDice(1, 3, -1)];

         Actions = new List<BSquadAction>();
         actionsUsed = 0;
         actionCount = 2;
         InterruptActions = new List<BSquadAction>();

         IsDead = false;
         MoveCompleted = false;
         MoveRange = 6;
         MoveType = HexMove.Tracked;

         FireCompleted = false;

         SightRange = 10;
         VisibleEnemies = new List<BSquad>();
         SightCalculated = false;
      }

      /// <summary>
      /// Vaiheiden kanssa tehtävät alustukset
      /// </summary>
      /// <param name="phase"></param>
      public void InitPhase(BattlePhase phase, BMap map, BPlayer own, BPlayer enemy)
      {
         if (phase == BattlePhase.Move)
         {
            MoveCompleted = false;
            FireCompleted = false;
            InitSights(map, own, enemy);
         }
      }

      /// <summary>
      /// Squadin valinnan yhteydessä tehtävät alustukset
      /// </summary>
      /// <param name="map"></param>
      /// <param name="own"></param>
      /// <param name="enemy"></param>
      public virtual void SelectSquad(BMap map, BPlayer own, BPlayer enemy)
      {
         if (IsDead)
            return;

         actionsUsed = 0;

         List<Vector2> occupieds = new List<Vector2>();
         foreach (BSquad sqd in own.Squads)
            occupieds.Add(sqd.Position);
         foreach (BSquad sqd in enemy.Squads)
            occupieds.Add(sqd.Position);
         map.InitOccupieds(occupieds);

         // Haetaan gridi
         MoveGrid = BPathfinder.GetMoveGrid(map, Position);
         InitSights(map, own, enemy);
      }

      public virtual void InitSights(BMap map, BPlayer own, BPlayer enemy)
      {
         SightCalculated = true;
         VisibleEnemies.Clear();
         foreach (BSquad e in enemy.Squads)
         {
            BHex eHex = map.GetHex(e.Position);
            if (eHex != null && CanSeeHex(map, eHex))
               VisibleEnemies.Add(e);
         }
      }

      public Texture2D GetTexture()
      {
         return texture;
      }

      public void AddAction(BSquadAction action)
      {
         SightCalculated = false;
         actionsUsed += action.Cost;
         Actions.Add(action);
      }

      public bool ActionsOver()
      {
         return actionsUsed >= actionCount;
      }

      public void AddInterruptAction(BSquadAction action)
      {
         InterruptActions.Add(action);
      }

      public bool IsActive()
      {
         return Actions.Count > 0;
      }

      public void SetPosition(Vector2 val) { Position = val; }

      public virtual bool TakeDamage(int damage)
      {
         if (damage == 0)
         {
            /*Toughness--;
            if (Toughness == 0)
            {
               IsDead = true;
               return true;
            }*/
            return false;
         }
         else
         {
            IsDead = true;
            return true;
         }
      }

      public virtual bool CanSeeHex(BMap map, BHex target)
      {
         if (IsDead)
            return false;

         map.Occupieds.Clear();
         bool los = BPathfinder.CanSeeHex(map, Position, target);
         return los;
      }

      public virtual bool CanShootEnemy(BMap map, BPlayer own, BPlayer enemy, BSquad enemyTarget)
      {
         return true;
      }

      public virtual void Draw(SpriteBatch sp, BCamera cam, Color color)
      {
      }

      public virtual void Update(GameTime gt, bool current)
      {

      }
   }
}
