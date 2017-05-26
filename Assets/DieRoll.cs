using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets
{
   public class Die
   {
      public int Amount { get; protected set; }
      public int Sides { get; protected set; }
      public int Add { get; protected set; }

      public Die(int amount, int sides, int add)
      {
         Amount = amount;
         Sides = sides;
         Add = add;
      }
   }

   public static class DieRoll
   {
      private static Random rnd;

      public static void Init()
      {
         rnd = new Random();
      }

      /// <summary>
      /// Vakio 1d6 noppa
      /// </summary>
      /// <returns></returns>
      public static int RollDice()
      {
         return RollDice(1, 6, 0);
      }

      public static int RollDice(Die die)
      {
         return RollDice(die.Amount, die.Sides, die.Add);
      }

      /// <summary>
      /// Heitä noppaa
      /// </summary>
      /// <param name="amount">Noppien lukumäärä</param>
      /// <param name="sides">Nopan sivujen määrä</param>
      /// <param name="add">Lisä tai vähennys lukumäärään</param>
      /// <returns></returns>
      public static int RollDice(int amount, int sides, int add)
      {
         int value = 0;
         for (int i = 0; i < amount; i++)
            value += rnd.Next(1, sides + 1);
         return value + add;
      }

      /// <summary>
      /// Heitä noppaa, mutta enempi keskiarvotuloksia
      /// </summary>
      /// <param name="multi">Monikertoiminen jakauma</param>
      /// <param name="amount">Noppien lukumäärä</param>
      /// <param name="sides">Nopan sivujen määrä</param>
      /// <param name="add">Lisä tai vähennys lukumäärään</param>
      /// <returns></returns>
      public static int RollNormalDice(int multi, int amount, int sides, int add)
      {
         int value = 0;
         for (int i = 0; i < amount; i++)
         {
            int multiVal = 0;
            for (int j = 0; j < multi; j++)
               multiVal += rnd.Next(1, sides + 1);
            value += multiVal / multi;
         }
         return value + add;
      }

      /// <summary>
      /// Heitä noppaa, mutta heitä huonoimmat uudestaan
      /// </summary>
      /// <param name="reroll">Montako noppaa uudelleenheitetään</param>
      /// <param name="amount">Noppien lukumäärä</param>
      /// <param name="sides">Nopan sivujen määrä</param>
      /// <param name="add">Lisä tai vähennys lukumäärään</param>
      /// <returns></returns>
      public static int RollBestDice(int reroll, int amount, int sides, int add)
      {
         List<int> values = new List<int>();
         for (int i = 0; i < amount; i++)
            values.Add(rnd.Next(1, sides + 1));
         values.Sort();

         for (int i = 0; i < reroll; i++)
            values[i] = rnd.Next(1, sides + 1);

         int value = 0;
         foreach (int i in values)
            value += i;
         return value + add;
      }

      /// <summary>
      /// Heitä noppaa, mutta heitä parhaimmat uudestaan
      /// </summary>
      /// <param name="reroll">Montako noppaa uudelleenheitetään</param>
      /// <param name="amount">Noppien lukumäärä</param>
      /// <param name="sides">Nopan sivujen määrä</param>
      /// <param name="add">Lisä tai vähennys lukumäärään</param>
      /// <returns></returns>
      public static int RollWorstDice(int reroll, int amount, int sides, int add)
      {
         List<int> values = new List<int>();
         for (int i = 0; i < amount; i++)
            values.Add(rnd.Next(1, sides + 1));
         values.Sort();
         values.Reverse();

         for (int i = 0; i < reroll; i++)
            values[i] = rnd.Next(1, sides + 1);

         int value = 0;
         foreach (int i in values)
            value += i;
         return value + add;
      }
   }
}
