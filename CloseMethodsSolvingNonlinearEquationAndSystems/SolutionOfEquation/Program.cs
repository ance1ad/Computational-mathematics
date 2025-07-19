using System.Security.Cryptography.X509Certificates;

namespace SolutionOfEquation
{
    internal class Program
    {

        static double f1(double x) => (Math.Sin(x) - (1 / (x + 3)));
        static void HalfDivision(double e) // метод половинных делений
        {
            double a = -1;
            double b = 1;
            Console.Write("Хотите задать свой интервал - введите 1\nИспользовать готовый - другой ввод: ");
            string choose = Console.ReadLine();
            if(choose == "1")
            {
                Console.WriteLine("Введите левый край интервала :");
                a = double.Parse(Console.ReadLine());
                Console.WriteLine("Введите правый край интервала :");
                b = double.Parse(Console.ReadLine());
                if(a>b) 
                {
                    double temp = a;
                    a = b;
                    b = temp;
                }
                    
            }

            if (f1(a) * f1(b) > 0)
            {
                bool notFind = true;
                while (notFind) // f(a)*f(b) < 0 ищем интервалы
                {
                    if (a != -3 && b != -3)
                    {
                        if (f1(a) * f1(b) < 0)
                            notFind = false;
                    }
                    a-=0.1; b+=0.1;
                }
            }
            Console.WriteLine($"Подходящий интервал для решения: [{Math.Round(a,4)};{Math.Round(b,4)}]");

            double x = (a + b) / 2;
            int iter = 1;
            while ((Math.Abs(b - a)) > e)
            {
                if ((f1(x) * f1(b)) < 0)
                {
                    a = x;
                }
                else
                {
                    b = x;
                }
                Console.WriteLine($"Итерация {iter}, x = {x}");
                x = (a + b) / 2;
                iter++;
            }
            Console.WriteLine("Ответ найден методом половинного деления, x = " + ((a + b) / 2));
            Console.WriteLine("Кол-во итераций: " + iter+"\n\n\n");
        }

        static double Newt1Equation(double x, double y) => (2*y + x - 7);

        static double Newt2Equation(double x, double y) => (x * x + 4 * y * y - 25);
        // найдем производные
        static double Derivate1x(double x, double y) => (1);
        static double Derivate1y(double x, double y) => (2);
        
        static double Derivate2x(double x, double y) => (2*x);
        static double Derivate2y(double x, double y) => (8*y);

        static void InverseMatrix(double[,] arr)
        {
            double deter = arr[0, 0] * arr[1, 1] - arr[0, 1] * arr[1, 0];
            if (deter == 0)
            {
                Console.WriteLine("Матрица не имеет обратной !");
                return;
            }
            double temp = arr[0, 0];
            arr[0, 0] = arr[1, 1] / deter;
            arr[1, 1] = temp / deter;
            arr[0, 1] = -arr[0, 1] / deter;
            arr[1, 0] = -arr[1, 0] / deter;
        }


        static void NewtoneMethod(double x, double y, double e)
        {
            double[,] jacob = new double[2, 2];
            double dx, dy, norm;
            double[] vec = new double[2];

            int iter = 1;
            do
            {
                //якоби
                jacob[0, 0] = Derivate1x(x, y);
                jacob[0, 1] = Derivate1y(x, y);
                jacob[1, 0] = Derivate2x(x, y);
                jacob[1, 1] = Derivate2y(x, y);
                InverseMatrix(jacob);
                dx = -jacob[0, 0] * Newt1Equation(x, y) - jacob[0, 1] * Newt2Equation(x, y);
                dy = -jacob[1, 0] * Newt1Equation(x, y) - jacob[1, 1] * Newt2Equation(x, y);
                x += dx;
                y += dy;
                vec[0] = Newt1Equation(x, y);
                vec[1] = Newt2Equation(x, y);
                norm = Math.Sqrt(vec[0] * vec[0] + vec[1] * vec[1]);
                Console.WriteLine($"Итерация {iter}, x = {Math.Round(x,5)} | y = {Math.Round(y, 5)}");
                iter++;
            } while (norm >= e && iter < 500); // ограничение на количество итераций
            if (iter >= 500) Console.WriteLine("Превышено максимальное количество итераций, решений нет");
            else Console.WriteLine($"Результат вычисления методом Ньютона:\nx = {Math.Round(x,4)}\t|\ty = {Math.Round(y, 4)}\nкол-во итераций: {iter}");
        }


        static void Main(string[] args)
        {
            Console.WriteLine("Уравнение: sin(x) - (1/x+3)");
            double e = 0.001;
            HalfDivision(e);
            //---------------Ньютон----------------------------
            Console.WriteLine("Система уравнений : ");
            Console.WriteLine("2y + x - 7 = 0");
            Console.WriteLine("x^2 + 4y^2 - 25 = 0 ");
            Console.Write("Введите X для начального приближения: ");
            double x = double.Parse(Console.ReadLine());
            Console.Write("Введите Y для начального приближения: ");
            double y = double.Parse(Console.ReadLine());
            NewtoneMethod(x, y, e);
        }
    }
}