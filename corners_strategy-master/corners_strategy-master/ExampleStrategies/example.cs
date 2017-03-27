using System;
using System.IO;

namespace simple_cs
{
    class Program
    {
        static void Main(string[] args)
        {
            // Номер хода
            int turnNum;
            // Расположение фишек на доске(двумерный массив)
            int[,] board = new int[10, 10];


            // Чтение входныех данных
            using (TextReader reader = File.OpenText("input.txt"))// Открываем файл
            {
                // Номер хода
                turnNum = int.Parse(reader.ReadLine());

                // Состояние доски
                for (int lineNum = 0; lineNum < 10; lineNum++)
                {
                    string line = reader.ReadLine();
                    string[] cells = line.Split(' ');

                    for (int columnNum = 0; columnNum < 10; columnNum++)
                    {
                        board[lineNum, columnNum] = Convert.ToInt32(cells[columnNum]);
                    }
                }
            }

            // Пример простой стратегии

            // Перебираем все клетки доски
            for (int row = 0; row < 10; row++)
            {
                for (int column = 0; column < 10; column++)
                {
                    if (board[row, column] == 1)    // Если в клетке наша фишка
                    {
                        if (column + 1 < 10 && board[row, column + 1] == 0) // Если клетка справа не выходит за пределы доски и в ней пусто
                        {
                            File.WriteAllText("output.txt",             // Записываем ход в выходной файл
                                Convert.ToString(column) + " " +        // Индекс столбца откуда ходим
                                Convert.ToString(row) + " " +           // Индекс строки откуда ходим
                                Convert.ToString(column + 1) + " " +    // Индекс столбца куда ходим
                                Convert.ToString(row)                   // Индекс строки куда ходим
                                );

                            return;                             // Завершаем программу
                        }
                        else if (row + 1 < 10 && board[row + 1, column] == 0)   // Если клетка снизу не выходит за пределы доски и в ней пусто
                        {
                            File.WriteAllText("output.txt",
                                Convert.ToString(column) + " " +
                                Convert.ToString(row) + " " +
                                Convert.ToString(column) + " " +
                                Convert.ToString(row + 1)
                                );

                            return;
                        }
                    }

                    // Продолжаем перебор
                }
            }
        }
    }
}
