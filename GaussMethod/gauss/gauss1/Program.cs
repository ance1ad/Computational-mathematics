namespace gauss1
{
    internal class Program
    {
        static void InputMatrix(double[,] arr, int m, int n)
        {
            int j, i;
            Console.WriteLine("Введите вашу матрицу");
            for (i = 0; i < m; i++)
            {
                for (j = 0; j < n - 1; j++)
                {
                    Console.Write($"строка {i + 1} столбец {j + 1}: ");
                    arr[i, j] = double.Parse(Console.ReadLine());
                }
                Console.Write($"Свободный член строки {i + 1}: ");
                arr[i, j] = double.Parse(Console.ReadLine());
            }
        }
        static void PrintMatrix(double[,] arr, int m, int n)
        {

            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    arr[i, j] = Math.Round(arr[i, j], 3);
                    Console.Write(arr[i, j] + "\t");
                }
                Console.WriteLine();
            }
        }

        static bool SwapString(double[,] arr, int n, int m, int el)
        {
            int run = el + 1;
            double timeVar;
            if (el >= (m - 1))
            {
                Console.WriteLine("Ошибка в вызове метода перестановки строки");
                return false;
            }

            while (run != m - 1 && arr[run, el] == 0)
            {
                run++;
            }
            if (run == m - 1)
            {
                Console.WriteLine("элемент диагонали " + el + " и ниже имеет нули, решенее не одно ");
                return false;
            }
            else
            {
                for (int i = 0; i < n; i++)
                {
                    timeVar = arr[el, i];
                    arr[el, i] = arr[run, i];
                    arr[run, i] = timeVar;
                }
                return true;
            }
        }  // ищем строку с ненулевым эл-ом по диагонали, если нет возвращает false


        static void ZeroingUnder(double[,] arr, int n, int m, int str1, int str2, int delPose)
        {
            double diff;
            if (arr[str1, delPose] != 0) // на всякий
            {

                for (int allStr = str2; allStr < m; allStr++) // бежим по всем строкам
                {
                    diff = arr[allStr, delPose] / arr[str1, delPose];
                    for (int column = 0; column < n; column++)
                    {
                        arr[allStr, column] -= diff * arr[str1, column];
                    }
                    Console.WriteLine("Зануление под элементом на диагонали " + str1);
                    PrintMatrix(arr, m, n);
                }
            }
        }


        static bool MakeMatrixStairs(double[,] arr, int m, int n)
        {
            

            bool fl = true;
            int iter = m; // если не квадратная матрица
            if (n < m)
                iter = n;
            for (int stolb = 0; stolb < iter-1; stolb++) // бежим по диагонали
            {
                // если на диагонале 0
                if (arr[stolb, stolb] == 0 && stolb != n - 1) //свопаем пока не найдем строку
                {
                    fl = SwapString(arr, n,m, stolb); // ищем ненулевой эл-т на диагонали
                    if (!fl && stolb == 0)
                    {
                        Console.WriteLine("Первый столбец нулевой, найти переменную невозможно");
                        return false;
                    }
                    if (!fl) // внизу нулевые элементы
                    {
                        Console.WriteLine("Решений бесконечно, смотри строку " + stolb);
                        return false;
                    }
                }
                ZeroingUnder(arr, n, m, stolb, stolb + 1, stolb);
            }
            if (n - 1 > m)
            {
                Console.WriteLine("Переменных слишком много, решение точно не одно");
                return false;
            }
            return true;
        }


        static bool SlayAnalys(double[,] arr, ref int m, int n)
        {
            // бежим по строке и считаем нулевые элементы
            // ежели их n-1 то нулевая строка
            // если n-2 то  решений нет, так как ранги не совпадают
            int zeroCount;
            for (int i = 0; i < m; i++)
            {
                zeroCount = 0;
                for (int j = 0; j < n; j++)
                {
                    if (arr[i, j] == 0)
                    {
                        zeroCount++;
                    }
                }
                if (zeroCount == n)
                {
                    Console.WriteLine($"Нулевая строка { i+1 }");
                    m--;
                    Console.WriteLine("Обновленная матрица без нулевой");
                    PrintMatrix(arr, m, n);
                }
                if (zeroCount == n - 1 && arr[i, n - 1] != 0) // рангу хана
                {
                    Console.WriteLine($"Ранги разные, решений нет, смотри строку  {i+1}");
                    return false;
                }
            }
            return true;
        }


        static void BackMove(double[,] arr, int m, int n, double[] xArr)
        {

            double acum;
            xArr[n - 2] = arr[m - 1, n - 1] / arr[m - 1, n - 2];

            for (int str = m - 2; str >= 0; str--)
            {
                acum = 0;
                for (int perem = n - 2; perem != str; perem--) // n-3 так как мы считаем пременные
                {
                    acum += xArr[perem] * arr[str, perem];
                }
                xArr[str] = (arr[str, n - 1] - acum) / arr[str, str];
            }
        }


        static void Main(string[] args)
        {
            int m = 0;
            int n = 0;
            while (m < 1 || n < 1)
            {
                Console.Write("Введите кол-во уравнений: ");
                m = int.Parse(Console.ReadLine());
                Console.Write("Введите кол-во переменных: ");
                n = int.Parse(Console.ReadLine());
                if (m < 1 || n < 1)
                    Console.WriteLine("Введено некорректное значение");
            }

            n++;

            double[,] arr = new double[m, n];

            InputMatrix(arr, m, n);

            Console.WriteLine("\n" + "Ваша расширенная матрица: ");
            PrintMatrix(arr, m, n);

            bool fl = MakeMatrixStairs(arr, m, n);
            //bool solveExist = SlayAnalys(arr, m, n);

            Console.WriteLine("\n" + "Лестничный вид");
            PrintMatrix(arr, m, n);

            var xArr = new double[n - 1];
            if (fl == true)
            {

                bool solveExist = SlayAnalys(arr, ref m, n);
                if (solveExist)
                {
                    BackMove(arr, m, n, xArr);
                    Console.WriteLine("\n" + "Ответ: ");
                    for (int i = 0; i < n - 1; i++)
                    {
                        Console.WriteLine($"X{i + 1} = {xArr[i]}");
                    }
                }
            }
        }

    }
}