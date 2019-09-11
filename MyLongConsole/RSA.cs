using MyLongConsole;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class RSA
{
    public static int Times = 20;

    private static LongArithmetic Pow(LongArithmetic n, LongArithmetic power)
    {
        LongArithmetic res = 1;
        for(int i = 0; i < power; i++)
        {
            res *= n;
        }
        return res;
    }

    private static LongArithmetic binpow(LongArithmetic a, LongArithmetic n)
    {
        if (n == 0)
            return 1;
        if (n % 2 == 1)
            return binpow(a, n - 1) * a;
        else
        {
            LongArithmetic b = binpow(a, n / 2);
            return b * b;
        }
    }

    public static LongArithmetic Random()
    {
        //Random rnd = new Random();
        LaggedFibRandom rnd = new LaggedFibRandom((int)(DateTime.UtcNow.ToBinary() % 1000000000));

        StringBuilder sb = new StringBuilder();

        for(int i = 0; i < Times; i++)
        {
            sb.Append(rnd.Next() % 10000);
        }

        LongArithmetic res = LongArithmetic.Parse(sb.ToString());

        return res;
    }



    public static bool ferma(LongArithmetic x)
    {
        //Random rnd = new Random();
        LaggedFibRandom rnd = new LaggedFibRandom((int)(DateTime.UtcNow.ToBinary() % 1000000000));

        for (int j = 2; j < 100; j++)
        {
            if (x % j == 0 && x != j)
            {
                return false;
            }
        }

        for (int i = 0; i < 20; i++)
        {
            LongArithmetic a = (rnd.Next() % (x - 2)) + 2;

            if (gcd(a, x) != 1)
                return false;

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Проверка {0} на простоту. Тест {1}:", x.ToString(), i + 1);
            Console.ForegroundColor = ConsoleColor.White;

            if (mypows(a, x - 1, x) != 1)
                return false;
        }
        return true;
    }

    public static LongArithmetic gcd(LongArithmetic a, LongArithmetic b)
    {
        if (b == 0)
            return a;
        return gcd(b, a % b);
    }

    public static LongArithmetic egcd(LongArithmetic a, LongArithmetic b, ref LongArithmetic x, ref LongArithmetic y)
    {
        if (a == 0)
        {
            x = 0; y = 1;
            return b;
        }
        LongArithmetic x1 = new LongArithmetic(), y1 = new LongArithmetic();
        LongArithmetic d = egcd(b % a, a, ref x1, ref y1);
        x = y1 - (b / a) * x1;
        y = x1;
        return d;
    }

    public static LongArithmetic mypows(LongArithmetic a, LongArithmetic b, LongArithmetic m)
    {
        /*List<bool> bc = new List<bool>();

        LongArithmetic test = b;

        while (test != 0)
        {
            bc.Add(test % 2 == 1);
            test /= 2;
        }*/



        LongArithmetic tb = b;
        LongArithmetic t = a;
        LongArithmetic d = 1;

        for (int i = 0; !tb.IsZero(); i++)
        {
            //Console.WriteLine("i'm on {0}", i);
            if (LongArithmetic.ModS(tb, 2) == 1)
            {
                d = (d * t) % m;
            }
            tb = LongArithmetic.DivS(tb, 2);
            t *= t;
            t %= m;
        }


        return d;
    }
}
