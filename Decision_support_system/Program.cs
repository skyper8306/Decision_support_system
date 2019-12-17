using System;

namespace Decision_support_system
{
    class Program
    {
        static int num_k = 6;// Кол-во критериев (Б.О.)
        static int num_alt = 6;// Кол-во альтернатив
        static void Main(string[] args)
        {
            // Входные данные
            string[] crits = new string[6] { "Стоимость\t\t", "Величина автопарка\t", "Качество авто\t\t",
                "Удобство использования\t", "Страховые случаи\t", "Качество тех. поддержки" };
            double[] znach = { 0.35, 0.30, 0.15, 0.10, 0.05, 0.05 };
            int[,,] bo = {
                            {
                                { 1, 0, 0, 0, 1, 1},
                                { 1, 1, 1, 0, 1, 1},
                                { 1, 0, 1, 0, 1, 1},
                                { 1, 1, 1, 1, 1, 1},
                                { 0, 0, 0, 0, 1, 1},
                                { 0, 0, 0, 0, 0, 1}
                            },
                            {
                                { 1, 1, 1, 1, 0, 1},
                                { 0, 1, 1, 1, 0, 1},
                                { 0, 0, 1, 1, 0, 1},
                                { 0, 0, 0, 1, 0, 1},
                                { 1, 1, 1, 1, 1, 1},
                                { 0, 0, 0, 0, 0, 1}
                            },
                            {
                                { 1, 1, 1, 1, 1, 1},
                                { 0, 1, 1, 1, 0, 1},
                                { 0, 0, 1, 1, 0, 1},
                                { 0, 0, 0, 1, 0, 1},
                                { 0, 1, 1, 1, 1, 1},
                                { 0, 0, 0, 0, 0, 1}
                            },
                            {
                                { 1, 1, 1, 1, 1, 1},
                                { 0, 1, 1, 1, 0, 1},
                                { 0, 0, 1, 1, 0, 1},
                                { 0, 0, 0, 1, 0, 1},
                                { 0, 1, 1, 1, 1, 1},
                                { 0, 0, 0, 0, 0, 1}
                            },
                            {
                                { 1, 1, 1, 1, 0, 1},
                                { 0, 1, 1, 1, 0, 0},
                                { 0, 0, 1, 0, 0, 0},
                                { 0, 0, 1, 1, 0, 0},
                                { 1, 1, 1, 1, 1, 1},
                                { 0, 1, 1, 1, 0, 1}
                            },
                            {
                                { 1, 1, 1, 1, 1, 1},
                                { 0, 1, 1, 1, 0, 1},
                                { 0, 0, 1, 1, 0, 1},
                                { 0, 0, 0, 1, 0, 0},
                                { 0, 1, 1, 1, 1, 1},
                                { 0, 0, 0, 1, 0, 1}
                            }
                        };
            int[,] resDom = new int[num_k, num_alt];
            int[,] resBloc = new int[num_k, num_alt];
            double[,] resTour = new double[num_k, num_alt];
            double[] tempd = new double[num_alt];
            int[] tempi = new int[num_alt];

            //Блокировка
            Console.WriteLine("\tМЕХАНИЗМ БЛОКИРОВКИ:");
            Block(bo, resBloc);
            for (int i = 0; i < num_k; i++)
            {
                Console.Write("Критерий " + crits[i]);
                Console.Write("\tРешения: ");
                for (int j = 0; j < num_k; j++)
                {
                    if (resBloc[i, j] == 1)
                        Console.Write((j + 1) + "; ");
                }
                Console.WriteLine();
            }
            FunCh(resBloc);
            Console.Write("Итог: ");
            for (int i = 0; i < num_alt; i++)
                tempi[i] = resBloc[num_k - 1, i];
            PrintVector(tempi);


            //Доминирование
            Console.WriteLine("\n\tМЕХАНИЗМ ДОМИНИРОВАНИЯ:");
            Domin(bo, resDom);
            for (int i = 0; i < num_k; i++)
            {
                Console.Write("Критерий " + crits[i]);
                Console.Write("\tРешения: ");
                for (int j = 0; j < num_k; j++)
                {
                    if (resDom[i, j] == 1)
                        Console.Write((j + 1) + "; ");
                }
                Console.WriteLine();
            }
            FunCh(resDom);
            Console.Write("Итог: ");
            for (int i = 0; i < num_alt; i++)
                tempi[i] = resDom[num_k - 1, i];
            PrintVector(tempi);

            //Турнирный
            Console.WriteLine("\n\tТУРНИРНЫЙ МЕХАНИЗМ:");
            Tournament(bo, resTour);
            Console.Write("Вариант" + "\t");
            for (int i = 0; i < num_alt; i++)
                Console.Write((i + 1) + "\t");
            Console.WriteLine();
            for (int i = 0; i < num_k; i++)
            {
                Console.Write("R" + (i + 1) + ":\t");
                for (int j = 0; j < num_alt; j++)
                    tempd[j] = resTour[i, j];
                PrintDoubleVector(tempd);
            }
            Console.WriteLine();
            Weight(resTour, znach);
            Console.Write("Итог: ");
            for (int j = 0; j < num_alt; j++)
                tempd[j] = resTour[num_k - 1, j];
            PrintDoubleVector(tempd);
            Console.WriteLine();
            Console.WriteLine();
            Console.ReadLine();
        }

