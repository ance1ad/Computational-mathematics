using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace ConsoleApp1
{
    internal class Program
    {
        static int InputDimension()
        {
            int m = 0;
            while (m<3)
            {
                Console.WriteLine("Введите размерность таблицы > 2");
                m  = int.Parse(Console.ReadLine());
            }
            return m;
        }


        static void InputTable(int m, double[,] arr)
        {

            for (int i = 0; i < m; i++)
            {
                Console.Write($"Аргумент строки {i + 1}: ");
                arr[i, 0] = double.Parse(Console.ReadLine());
                Console.Write("Его значение: ");
                arr[i, 1] = double.Parse(Console.ReadLine());
            }
        }


        static void PrintTable(int m, double[,] arr)
        {
            Console.WriteLine(" x\t\t  y");
            for (int i = 0; i < m; i++)
            {
                Console.WriteLine("_________________________");
                Console.WriteLine(Math.Round(arr[i, 0], 3) + "\t" + "|" + "\t" + Math.Round(arr[i, 1], 3) + "\t|");
            }
            Console.WriteLine("_________________________");
        }


        static bool SortTable(int size, double[,] t)
        {
            double temp;
            bool flag = false;
            for (int i = 0; i < size; i++)
            {
                for (int j = i + 1; j < size; j++)
                {
                    if (t[j,0] < t[i,0])
                    {
                        flag = true; // что-то да отсортировалось
                        temp = t[i,0];
                        t[i,0] = t[j,0];
                        t[j,0] = temp;

                        temp = t[i,1];
                        t[i,1] = t[j,1];
                        t[j,1] = temp;

                    }
                }
            }
            return flag; // сортировка таблицы по возрастанию была/не была
        }


        static bool ArgumentStepAnalys(int m, double[,] table)
        {
            double h1 = table[1, 0] - table[0, 0];
            double h2 = h1;
            int i = 1;
            while(i!=m) // очень маленькая разница
            {
                h2 = table[i, 0] - table[i-1, 0];
                if (Math.Abs(h2 - h1) > 0.00001)
                    return false;
                i++;
            }
                return true;
        }


        static bool InputValidation(int size, double[,] t)
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = i + 1; j < size; j++)
                {
                    if (t[i,0] == t[j,0] && t[i,1] != t[j,1])
                    {
                        RedWrite($"Для одинаковых аргументов введены разные значения\n(см y = {t[i,1]} и {t[j, 1]} в таблице)");
                        return false;
                    }
                    if (t[i, 0] == t[j, 0])
                    {
                        RedWrite($"Заданы одинаковые аргументы,\nесть шанс деления на 0 (см X = {t[i, 0]})");
                        return false;
                    }
                }
            }
            return true;
        }


        static double Calculate2Derivative(int i, double[,] table)
        {
            double h1 = table[i, 0] - table[i - 1, 0];
            double h2 = table[i + 1, 0] - table[i, 0];
            double denominator = (h1 + h2)/2;
            double dif = ((table[i + 1, 1] - table[i, 1]) / h2 - (table[i, 1] - table[i - 1, 1]) / h1) / denominator;
            return dif;
        }


        static void CountDerivative(int m, double[,] table)
        {
            // первая точка
            double h1, h2;
            double dif;
            h1 = table[1, 0] - table[0, 0];
            dif = (table[1, 1] - table[0, 1]) / h1;
            RedWrite("Для точки: " + table[0, 0]);
            Console.WriteLine("Первая производная = " + Math.Round(dif, 3));
            dif = Calculate2Derivative(1, table);
            Console.WriteLine("Вторая производная = " + Math.Round(dif, 3));

            // центральные точки
            for (int i = 1; i < m - 1; i++)
            {
                h1 = table[i, 0] - table[i - 1, 0];
                h2 = table[i + 1, 0] - table[i, 0];
                dif = (table[i + 1, 1] - table[i - 1, 1]) / (h1 + h2);
                RedWrite("Для точки: " + table[i, 0]);
                Console.WriteLine("Первая производная = " + Math.Round(dif, 3));

                dif = Calculate2Derivative(i, table);
                Console.WriteLine("Вторая производная = " + Math.Round(dif, 3));
            }
            // последняя точка
            h2 = table[m - 1, 0] - table[m - 2, 0];
            dif = (table[m - 1, 1] - table[m - 2, 1]) / h2;
            RedWrite("Для точки: " + table[m - 1, 0]);
            Console.WriteLine("Первая производная = " + Math.Round(dif, 3));
            dif = Calculate2Derivative(m - 2, table);
            Console.WriteLine("Вторая производная = " + Math.Round(dif, 3));
        }


        static void RedWrite(string text)
        {
            Console.ForegroundColor = ConsoleColor.Red; // устанавливаем цвет
            Console.WriteLine(text);
            Console.ResetColor(); // сбрасываем в стандартный
        }

        static void Main(string[] args)
        {
            int m = InputDimension();
            double[,] table = new double[m, 2];// { { 2, 0.7581 }, { 3, 0.9541 }, {10,1.2537}, { 5, 1.3554 }, {6,1.6571 } };
            RedWrite("Введите вашу таблицу аргументов и значений");
            InputTable(m,table);
            Console.ForegroundColor = ConsoleColor.Red; // устанавливаем цвет
            RedWrite("     Ваша таблица");
            Console.ResetColor(); // сбрасываем в стандартный
            PrintTable(m, table);
            if (SortTable(m, table) == true)
            {
                RedWrite("\n Отсортированная таблица");
                PrintTable(m, table);
                Console.WriteLine("\n");
            }
            if(InputValidation(m, table) == false)
                return; // некорректный ввод

            //проверка равный ли шаг
            if (ArgumentStepAnalys(m, table) == false)
                Console.WriteLine("Шаг аргументов разный, сетка узлов задана неравномерно");
            else
                Console.WriteLine("Сетка узлов задана равномерно");
            CountDerivative(m, table);
        }
    }
}