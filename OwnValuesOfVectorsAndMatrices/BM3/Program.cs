using System.Numerics;

namespace BM3
{
    internal class Program
    {
        static void InitMatrix(double[,] arr, int n)
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    Console.Write($"строка {i + 1} столбец {j + 1}: ");
                    arr[i, j] = double.Parse(Console.ReadLine());
                }
            }
        }


        static void PrintMatrix(double[,] arr, int n)
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    Console.Write(arr[i,j] + "\t");
                }
                Console.WriteLine();
            }
        }


        static void ReUpdate(double[] yOld, double[] yNew, int n)
        {
            for (int i = 0; i < n; i++)
            {
                yOld[i] = yNew[i];
            }
        }


        static bool CheckCondition(double dOld, double dNew, double e)
        {
            var a = Math.Abs(dNew - dOld);
            Console.Write(", Разница: "+ a + "\n");
            if (a < e) 
                return true; 
            return false;
        }


        static void InitYNew(double[,] A, double[] yOld, double[] yNew, int n)
        {

            // yk+1 = A * yk - где yk - нормированный вектор
            for (int i = 0; i < n; i++) // сбиваем в 0, чтобы переинициализировать
            {
                yNew[i] = 0;
            }
            for (int str = 0; str < n; str++)
            {
                for (int run = 0; run < n; run++)
                {
                    yNew[str] += A[str,run]* yOld[run];
                }
            }

        }


        static void VectorNormalization(double[] vec, int n)
        {
            var len = CalculateLength(vec, n);
            for (int i = 0; i < n; i++)
            {
                vec[i] = vec[i] / len;
            }
        }


        static double CalculateLength(double[] vec, int n)
        {
            int len = 0; // длина векторюги
            double sum = 0;
            for (int i = 0; i < n; i++)
            {
                sum += Math.Pow(vec[i], 2);
            }
            return (Math.Pow(sum,0.5));
        }


        static void Main(string[] args)
        {
            double e = 0.001;
            int n = 0;
            while(n<=1)
            {
                Console.Write("Введите размерность матрицы (больше 1): ");
                n = int.Parse(Console.ReadLine());
            }    

            var A = new double[n,n];
            InitMatrix(A, n);
            Console.WriteLine("Ваша матрица: ");
            PrintMatrix(A, n);

            double[] yOld = new double[n];
            for (int i = 0; i < n; i++)
            {
                yOld[i] = 1;
            }
            double[] yNew = new double[n];
            double dOld = 1;
            double dNew = 1;
            bool flag = false;
            int counter = 0;
            while (flag == false)
            {
                InitYNew(A,yOld,yNew,n);
                dNew = CalculateLength(yNew, n);
                if (dNew != 0 && dNew != 1)
                    VectorNormalization(yNew, n);
                Console.WriteLine("Итерация: " + counter);
                Console.Write($"Xk: {Math.Round(dOld, 6)}, Xk+1: {Math.Round(dNew,6)}");
                flag = CheckCondition(dOld,dNew,e);
                if (flag == false)
                {
                    counter++;
                    dOld = dNew;
                    ReUpdate(yOld, yNew, n);
                }    
            }
            Console.WriteLine("Кол-во итераций: " + counter + "\n");

            Console.WriteLine("Собственный вектор:");
            for (int i = 0; i < n; i++)
            {
                Console.Write(Math.Round(yOld[i],5) + " ");
            }
            Console.WriteLine("\n" + "Собственное значение матрицы: " + Math.Round(dOld,6));
        }
    }
}