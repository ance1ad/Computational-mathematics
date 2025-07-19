using System;

namespace EilerAndRunge
{
    internal class Program
    {
        static double F(double x, double y)
        {
            return -y;
        }

        static double EulerMethod(double a, double b, double h, double y)
        {
            // Инициализируем начальные условия
           
            double x = a;
            int iter = 0;
            double func;
            h = 0.5;
            // Используем цикл для интегрирования на каждом шаге
            Console.WriteLine("i\tx\t\ty\t\t  F(x,y)");
            Console.WriteLine("-----------------------------------------------------");
            while (x < b)
            {
                func = F(x, y);
                Console.WriteLine($"{iter}{Math.Round(x, 4),9}{Math.Round(y, 6),16}{Math.Round(func,8),20}");
                y = y + (h * func);
                x += h; // Переходим к следующему шагу
                iter++;
            }

            return y; // Возвращаем приближенное значение y на конечном шаге
        }


        static double RK4Method(double a, double b, double h, double y)
        {
            double k1, k2, k3, k4;
            double x = a, func;
            int iter = 0;
            h = 0.5;
            Console.WriteLine("i\tx\t\t  y\t\tF(x,y)");
            Console.WriteLine("----------------------------------------------------");
            while (x < b)
            {
                k1 = h * F(x, y);
                k2 = h * F(x + h / 2, y + k1 / 2);
                k3 = h * F(x + h / 2, y + k2 / 2);
                k4 = h * F(x + h, y + k3);

                func = F(x, y);
                y = y + (k1 + 2 * k2 + 2 * k3 + k4) / 6;
                
                Console.WriteLine($"{iter}{Math.Round(x, 4),9}{Math.Round(y, 6),16}{Math.Round(func, 8),20}");
                //Console.WriteLine($"k1: {Math.Round(k1, 4)}\t\t k2: {Math.Round(k2, 6),10}\nk3: {Math.Round(k3, 8)}\t\tk4: {Math.Round(k4, 8),10}");
                Console.WriteLine();

                x += h;
                iter++;
            }
            return y;
        }

        static void Main(string[] args)
        {
            double a = 0; // Левый конец интервала
            double b = 1; // Правый конец интервала
            double h = 0.05; // Шаг
            Console.WriteLine("\t\tДифференицальное уравнение: y' + y = 0");
            Console.Write("Введите начально значение для функции: ");
            string input = Console.ReadLine();
            double y = 0;
            bool inputIsNotValid = true;
            while(inputIsNotValid)
            {
                if (double.TryParse(input, out y))
                {
                    Console.WriteLine($"Начальное условие: y(0) = {y}");
                    inputIsNotValid = false;
                }
                else
                {
                    Console.Write("Введено некрректное значение, введите число: ");
                    input = Console.ReadLine();
                }
            }
            


            // Вызываем метод Эйлера для решения уравнения y' + y = 0 с начальным условием y(0) = 0
            Console.WriteLine("\t\tМетод Эйлера");
            EulerMethod(a, b, h, y);
            Console.WriteLine("\n\n\tМетод Рунге-Кутта 4го порядка");
            RK4Method(a, b, h, y);
        }
    }
}
