using System;

namespace Common
{
    public static class Randomizer
    {
        private static readonly Random getrandom = new Random();
        private static readonly object syncLock = new object();
        
        public static int GetRandomNumber(int min, int max)
        {
            lock(syncLock)
            {
                return getrandom.Next(min, max);
            }
        }
    }
}