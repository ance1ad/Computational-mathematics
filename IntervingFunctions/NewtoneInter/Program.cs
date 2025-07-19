namespace NewtoneInter
{
    internal class Program
    {
        static void InitTable(double[]x, double[]y, int size)
        {
            for (int i = 0; i < size; i++)
            {
                Console.Write($"X{i+1} = ");
                x[i] = double.Parse(Console.ReadLine());
                Console.Write($"Y{i+1} = ");
                y[i] = double.Parse(Console.ReadLine());
            }
        }


        static void PrintTable(double[] x, double[] y, int size)
        {
            Console.WriteLine("_________________________________________________\n");
            for (int i = 0; i < size; i++)
            {
                Console.WriteLine($"|\tX{i+1} = {Math.Round(x[i],4)}\t|\tY{i + 1} = {Math.Round(y[i], 4)} \t|");
            }
            Console.WriteLine("_________________________________________________\n");
        }


        static bool SortTable(double[] x, double[] y, int size)
        {
            double temp;
            bool flag = false;
            for (int i = 0; i < size; i++)
            {
                for (int j = i+1; j < size; j++)
                {
                    if (x[j] < x[i])
                    {
                        flag = true;
                        temp = x[i];
                        x[i] = x[j];
                        x[j] = temp;
                        temp = y[i];
                        y[i] = y[j];
                        y[j] = temp;
                    }
                }
            }
            return flag;//сортировка таблицы по возрастанию
        } 


        static bool InputValidation(double[] x, double[] y, int size)
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = i+1; j < size; j++)
                {
                    if (x[i] == x[j] && y[i] !=y[j])
                    {
                        Console.WriteLine($"Для одинаковых аргументов введены разные значения (см {i+1} и {j+1} в сортированной таблице)");
                        return false;
                    }    
                }
            }
            return true;
        } // проверка ввода таблицы


        static bool TableAnalys(double[] x, double[] y, int size, ref double h)
        {
            h = x[1] - x[0];
            int iter = 2;
            if (InputValidation(x,y,size)== false)
                return false;
                
            while (h == 0 && iter != size)
            {
                h = x[iter] - x[iter - 1];
                iter++;
            }
            if (h == 0)
            {
                Console.WriteLine("Введены одинаковые аргументы");
                return false;
            }

            for (int i = 2; i < size; i++)
            {
                if (Math.Abs(x[i] - x[i - 1]) != h)
                {
                    Console.WriteLine("Задана таблица с неравностоящими узлами");
                    return false;
                }
            }
            Console.WriteLine("Таблица задана верно, шаг равен "+ h);
            return true;
        } // анализирует таблицу


        static double DotAnalys(double xI, double[] x, int size, ref bool beyond, ref double h)
        {
            beyond = (xI > x[size - 1]) || (xI < x[0]); 
            double q;
            if (beyond) // если точка за границами
            {
                Console.WriteLine("Точка за границами, применяется метод конечной разности");
                q = (xI - x[size - 1]) / h;
            }
            else
            {
                Console.WriteLine("Точка внутри интервала узлов, применяется метод разделенной разности");
                q = (xI - x[0]) / h;
            }
            if (Math.Abs(q) >= 1)
                Console.WriteLine("Ответ может быть дан с погрешностью, |q| = " + Math.Abs(q));
            return q;
        } //анализирует аргумент


        static double NewtInterp(double[] x, double[] y, int size, double xInterp, ref double h)
        {
                
            var d = new double[size]; // массив дельт
            double p = 0,t=1,r=0; // ответ p, остальное вспомогательное
            bool beyond = false;
            double q = DotAnalys(xInterp,x,size, ref beyond, ref h);

            for (int i = 0; i < size; i++)
            {
                d[i] = y[i];
            }

            if (beyond)
                p = d[size-1];
            else
                p = d[0];

            int counter = 1; // просто для счета
            for (int k = 1; k < size; k++) 
            {
                for (int i = 0;i < size-k; i++)
                {
                    d[i] = d[i + 1] - d[i];
                }
                if (beyond)
                {
                    t = t * (q + k - 1) / k;
                    r = d[size - k] * t;
                }
                else
                {
                    t = t * (q - k + 1) / k;
                    r = d[0] * t;
                }
                p = p + r;
                Console.WriteLine("Итерация " + counter + " значение = " + p);
                counter++;
            }
            return p;
        } // выполняет основной алгоритм интерполяции


        static void Main(string[] args)
        {
            Console.WriteLine("Программа вычисляет значение функции по заданной таблице аргументов\nи значений, аргументы заданы на равностоящем друг от друга расстоянии");
            Console.WriteLine("Хотите задать свою таблицу или использовать готовую ?");
            Console.Write("(1) - ввести свою  (2) - использовать готовую: ");
            string choice = Console.ReadLine();
            double[] x, y;
            int size;
            double dot = 0;
            double h = 1;

            switch (choice)
            {
                case "1":
                    Console.Write("Введите кол-во строк: (>2) ");
                    size = int.Parse(Console.ReadLine());
                    if (size < 3)
                    {
                        Console.WriteLine("Введена маленькая размерность");
                        return;
                    }   
                    x = new double[size];
                    y = new double[size];
                    InitTable(x, y, size);
                    Console.WriteLine("\n\t\tВаша таблица значений");
                    PrintTable(x, y, size);
                    if(SortTable(x, y, size)==true)
                    {
                        Console.WriteLine("\n\tВаша осортированная таблица значений");
                        PrintTable(x, y, size);
                    }
                    if (TableAnalys(x,y, size, ref h) == false)
                        return;
                    break;

                case "2":
                    size = 4;
                    x = new double[] { 2.1, 2.6, 3.1, 3.6 };
                    y = new double[] { 0.7581, 0.9541, 1.2537, 1.3554 };
                    h = x[1] - x[0];
                    Console.WriteLine("\n\t\tВаша таблица значений");
                    PrintTable(x, y, size);
                    break;

                default:
                    Console.WriteLine("Неправильный выбор.");
                    return;
            }
            bool end = false;

            while (end == false)
            {
                Console.Write("Введите точку, которую хотите найти: ");
                dot = double.Parse(Console.ReadLine());
                double p = NewtInterp(x, y, size, dot, ref h);
                Console.WriteLine($"Функция для {dot} принимает значение: {p}");
                Console.WriteLine("_________________________________________________");
                Console.Write("Если хотите еще ввести значение введите 1\n(другой ввод - выход из программы): ");
                choice = Console.ReadLine();
                if (choice != "1")
                    end = true;
            }
            Console.WriteLine("Конец программы");
        }
    }
}