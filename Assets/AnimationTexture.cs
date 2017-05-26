using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Assets
{
    public class AnimationTexture
    {
        public Texture2D texture;
        public Vector2 hotSpot;

        public AnimationTexture(Texture2D _texture, Vector2 _hotSpot)
        {
            texture = _texture;
            hotSpot = _hotSpot;
        }
    }
}
