using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace Assets
{
    /// <summary>
    /// Dictionary wrapper which loads needed data from XNA's content pipeline
    /// </summary>
    public class SongDictionary : Dictionary<String, Song>
    {
        String path;
        ContentManager Content;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iPath">Content path</param>
        public SongDictionary(String iPath, ContentManager iContent)
            : base()
        {
            path = iPath;
            Content = iContent;
        }


        public new Song this[string index]
        {
            get
            {
                if (base.ContainsKey(index)) return base[index];

                // If not found, then !

                Song tmp;
                try
                {
                    tmp = Content.Load<Song>(path + "\\" + index);
                    //tmp = Routines.loadTexture(path + index);
                }
                catch (ContentLoadException)
                {

                    tmp = null;
                }
                base.Add(index, tmp);

                /*
                if (tmp != null)
                    tmp. = index;*/

                return tmp;


            }
        }

    }
}
