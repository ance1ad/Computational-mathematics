namespace IterMethod
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
                    if (j == n-1)
                        Console.WriteLine("| " + Math.Round(arr[i, j], 3));
                    else
                    Console.Write(Math.Round(arr[i,j],3) + "\t");
                }
            }
        }
        static void SwapString(double[,] arr, int m, int n, int start, int finish)
        {
            
            if (finish >= m)  
            {
                Console.WriteLine("Ошибка в методе перестановки строк");
                return;
            }
            double timeVar;
            for (int i = 0; i < n; i++)
            {
                timeVar = arr[start,i];
                arr[start, i] = arr[finish, i];
                arr[finish, i] = timeVar;
            }
           
        }



        static bool SortDominance(double[,] arr, int m, int n)
        {
            int iterations = m; // если матрица кривая то не будет выход за пределы для диагонали
            int poseDiag = 0;   // на каком элементе диагонали
            int strIter = 0;    // итератор для поиска строк
            bool findStr;       // нашли строку удовлетворяющую условию суммы
            bool notFind = true;
            double sum;
            if (n-1 < m)
                iterations = n-1;
            while (poseDiag!=iterations)
            {
                strIter = poseDiag;
                findStr = false;
                for (int i = strIter; i < iterations && findStr == false; i++)
                {
                    sum = 0;
                    for (int j = 0; j < n-1; j++)
                    {
                        sum += Math.Abs(arr[i, j]);
                    }
                    sum -= Math.Abs(arr[i, poseDiag]);
                    if (Math.Abs(arr[i, poseDiag]) > sum) // нашли строку
                    {
                        Console.WriteLine($"Найдена строка для {poseDiag+1} с диаг. преобладанием, ее позиция {i+1}");
                        findStr = true;
                        SwapString(arr, m, n, poseDiag, i);
                        poseDiag++;
                    }
                    if (i == iterations-1 && findStr == false)
                    {
                        Console.WriteLine($"Для строки {poseDiag + 1} не найдена строка с диагональным преобладанием");
                        poseDiag++;
                        notFind = false;
                    }     
                }
            }
            return notFind;
        }
















        static bool CheckMatrixNorm(double[,] arr, int m, int n)
        {
            bool fl = true; // норма меньше 1
            double sum;
            double maxSumStr = 0;
            for (int i = 0; i < m; i++)
            {
                sum = 0;
                for (int j = 0; j < n; j++)
                {
                    sum += Math.Abs(arr[i, j]);
                }
                if (sum >= 1)
                {
                    Console.WriteLine("максимальным модуль в G больше 1, сходимость не по прогрессии, решение не одно");
                    return false;
                }
                if (sum > maxSumStr)
                    maxSumStr = sum;
            }
            Console.WriteLine("Метод простой итерации сходится для начального приближения Xk = g, " + "\n" + "максимальный модуль равен  " + maxSumStr + " решение единственное");
            return true;
        }

        static void MakeGMatrix(double[,] arr, int m, int n)
        {
            double timeDivider;
            int iter = n;
            if (n > m)
                iter = m;
            for (int i = 0; i < iter; i++)
            {
                timeDivider = arr[i, i];
                if (timeDivider != 0)
                {
                    for (int j = 0; j < n; j++)
                    {
                        arr[i, j] /= timeDivider;
                        if (j != n - 1)
                        {
                            arr[i, j] *= -1;
                        }
                    }
                    arr[i, i] = 0;
                }   
            }
        }




        static void XPrint(double[] xOld, double[] xNew,int n)
        {
            Console.WriteLine("Xk" + "\t" + "\t" + "\t" + "Xk+1");
            for (int i = 0; i < n; i++)
            {
                Console.WriteLine(Math.Round(xOld[i],3) + "\t" + "\t" + "\t" + Math.Round(xNew[i],3));
            }
        }

        static bool CompareX(double[] xOld, double[] xNew, int n,double e)
        {
            double sum = 0; ;
            for (int i = 0; i < n; i++)
            {
                sum += Math.Pow((xOld[i] - xNew[i]),2);
            }
            sum = Math.Sqrt(sum);
            Console.WriteLine("Разница = " + sum);
            if (sum<=e)
                return true;   
             return false;
        }




        static bool FindVariables(double[,] arr, int m, int n, double e, double[] xOld, double[] xNew)
        {
            for (int i = 0; i < n-1; i++)
            {
                xOld[i] = 0;
            }
            bool stopCondition = false;
            int count = 0;
            double sumInStr;
            // тут уже вычисляем Xk+1
            while(!stopCondition)
            {
                
                for (int str = 0; str < n-1; str++)
                {
                    sumInStr = 0;
                    for (int stolb = 0; stolb < n-1; stolb++)
                    {
                        sumInStr += (arr[str, stolb] * xOld[stolb]);
                    }
                    sumInStr += arr[str, n - 1];
                    xNew[str] = sumInStr;
                }
                Console.WriteLine();
                Console.WriteLine("Итерация " + count);
                XPrint(xOld, xNew, n - 1);

                if (count == 200)
                {
                    Console.WriteLine("Итераций слишком много, метод расходится");
                    return false;
                }
                stopCondition = CompareX(xOld, xNew, n - 1, e);
                count++;

                for (int l = 0; l < n-1; l++)
                {
                    xOld[l] = xNew[l];
                }
            }
            return true;
        }








        static void Main(string[] args)
        {
            int m = 0;
            int n = 0;
            double e = 0.001;
            while (m < 1 || n < 1)
            {
                Console.Write("Введите кол-во уравнений: ");
                m = int.Parse(Console.ReadLine());
                Console.Write("Введите кол-во переменных: ");
                n = int.Parse(Console.ReadLine());
                if (m < 1 || n < 1)
                    Console.WriteLine("Введено некорректное значение" + "\n");
            }
            n++;
            double[,] arr = new double[m, n];


            InputMatrix(arr, m, n);
            Console.WriteLine("Ваша матрица");
            PrintMatrix(arr, m,n);


            SortDominance(arr, m, n);
            bool diagDom = true; // SortDominance(arr, m, n);
            bool moreVar = (n-1>m);
            if (moreVar)
                Console.WriteLine("Переменных больше чем уравнений, метод не работает");

            if (diagDom && !moreVar)
            {
                Console.WriteLine("\n" + "Матрица расставленая в соответствии с диагональным преобладанием: ");
                PrintMatrix(arr, m, n);

                // далее выражаем переменные

                MakeGMatrix(arr, m, n);
                Console.WriteLine("\n" + "Матрица G и вектор g ");
                PrintMatrix(arr, m, n);
               
                CheckMatrixNorm(arr, m, n - 1);
                


                var xOld = new double[n - 1]; // Xk
                var xNew = new double[n - 1]; // Xk+1
                bool findVar = FindVariables(arr, m, n, e, xOld, xNew);
                if(findVar)
                {
                    
                    if (m > n - 1)
                        Console.WriteLine("\n" + $"Ответ найден, но для первых {n - 1} уравнений" +"\n" + "Точность равна: " + e);

                    if (m == n - 1)
                        Console.WriteLine("\n" + "Ответ найден !" + "\n" + "Точность равна: "+e);

                    for (int i = 0; i < n - 1; i++)
                    {
                        Console.WriteLine($"X{i + 1} = {xNew[i]}");
                    }
                }
                
            }
            if(!diagDom)
            {
                Console.WriteLine("\n" +"Диагональное преобладание имеют не все строки," + "\n" + "достаточное условие сходимости нарушено");
                PrintMatrix(arr, m, n);
            }

        }
    }
}