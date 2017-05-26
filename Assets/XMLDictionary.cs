using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;

namespace Assets
{
    public class XMLDictionary : Dictionary<string, List<string>>
    {
        ContentManager Content;

        public XMLDictionary(ContentManager iContent)
        {
            Content = iContent;
        }

        public new List<string> this[string index]
        {
            get
            {
                if (base.ContainsKey(index)) return base[index];

                List<string> tmp;
                try
                {
                    tmp = Content.Load<List<string>>(index);
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
