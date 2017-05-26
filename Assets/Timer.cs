using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Assets
{
    public class Timer
    {
        private double since_started = 0;
        private double delay;

        private bool started = false;

        public Timer(double _delay)
        {
            delay = _delay;
        }

        public void Reset()
        {
            since_started = 0;
        }

        public void Start()
        {
            started = true;
        }

        public void Start(double _delay)
        {
            delay = _delay;
            started = true;
        }

        public void Stop()
        {
            started = false;
        }

        public void Update(GameTime gameTime)
        {
            if (started)
            {
                double time = gameTime.ElapsedGameTime.Milliseconds;
                since_started += time;
            }
        }

        public bool IsTimedOut()
        {
            return since_started > delay;
        }

        public bool IsStarted()
        {
            return started;
        }

        public double GetSinceStarted()
        {
            return since_started;
        }

        public double GetDelay()
        {
            return delay;
        }
    }
}
