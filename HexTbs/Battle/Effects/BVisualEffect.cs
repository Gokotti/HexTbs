using HexTbs.Battle.Map;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HexTbs.Battle.Effects
{
   public abstract class BVisualEffect
   {
      public abstract Texture2D GetTexture();
      public abstract Vector2 GetTextureOrigin();
      public abstract bool IsOver();
      public abstract void Draw(SpriteBatch sp, BCamera cam);
      public abstract void Update(GameTime gt);
   }
}
