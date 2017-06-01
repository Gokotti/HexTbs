using HexTbs.Battle.Map;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HexTbs.Battle.Unit.SquadModels
{
   [Serializable]
   public class SquadModel
   {
      protected Texture2D texture;

      public int Actions { get; set; }
      public int MoveRange { get; set; }
      public HexMove MoveType { get; set; }
      public int SightRange { get; set; }
      public int Toughness { get; set; }
      public int Defence { get; set; }
   }
}