        //механизм доминирования
        static void Domin(int[,,] bo, int[,] result)
        {
            for (int i = 0; i < num_k; i++)
            {
                for (int j = 0; j < num_alt; j++)
                {
                    result[i, j] = 1;
                    for (int k = 0; k < num_alt && (result[i, j] != 0); k++)
                        if (j != k && bo[i, j, k] == 0) result[i, j] = 0;
                }
            }
        }

        //механизм блокировки
        static void Block(int[,,] bo, int[,] result)
        {
            for (int i = 0; i < num_k; i++)
            {
                for (int j = 0; j < num_alt; j++)
                {
                    result[i, j] = 1;
                    for (int k = 0; (k < num_alt) && (result[i, j] != 0); k++)
                        if ((k != j) && bo[i, k, j] != 0) result[i, j] = 0;
                }
            }
        }

        //турнирный механиз
        static void Tournament(int[,,] bo, double[,] res)
        {
            for (int i = 0; i < num_k; i++)
            {
                for (int j = 0; j < num_alt; j++)
                {
                    res[i, j] = 0;
                    for (int k = 0; k < num_alt; k++)
                    {
                        if ((k != j) && (bo[i, j, k] != 0))
                        {
                            if (bo[i, k, j] == 0) res[i, j] += 1;
                            else res[i, j] += 0.5;
                        }
                    }
                }
            }
        }

        // Взвешенный механизм выбора
        static void Weight(double[,] result, double[] znach)
        {
            for (int i = 0; i < num_alt; i++)
            {
                result[num_k - 1, i] = 0;
                for (int j = 0; j < num_k; j++)
                    result[num_k - 1, i] += result[j, i] * znach[j];
            }
        }

        // Мажоритарный механизм порождения Ф.В.
        static void FunCh(int[,] result)
        {
            for (int i = 0; i < num_alt; i++)
            {
                int num = 0;
                for (int j = 0; j < num_k; j++)
                    num += result[j, i] != 0 ? 1 : 0;

                if (num >= num_k / 2)
                    result[num_k - 1, i] = 1;
                else
                    result[num_k - 1, i] = 0;
            }
        }

        static void PrintDoubleVector(double[] result, bool width = true)
        {
            bool ch = false;
            int[] temp = { 0, 0, 0, 0, 0, 0 };
            int i, j, count;
            for (i = 0; i < num_alt; i++)
                temp[i] = 0;
            for (i = 0; i < num_alt; i++)
                for (j = 0; j < num_alt; j++)
                    if (result[i] >= result[j])
                        temp[i]++;
            for (i = 0; i < num_alt; i++)
            {
                count = 0;
                for (j = 0; j < num_alt; j++)
                {
                    if (temp[j] == temp[i])
                        count++;
                }
                ch = false;

                for (j = 0; j < count; j++)
                {
                    if (ch) Console.Write("-");
                    ch = true;
                    Console.Write(num_alt - temp[i] + j + 1);
                }
                Console.Write("; ");
                if (i != 6)
                {
                    if (width)
                        Console.Write("\t");
                }
            }
            Console.WriteLine();
        }

        static void PrintVector(int[] result)
        {
            int i;
            bool ch = false;
            for (i = 0; i < num_alt; i++)
            {
                if (result[i] == 1)
                {
                    if (ch)
                        Console.Write(", ");
                    ch = true;
                    Console.Write(i + 1);
                }
            }
            if (!ch)
                Console.WriteLine("нет выделенных решений;");
            else
                Console.WriteLine(";");
        }
    }
}
