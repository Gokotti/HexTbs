using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Assets;
using HexTbs.Battle.Effects;
using HexTbs.Battle.Player;
using HexTbs.Battle.Unit.Weapons;
using HexTbs.Battle.Map;

namespace HexTbs.Battle.Unit.Actions
{
   public class VehicleAttackAction : BSquadAction
   {
      protected BPlayer Player { get; set; }
      protected BVehicleSquad Squad { get; set; }
      protected BSquad Enemy { get; set; }
      protected BVehicleWeapon Weapon {get; set; }

      protected bool hit = false;
      protected bool over = false;
      protected int damageGiven = 0;

      protected List<BProjectileAnimation> projectiles = new List<BProjectileAnimation>();
      protected List<BExplosionEffect> explosions = new List<BExplosionEffect>();

      protected int shots;
      protected int shotsFired;

      public VehicleAttackAction(BPlayer player, BVehicleSquad squad, BSquad enemy, BVehicleWeapon weapon)
      {
         Cost = 2;

         Player = player;
         Squad = squad;
         Enemy = enemy;
         Weapon = weapon;

         // Projektiilit
         //projectiles = new List<BProjectileAnimation>();
         //explosions = new List<BExplosionEffect>();
         for (int i = 0; i < Weapon.ShotsAmount; i++)
         {
            int xplus = DieRoll.RollNormalDice(5, 1, 40, 0) - DieRoll.RollNormalDice(5, 1, 40, 0);
            int yplus = DieRoll.RollNormalDice(5, 1, 40, 0) - DieRoll.RollNormalDice(5, 1, 40, 0);
            BProjectileAnimation proj = new  BProjectileAnimation(Squad.Position, new Vector2(enemy.Position.X + xplus, enemy.Position.Y + yplus));
            proj.SetDelay(i * 100);
            projectiles.Add(proj);

         }
         Player.Effects.AddRange(projectiles);

         // Heitetään nopat
         int attackRoll = DieRoll.RollDice(1, 20, 0);
         int damageRoll = DieRoll.RollDice(1, 10, 0);

         int attack = GetAttack();
         int defend = GetDefend();

         int penetration = GetPenetration();
         int armor = GetArmour();

         int damage = GetDamage();
         int toughness = GetToughness();

         PrintHitChances(attack, defend);
         PrintDamageChances(penetration, armor, damage, toughness);

         int attackGiven = attack - defend + attackRoll - 10;
         if (attackGiven >= 0 && attackRoll != 1)
            hit = true;

         if (enemy is BVehicleSquad)
         {
            int armorSave = armor - penetration;
            if (armorSave < 0)
               armorSave = 0;

            damageGiven = damage - (armorSave + toughness) + damageRoll - 5;

            //if (hit && damageGiven > 0 && damageRoll != 1)
            //   damaged = true;
         }
         else
         {
            damageGiven = damage - toughness + damageRoll - 5;
            //if (hit && damageGiven > 0  && damageRoll != 1)
               //damaged = true;
         }

         if (damageGiven < 0)
            damageGiven = 0;
      }

      protected void PrintHitChances(float a, float d)
      {
         float prob = (10f + (a - d)) / 20f;
         prob *= 100;

         if (prob >= 100)
            prob = 95;

         Console.WriteLine("HitChange: " + prob + "%");
      }

      protected void PrintDamageChances(float p, float a, float d, float t)
      {
         float arms = a - p;
         if (arms < 0)
            arms = 0;

         float prob = (5f + (d - t - arms)) / 10f;
         prob *= 100;

         if (prob >= 100)
            prob = 95;

         Console.WriteLine("DamageChange: " + prob + "%");
      }

      protected int GetAttack()
      {
         int attack = Weapon.Accuracy;
         return attack;
      }

      protected int GetDefend()
      {
         int defend = Enemy.Defence;
         return defend;
      }

      protected int GetDamage()
      {
         int damage = Weapon.FirePower;
         return damage;
      }

      protected int GetToughness()
      {
         int toughness = Enemy.Toughness;
         return toughness;
      }

      protected int GetArmour()
      {
         if (Enemy is BVehicleSquad)
         {
            BVehicleSquad e = Enemy as BVehicleSquad;
            HexDirection pAngle = BHex.AngleToDirection(MathRoutines.RadianToDegree(projectiles[0].Angle));
            HexDirection eAngle = e.Direction;

            int diff = BHex.DirectionDifference(pAngle, eAngle);
            if (diff == 0)
            {
               Console.WriteLine("REAR!");
               return e.RearArmor;
            }
            else if (diff == 1 || diff == 2)
            {
               Console.WriteLine("SIDE!");
               return e.SideArmor;
            }

            Console.WriteLine("FRONT!");
            return e.FrontArmor;
         }
         
         return 0;
      }

      protected int GetPenetration()
      {
         int penetration = Weapon.Penetration;
         return penetration;
      }

      public override bool IsDone()
      {
         return over;
      }

      public override void Update(GameTime gt)
      {
         bool projOver = true;
         foreach (BProjectileAnimation pj in projectiles)
         {
            if (!pj.IsOver())
               projOver = false;
         }

         bool expOver = true;
         foreach (BExplosionEffect ex in explosions)
         {
            if (!ex.IsOver())
               expOver = false;
         }

         // Matka päättyi, losauta
         List<BProjectileAnimation> remoProj = new List<BProjectileAnimation>();
         foreach (BProjectileAnimation proj in projectiles)
         {
            if (proj.IsOver())
            {
               BExplosionEffect exp = new BExplosionEffect(Enemy.Position);
               explosions.Add(exp);
               Player.Effects.Add(exp);
               remoProj.Add(proj);
            }
         }

         foreach (BProjectileAnimation proj in remoProj)
            projectiles.Remove(proj);

         List<BExplosionEffect> remoExp = new List<BExplosionEffect>();
         foreach (BExplosionEffect exp in explosions)
         {
            if (exp.IsOver())
            {
               remoExp.Add(exp);
            }
         }

         foreach (BExplosionEffect exp in remoExp)
            explosions.Remove(exp);

         // Räjähdys päättyi, lopeta
         if (expOver && projOver)
         {
            over = true;
         }

         // Lopuksi otetaan damaget
         if (over && hit)
         {
            Enemy.TakeDamage(damageGiven);
         }
      }
   }
}
