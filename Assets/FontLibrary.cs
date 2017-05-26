using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Assets
{
    public class FontDictionary : Dictionary<string,SpriteFont>
    {
        String path;
        ContentManager Content;

        public FontDictionary(string _path,ContentManager cManager)
        {
            path = _path;
            Content = cManager;
        }

        public new SpriteFont this[string index]
        {
            get
            {
                if (base.ContainsKey(index)) return base[index];

                // If not found, then !
                SpriteFont tmp;
                try
                {
                    tmp = Content.Load<SpriteFont>(path + "\\" + index);
                }
                catch (ContentLoadException)
                {
                    tmp = null;
                }
                base.Add(index, tmp);
                return tmp;
            }
        }
    }
}
