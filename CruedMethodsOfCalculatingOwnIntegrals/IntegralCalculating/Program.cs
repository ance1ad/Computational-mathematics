namespace IntegralCalculating
{
    internal class Program
    {
        public delegate double Funct (double x);

        static void RedWrite(string text)
        {
            Console.ForegroundColor = ConsoleColor.Red; // устанавливаем цвет
            Console.WriteLine(text);
            Console.ResetColor(); // сбрасываем в стандартный
        }
        static void GreenWrite(string text)
        {
            Console.ForegroundColor = ConsoleColor.Green; // устанавливаем цвет
            Console.WriteLine(text);
            Console.ResetColor(); // сбрасываем в стандартный
        }


        public static double Funct1Value(double x)
        {
            return (3 * x*x+ 7*x - 10 + Math.Pow(2,x));
        }


        public static double Funct2Value(double x)
        {
            return Math.Sin(Math.Log(x))/x;
        }


        public static double TrapezoidalMethod(Funct f, double a, double b, int n)
        {
            double h = (b - a) / n; // шаг интегрирования
            double sum = (f(a) + f(b)) / 2,x; // первое слагаемое в формуле
            for (int i = 1; i < n; i++)
            {
                x = a + i * h;
                sum += f(x);
            }
            return sum * h;
        }


        public static double RectangleMethod(Funct f, double a, double b, int n)
        {
            //n - кол-во прямоугольников
            double h = (b - a) / n; // шаг интегрирования
            double sum = 0.0, xi;

            for (int i = 1; i < n; i++)
            {
                xi = a + h * i + h / 2; // середина i-го прямоугольника
                sum += f(xi);
            }
            return sum * h;
        }


        static double SearchAnsw(Funct f, double a, double b, double e, int num)
        {
            int h = 4, i = 0;
            double time = 10, neww = 0, diff = 2;
            while (diff >= e)
            {
                if(num == 1)
                    neww = TrapezoidalMethod(f, a, b, h);
                if(num==2)
                    neww = RectangleMethod(f, a, b, h);
                diff = Math.Abs(neww - time);
                if (i>0)
                {
                    Console.Write("Итерация " + (i) + ": " + Math.Round(neww, 4) + "\t" + "|");
                    Console.WriteLine("\tРазница равна " + Math.Round(diff, 4));
                }
                time = neww;
                h *= 2; i++;
            }
            Console.WriteLine("Ответ найден, кол-во итераций: " + (i-1) + " конечный шаг: " + h);
            return neww;
        }

        static void Main(string[] args)
        {
            Funct f1 = Funct1Value;
            Funct f2 = Funct2Value;
            double[] diff = new double[4];
            double e = 0.001;

            // прямоугольники
            
            RedWrite("\tМетод прямоугольников, подбор для интеграла 1");
            diff[0] = SearchAnsw(f1, 2, 6,e,1);
            GreenWrite("Для интеграла 1: " + diff[0]);
            Console.WriteLine("______________________________________________________");

            RedWrite("\tМетод прямоугольников, подбор для интеграла 2");
            diff[2] = SearchAnsw(f2, 1, 3,e,1);
            GreenWrite("Для интеграла 2: " + diff[2]);
            Console.WriteLine("______________________________________________________");


            // трапеции

            RedWrite("\tМетод трапеций, подбор для интеграла 1");
            diff[1] = SearchAnsw(f1, 2, 6,e,2);
            GreenWrite("Для интеграла 1: " + diff[1]);
            Console.WriteLine("______________________________________________________");

            RedWrite("\tМетод трапеций, подбор для интеграла 2");
            diff[3] = SearchAnsw(f2, 1, 3, e,2);
            GreenWrite("Для интеграла 2: " + diff[3]);
            Console.WriteLine("______________________________________________________");

            // разница
            RedWrite("Разница между ответами для метода прямоугольников и трапеций составляет:");
            Console.WriteLine("Для первого интеграла: " + Math.Abs(diff[0] - diff[1]));
            Console.WriteLine("Для второго интеграла: " + Math.Abs(diff[3] - diff[2]));

        }
    }
}