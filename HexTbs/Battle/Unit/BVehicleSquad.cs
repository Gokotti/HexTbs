using Assets;
using HexTbs.Battle.Unit.Actions;
using HexTbs.Battle.Unit.Weapons;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HexTbs.Battle.Map;
using HexTbs.Battle.Player;
using Microsoft.Xna.Framework.Graphics;
using HexTbs.Battle.Unit.SquadModels;
using System.Xml.Serialization;
using System.IO;

namespace HexTbs.Battle.Unit
{
   public class BVehicleSquad : BSquad
   {
      public BVehicleWeapon MainGun { get; protected set; }
      public List<BVehicleWeapon> Weapons { get; protected set; }

      public BVehicleTurret MainTurret { get; protected set; }

      // Panssarointi
      public int FrontArmor { get; protected set; }
      public int SideArmor { get; protected set; }
      public int RearArmor { get; protected set; }

      public BVehicleSquad(Vector2 _position) : base(_position)
      {
         MainGun = new BVehicleWeapon(20, 10, 6, 3, 1, 1);
         Weapons = new List<BVehicleWeapon>();

         Defence = 2;
         Toughness = 5;

         FrontArmor = 5;
         SideArmor = 4;
         RearArmor = 3;

         MoveRange = 6;
      }

      public BVehicleSquad(Vector2 _position, VehicleSquadModel model) : base(_position)
      {
         MainGun = new BVehicleWeapon(20, 10, 6, 3, 1, 1);
         Weapons = new List<BVehicleWeapon>();

         Defence = model.Defence;
         Toughness = model.Toughness;
         FrontArmor = model.FrontArmor;
         SideArmor = model.SideArmor;
         RearArmor = model.RearArmor;
         MoveRange = model.MoveRange;
         actionCount = model.Actions;
         MoveRange = model.MoveRange;
         MoveType = model.MoveType;
         SightRange = model.SightRange;

         texture = Statics.Textures["Squads//tank"];

         Direction = (HexDirection)DieRoll.RollDice() - 1;

         MainTurret = new BVehicleTurret(Direction, MainGun);
      }

      public static VehicleSquadModel LoadSquadModel()
      {
         VehicleSquadModel model = null;
         string path = "Content/Squads/vehtest.xml";

         XmlSerializer serializer = new XmlSerializer(typeof(VehicleSquadModel));
         StreamReader reader = new StreamReader(path);
         model = (VehicleSquadModel)serializer.Deserialize(reader);
         reader.Close();

         return model;
      }

      public override void SelectSquad(BMap map, BPlayer own, BPlayer enemy)
      {
         base.SelectSquad(map, own, enemy);
      }

      public override bool CanShootEnemy(BMap map, BPlayer own, BPlayer enemy, BSquad enemyTarget)
      {
         if (IsDead)
            return false;

         BHex target = map.GetHex(enemyTarget.Position);

         map.Occupieds.Clear();
         /*foreach (BSquad sqd in own.Squads)
         {
            if (sqd != this)
            {
               BHex hex = map.GetHex(sqd.Position);
               map.Occupieds.Add(new Point(hex.X, hex.Y));
            }
         }

         foreach (BSquad sqd in enemy.Squads)
         {
            if (sqd != enemyTarget)
            {
               BHex hex = map.GetHex(sqd.Position);
               map.Occupieds.Add(new Point(hex.X, hex.Y));
            }
         }*/

         bool los = BPathfinder.CanSeeHex(map, Position, target);

         if (los)
         {
            int[,] grid = BPathfinder.GetRangeGrid(map, Position, MainGun.Range);
            if (grid[target.X, target.Y] <= MainGun.Range)
            {
               return true;
            }
         }

         return false;
      }

      public override bool TakeDamage(int damage)
      {
         return base.TakeDamage(damage);
      }

      public override void Draw(SpriteBatch sp, BCamera cam, Color color)
      {
         float angle = MathRoutines.DegreeToRadian(BHex.DirectionToAngle(Direction));
         sp.Draw(GetTexture(), cam.GetVector(Position), null, color, angle, Statics.GetTextureOrigin(GetTexture()), 0.75f, SpriteEffects.None, 0);

         float turretAngle = MathRoutines.DegreeToRadian(BHex.DirectionToAngle(MainTurret.Direction));
         sp.Draw(MainTurret.Texture, cam.GetVector(Position), null, color, turretAngle, MainTurret.AxisPoint, 0.75f, SpriteEffects.None, 0);
      }

      public override void Update(GameTime gt, bool current)
      {
         base.Update(gt, current);

         if (current)
         {
            if (Actions.Count > 0)
            {
               BSquadAction action = Actions.First();
               if (action.IsDone())
               {
                  if (action is VehicleMoveAction)
                     MoveCompleted = true;
                  else if (action is VehicleAttackAction)
                     FireCompleted = true;
                  Actions.RemoveAt(0);
               }
               action.Update(gt);
            }
         }

         if (InterruptActions.Count > 0)
         {
            BSquadAction action = InterruptActions.First();
            action.Update(gt);

            if (action.IsDone())
            {
               if (action is VehicleInterruptAttackAction)
                  FireCompleted = true;

               InterruptActions.RemoveAt(0);
            }
         }
      }
   }
}
