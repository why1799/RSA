using MyLongConsole;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyLongConsole
{
class Program
    {

        /*public static  void fun(int count, long m)
        {
            long x_prev = 1;
            long x_cur = 0;
            long xx;

            for (int i = 0; i < count; i++)
            {
                xx = x_cur;
                x_cur = (x_cur + x_prev) % m;
                x_prev = xx;

                //cout << "число №" << i + 1 << " = " << x_cur << endl;
                Console.WriteLine($"число № {i + 1} = {x_cur}");
            }
        }*/

        [STAThreadAttribute]
        static void Main(string[] args)
        {
         Application.EnableVisualStyles();
         Application.SetCompatibleTextRenderingDefault(false);
         Application.Run(new RSA());


        /*LaggedFibRandom rnd = new LaggedFibRandom((int)(DateTime.UtcNow.ToBinary() % 1000000000));

        for(int i = 0; i < 50; i++)
        {
            Console.WriteLine(rnd.Next());
        }

            //fun(25, DateTime.UtcNow.ToBinary());

            Console.ReadKey();*/


            /*Console.WriteLine("{0:D4}", 5007);

            Console.ReadKey();*/


            /*for(int i =0; i <9999; i++)
            {
                Console.WriteLine((char)i);
            }

            Console.WriteLine((int)'а');

            Console.ReadKey();*/

            /*LongArithmetic k = LongArithmetic.Parse("34569076543797567");
            LongArithmetic e = LongArithmetic.Parse("18345704726945544737836055281283624175");
            LongArithmetic fn = LongArithmetic.Parse("117983778885371576894077368024102615736");

            Console.WriteLine(k * e % fn);
            Console.ReadKey();


            LongArithmetic x = new LongArithmetic();
            LongArithmetic y = new LongArithmetic();

            LongArithmetic z = global::RSA.egcd(fn, k, ref x, ref y);*/

            //LongArithmetic a = new LongArithmetic((long)99999999);
            //LongArithmetic b = new LongArithmetic(99999999);
            //int b;


            //List<int> aaa = new List<int>(100);

            //Console.WriteLine(RSA.mypows(175, 235, 257));
            //Console.ReadKey();


            //Console.WriteLine(5 / 10000);

            /*while(true)
            {
                Console.Clear();
                Console.Write("Введите а: ");
                a = LongArithmetic.Parse(Console.ReadLine());
                Console.Write("Введите b: ");
                b = LongArithmetic.Parse(Console.ReadLine());
                //b = int.Parse(Console.ReadLine());

                Console.WriteLine("a > b = {0}", a > b);
                Console.WriteLine("a < b = {0}", a < b);
                Console.WriteLine("a == b = {0}", a == b);
                Console.WriteLine("a != b = {0}", a != b);
                Console.WriteLine("a >= b = {0}", a >= b);
                Console.WriteLine("a <= b = {0}", a <= b);
                Console.WriteLine("a + b = {0}", a + b);
                Console.WriteLine("a - b = {0}", a - b);
                Console.WriteLine("a * b = {0}", a * b);
                Console.WriteLine("a / b = {0}", a / b);
                Console.WriteLine("a % b = {0}", a % b);
                Console.ReadKey();
            }*/

            /*int d = 100000;
            for(int i = 1; i < d; i++)
            {
                for(int j = 1; j < i + 1; j++)
                {
                    Console.WriteLine("j = {0}", j);
                    int t = i / j;
                    a = i;
                    b = j;
                    LongArithmetic c = a / b;
                    if(c != t)
                    {
                        Console.WriteLine("dc` yt nfr");
                    }
                }
            }*/

            /*for(int i = 2; i < 200000; i++)
            {
                Console.WriteLine("{0} {1} простое число", i, RSA.IsPrime(i) ? "" : "не");
            }*/

            /* bool exit = false;
             LongArithmetic j;
             LongArithmetic pr = new LongArithmetic() ;
             for (int i = 0; !exit; i++)
             {
                 j = RSA.Random();

                  if (j == pr)
                      continue;

                 exit = RSA.ferma(j);
                 //exit = MillerRabinTest(BigInteger.Parse(j.ToString()), 10);
                 //pr = j;

                 for (int i = 0; j != 0; i++)
                 {
                     j /= 2;
                     Console.WriteLine(i);
                 }

                 Console.WriteLine("{0}{1}простое число", j, exit ? " " : " не ");
             }*/

            //Console.Write(a + b);

            //Console.ReadKey();
        }

        // производится k раундов проверки числа n на простоту
        public static bool MillerRabinTest(BigInteger n, int k)
        {
            // если n == 2 или n == 3 - эти числа простые, возвращаем true
            if (n == 2 || n == 3)
                return true;

            // если n < 2 или n четное - возвращаем false
            if (n < 2 || n % 2 == 0)
                return false;

            // представим n − 1 в виде (2^s)·t, где t нечётно, это можно сделать последовательным делением n - 1 на 2
            BigInteger t = n - 1;

            int s = 0;

            while (t % 2 == 0)
            {
                t /= 2;
                s += 1;
            }

            // повторить k раз
            for (int i = 0; i < k; i++)
            {
                // выберем случайное целое число a в отрезке [2, n − 2]
                RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

                byte[] _a = new byte[n.ToByteArray().LongLength];

                BigInteger a;

                do
                {
                    rng.GetBytes(_a);
                    a = new BigInteger(_a);
                }
                while (a < 2 || a >= n - 2);

                // x ← a^t mod n, вычислим с помощью возведения в степень по модулю
                BigInteger x = BigInteger.ModPow(a, t, n);

                // если x == 1 или x == n − 1, то перейти на следующую итерацию цикла
                if (x == 1 || x == n - 1)
                    continue;

                // повторить s − 1 раз
                for (int r = 1; r < s; r++)
                {
                    // x ← x^2 mod n
                    x = BigInteger.ModPow(x, 2, n);

                    // если x == 1, то вернуть "составное"
                    if (x == 1)
                        return false;

                    // если x == n − 1, то перейти на следующую итерацию внешнего цикла
                    if (x == n - 1)
                        break;
                }

                if (x != n - 1)
                    return false;
            }

            // вернуть "вероятно простое"
            return true;
        }
    }
}
