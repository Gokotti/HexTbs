using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HexTbs.Battle.Map
{
   [Serializable]
   public class BMapModel
   {
      public string Name { get; set; }
      public int Width { get; set; }
      public int Height { get; set; }
      public List<string> Hexes { get; set; }
      public List<BMapModelCoordinate> P1Starts { get; set; }
      public List<BMapModelCoordinate> P2Starts { get; set; }

      public BMapModel()
      {
      }
   }

   public class BMapModelCoordinate
   {
      public int X { get; set; }
      public int Y { get; set; }
   }
}

