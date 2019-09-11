using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLongConsole
{
    public class LaggedFibRandom
    {
        private const int k = 10;
        private const int j = 7;
        private const int m = 2147483647;
        private const int u = 100000000;

        private const int a = 48271;
        private const int q = 44488;
        private const int r = 3399;

        private List<int> vals = null;
        private int curr;

        public LaggedFibRandom(int seed)
        {
            vals = new List<int>();
            if (seed == 0) seed = 1;
            int lCurr = seed;

            //Алгоритм Лемера 
            for (int i = 0; i < k + 1; ++i)
            {
                int hi = lCurr / q;
                int lo = lCurr % q;
                int t = (a * lo) - (r * hi);

                if (t > 0)
                    lCurr = t;
                else
                    lCurr = t + m;

                vals.Add(lCurr);
            }


            for (int i = 0; i < 1000; ++i)
            {
                NextDouble();
            }
        }

        public double NextDouble()
        {
            int left = vals[0] % m;
            int right = vals[k - j] % m;
            long sum = (long)left + (long)right;

            curr = (int)(sum % m);
            vals.Insert(k + 1, curr);
            vals.RemoveAt(0);
            return (1.0 * curr) / m;
        }

        public int Next()
        {
            int curr = (int)(NextDouble() * u);
            return curr;
        }
    }
}
