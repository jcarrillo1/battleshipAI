using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BattleShip
{
    public static class StaticRandom
    {
        /**
         * This code was grabbed from stack overflow as a solution to 
         * a problem with random number generation in c#
         * http://stackoverflow.com/questions/767999/random-number-generator-only-generating-one-random-number
         */
        private static int seed;

        private static ThreadLocal<Random> threadLocal = new ThreadLocal<Random>
            (() => new Random(Interlocked.Increment(ref seed)));

        static StaticRandom()
        {
            seed = Environment.TickCount;
        }

        public static Random Instance { get { return threadLocal.Value; } }
    }
}
