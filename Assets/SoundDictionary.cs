using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace Assets
{
    /// <summary>
    /// Dictionary wrapper which loads needed data from XNA's content pipeline
    /// </summary>
    public class SoundDictionary : Dictionary<String, SoundEffect>
    {
        String path;
        ContentManager Content;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iPath">Content path</param>
        public SoundDictionary(String iPath, ContentManager iContent)
            : base()
        {
            path = iPath;
            Content = iContent;
        }


        public new SoundEffect this[string index]
        {
            get
            {
                if (base.ContainsKey(index)) return base[index];

                // If not found, then !
                SoundEffect tmp;
                try
                {
                    tmp = Content.Load<SoundEffect>(path + "\\" + index);
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
