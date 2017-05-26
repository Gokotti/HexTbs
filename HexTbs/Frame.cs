using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace HexTbs
{
    public abstract class Frame
    {
        public abstract void Draw(SpriteBatch sp);
        public abstract void Update(GameTime gt);
    }
}
