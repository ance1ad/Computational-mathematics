namespace MinQuadro
{
    internal class Program
    {

        static void MakeMatrixStairs(double[,] arr, int m, int n)
        {
            int iter = m; // если не квадратная матрица
            if (n < m)
                iter = n;
            for (int stolb = 0; stolb < iter - 1; stolb++) // бежим по диагонали
            {
                ZeroingUnder(arr, n, m, stolb, stolb + 1, stolb);
            }
        }


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
                }
            }
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


        static void PrintMass(double[,] mass,int m, int n)
        {
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    Console.Write(Math.Round(mass[i,j],4) + "\t");
                }
                Console.WriteLine();
            }
        }

        static double[,] GetTable(double[] x)
        {
            double[,] table = new double[5,3];
            for (int i = 0; i < 5; i++)
            {
                table[i, 0] = 1; 
                table[i, 1] = Funct1(x[i]); 
                table[i, 2] = Funct2(x[i]);
            }
            return table;
        }


        static double Funct1(double x)
        {
            return Math.Sqrt(x);
        }


        static double Funct2(double x)
        {
            return Math.Log(x);
        }


        static void Main(string[] args)
        {
            var x = new double[] { 1, 4, 9, 16, 25 };
            var y = new double[] { -1.1, 0.6, 3.1, 4.7, 6.2 };

            Console.WriteLine("Заданы аргументы и значения:\n");
            Console.WriteLine("\tx\ty");
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine("\t"+x[i] + "\t" + y[i]);
            }
            Console.WriteLine("Базисные функции: ");
            Console.WriteLine("F1(x) = √x");
            Console.WriteLine("F2(x) = ln(x)");
            Console.WriteLine("____________________________________________________________-");
            double[,] X = GetTable(x);

            Console.WriteLine("Матрица из 1, и значений функций");
            Console.WriteLine("\n\t\tf1\tf2");
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine("\t"+X[i, 0] + "\t" + X[i,1] + '\t'+ X[i, 2]);
            }
            Console.WriteLine("____________________________________________________________-");
            // далее формируем матрицу для ее обработки Гауссом

            double[,] Xt = new double[3, 5];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    Xt[i, j] = X[j, i];
                }
            }
            Console.WriteLine("Транспонированная");
            PrintMass(Xt,3,5);

            // перемножение матриц
            double[,] xTx = new double[3, 3];
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    for (int k = 0; k < 5; k++)
                    {
                        xTx[i, j] += Xt[i, k] * X[k, j];
                    }

            // Столбец свободных членов
            double[] b = new double[3] { 0, 0, 0 };
            // произведение матрицы на вектор
            double sum = 0;
            for (int i = 0; i < 3; i++)
            {
                sum = 0;
                for (int j = 0; j < 5; j++)
                {
                    sum += (Xt[i, j] * y[j]);
                }
                b[i] = sum;
            }
            Console.WriteLine("\nМатрица перемноженная: ");
            PrintMass(xTx,3,3);
            Console.Write("\nЕе вектор: ");
            foreach (int val in b)
            {
                Console.Write(val + " ");
            }
            Console.WriteLine();
            //объеденим матрицу и вектор
            double[,] gauss = new double[3, 4];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {gauss[i, j] = xTx[i, j];}
                gauss[i, 3] = b[i];
            }

            Console.WriteLine("\n\nМатрица для нахождения коэффициентов: ");
            PrintMass(gauss,3,4);

            // Гаусс
            MakeMatrixStairs(gauss, 3, 4);
            int m = 3, n = 4;
            var a = new double[n - 1];
            BackMove(gauss, 3, 4, a); 
            Console.WriteLine("____________________________________________________________-");
            Console.WriteLine("\n" + "Коэффициенты а: ");
            for (int i = 0; i < n - 1; i++)
            {
                Console.WriteLine($"a{i} = {a[i]}");
            }

            Console.WriteLine("\nПолученные значения У: ");
            Console.WriteLine("y = a0 + a1 * f1(x) + a2 * f2(x)");
            for (int i = 0; i < 5; i++)
            {
                y[i] = a[0] + (a[1] * Funct1(x[i])) + (a[2] * Funct2(x[i]));
                Console.WriteLine($"Y[{i + 1}] = {Math.Round(y[i],4)}");
            }
        }

    }
}