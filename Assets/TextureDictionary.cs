using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Assets
{
   /// <summary>
   /// Dictionary wrapper which loads needed data from XNA's content pipeline
   /// </summary>
   public class TextureDictionary : Dictionary<String, Texture2D>
   {
      String path;
      ContentManager Content;

      /// <summary>
      /// 
      /// </summary>
      /// <param name="iPath">Content path</param>
      public TextureDictionary(String iPath, ContentManager iContent)
          : base()
      {
         path = iPath;
         Content = iContent;
      }

      public new Texture2D this[string index] {
         get {
            if (base.ContainsKey(index)) return base[index];

            // If not found, then !
            Texture2D tmp;
            try
            {
               tmp = Content.Load<Texture2D>(path + "\\" + index);
               //tmp = Routines.loadTexture(path + index);
            }
            catch (ContentLoadException)
            {
               tmp = null;
            }
            base.Add(index, tmp);

            if (tmp != null)
               tmp.Name = index;

            return tmp;
         }
      }

   }
}
