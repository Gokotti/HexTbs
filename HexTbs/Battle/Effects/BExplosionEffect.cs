using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HexTbs.Battle.Map;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Assets;

namespace HexTbs.Battle.Effects
{
   public class BExplosionEffect : BVisualEffect
   {
      protected Texture2D texture;
      protected Timer timer;

      public Vector2 Position { get; set; }

      public BExplosionEffect(Vector2 position)
      {
         Position = position;
         timer = new Timer(350);
         timer.Start();
         texture = Statics.Textures["Effects/explosion"];
      }

      public override Texture2D GetTexture()
      {
         return texture;
      }

      public override Vector2 GetTextureOrigin()
      {
         Texture2D txt = texture;
         return new Vector2(txt.Width / 2, txt.Height / 2);
      }

      public override bool IsOver()
      {
         return timer.IsTimedOut();
      }

      public override void Draw(SpriteBatch sp, BCamera cam)
      {
         sp.Draw(GetTexture(), cam.GetVector(Position), null, Color.White, angle, GetTextureOrigin(), size, SpriteEffects.None, 0);
      }

      private float size = 0.75f;
      private float angle = 0;
      public override void Update(GameTime gt)
      {
         timer.Update(gt);
         float sizeSpeed = 0.32f / gt.ElapsedGameTime.Milliseconds;
         angle += (float)Math.PI * 1 / 180;
         size += sizeSpeed;
      }
   }
}
