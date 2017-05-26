using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Assets;
using HexTbs.Battle.Map;

namespace HexTbs.Battle.Effects
{
   public class BProjectileAnimation : BVisualEffect
   {
      protected Texture2D texture;
      public Vector2 Position { get; protected set; }
      protected Vector2 Start;
      protected Vector2 End;
      public float Angle { get; protected set; }

      protected Timer DelayTimer;

      public BProjectileAnimation(Vector2 start, Vector2 end)
      {
         Position = start;
         Start = start;
         End = end;
         texture = Statics.Textures["Projectiles/cannonball"];

         DelayTimer = new Timer(0);

         CalculateMove();
      }

      public void SetDelay(float delay)
      {
         DelayTimer = new Timer(delay);
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
         return distanceTravelled >= travelLength;
      }

      public override void Draw(SpriteBatch sp, BCamera cam)
      {
         if (DelayTimer.IsTimedOut())
            sp.Draw(GetTexture(), cam.GetVector(Position), null, Color.White, Angle, GetTextureOrigin(), 1f, SpriteEffects.None, 0);
      }

      protected void CalculateMove()
      {
         step = End - Start;
         step.Normalize();
         travelLength = Vector2.Distance(Start, End);
         Angle = (float)Math.Atan2(End.Y - Start.Y, End.X - Start.X);
      }

      private Vector2 step;
      private float distanceTravelled = 0;
      private float travelLength;

      public override void Update(GameTime gt)
      {
         if (!DelayTimer.IsStarted())
            DelayTimer.Start();

         DelayTimer.Update(gt);

         if (DelayTimer.IsTimedOut())
         {
            float speed = 320f / (float)gt.ElapsedGameTime.Milliseconds;
            Vector2 realStep = step * speed;
            float stepLength = Vector2.Distance(Vector2.Zero, realStep);
            distanceTravelled += stepLength;
            Position += realStep;
         }
      }
   }
}
