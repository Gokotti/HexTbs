using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Assets
{
   public static class Statics
   {
      public static TextureDictionary Textures { get; set; }
      public static Random Random { get; set; }
      public static AudioEngine Audio { get; set; }
      public static SoundDictionary Sounds { get; set; }
      public static XMLDictionary Xml { get; set; }
      public static FontDictionary Fonts { get; set; }

      public static Point ScreenSize { get; set; }

      public static void InitStatics(ContentManager cManager)
      {
         Random = new Random();
         DieRoll.Init();

         Textures = new TextureDictionary("Images", cManager);
         Sounds = new SoundDictionary("Sounds", cManager);
         Fonts = new FontDictionary("Fonts", cManager);

         Audio = new AudioEngine();
         Xml = new XMLDictionary(cManager);
      }

      // Shuffle a list
      public static void Shuffle<T>(this IList<T> list)
      {
         Random rng = new Random();
         int n = list.Count;
         while (n > 1)
         {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
         }
      }

      // Crop texture
      public static Texture2D Crop(Texture2D image, Rectangle source)
      {
         var graphics = image.GraphicsDevice;
         var ret = new RenderTarget2D(graphics, source.Width, source.Height);
         var sb = new SpriteBatch(graphics);

         graphics.SetRenderTarget(ret); // draw to image
         graphics.Clear(new Color(0, 0, 0, 0));

         sb.Begin();
         sb.Draw(image, Vector2.Zero, source, Color.White);
         sb.End();

         graphics.SetRenderTarget(null); // set back to main window

         return (Texture2D)ret;
      }
   }
}
