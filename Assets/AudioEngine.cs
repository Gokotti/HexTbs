using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;

namespace Assets
{
    public class AudioEngine
    {
        public Tuple<sbyte,sbyte> getStereoShit(Vector2 listener, float listenerFace, Vector2 emitter)
        {
            float maxHearDist = 800;

            double dist = MathRoutines.getDistance(listener, emitter);
            float vol = (maxHearDist - (float)dist) / maxHearDist;

            float pan;
            if (dist < 16) 
                pan = 0f;
            else
            {
                pan = MathRoutines.getRotationByComponents(emitter - listener);
                pan -= listenerFace;
                pan = MathRoutines.getComponentsByRotation(pan).X;
            }

            return new Tuple<sbyte, sbyte>((sbyte)(pan * 127), (sbyte)(vol * 127));
        }

        public void PlayEffect(SoundEffect effect, sbyte panb, sbyte volb)
        {
            float pan = (float)panb / 128f;
            float vol = (float)volb / 128f;
            PlayEffect(effect, vol, pan);
           
        }

        public void PlayEffect(SoundEffect effect, float vol, float pan)
        {
            if (vol < 0.001) return;
            
            effect.Play(vol /** Statics.config.FXvol*/, 0f, pan);
        }

        public void PlayMusic(Song song)
        {
            StopMusic();
            MediaPlayer.Play(song);
        }

        public void StopMusic()
        {
            MediaPlayer.Stop();
        }
    }
}
