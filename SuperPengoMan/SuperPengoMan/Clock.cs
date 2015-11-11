using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperPengoMan
{
    class Clock
    {
        float timer;

        public Clock()
        {
            timer = 0;
        }

        public void AddTime(float time)
        {
            timer += time;
        }

        public float Timer()
        {
            return timer;
        }

        public void ResetTime()
        {
            timer = 0;
        }
    }
}
