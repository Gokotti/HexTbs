using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Assets
{
    public class Animation
    {
        List<AnimationTexture> frames = new List<AnimationTexture>();
        double since_started = 0;
        double frametime;
        public bool loop = true;

        private bool stopped = false;

        public Animation(TextureDictionary texDict, string path, int framecount, double _frametime)
        {
            // load images
            for (int i = 0; i < framecount; i++)
                frames.Add(new AnimationTexture(texDict[path + "\\" + i.ToString()], Vector2.Zero));
            frametime = _frametime;
        }

        public Animation(List<AnimationTexture> frames, double _frametime)
        {
            // insert images manually
            this.frames = frames;
            frametime = _frametime;
        }

        public Animation Clone()
        {
            Animation a = new Animation(frames, frametime);
            a.loop = loop;
            return a;
        }

        public void Reset()
        {
            since_started = 0;
        }

        public void Update(double time)
        {
            if (!stopped)
                since_started += time;
        }

        public Texture2D GetFrame(int index)
        {
            return frames[index].texture;
        }

        public bool IsAnimationOver()
        {
            if (loop) return false;

            if (since_started / frametime > frames.Count)
            {
                return true;
            }

            return false;
        }

        public Texture2D GetFrame()
        {
            if (!loop)
            {
                int t = Math.Min((int)(since_started / frametime), frames.Count - 1);
                return frames[t].texture;
            }
            else 
            {
                return frames[(int)(since_started / frametime) % frames.Count].texture;
            }
        }

        public AnimationTexture GetAnimationFrame()
        {
            if (!loop)
            {
                int t = Math.Min((int)(since_started / frametime), frames.Count - 1);
                return frames[t];
            }
            else
            {
                return frames[(int)(since_started / frametime) % frames.Count];
            }
        }

        public Vector2 GetHotSpot(int index)
        {
            return frames[index].hotSpot;
        }

        public void SetHotSpot(int index, Vector2 _hs)
        {
            frames[index].hotSpot = _hs;
        }

        public void Start()
        {
            stopped = false;
        }

        public void Stop()
        {
            stopped = true;
        }
    }
}
