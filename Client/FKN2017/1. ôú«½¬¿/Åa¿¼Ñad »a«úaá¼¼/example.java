package com.company;

import java.io.File;
import java.io.FileNotFoundException;
import java.io.PrintWriter;
import java.util.Scanner;

public class Main {

    public static void main(String[] args) {
        // Номер хода
        int turnNum;
        // Расположение фишек на доске(двумерный массив)
        int[][] board = new int[10][10];

        // Чтение входныех данных
        try {
            Scanner scanner = new Scanner(new File("input.txt"));

            // Номер хода
            turnNum = Integer.parseInt(scanner.nextLine());

            // Состояние доски
            for (int lineNum = 0; lineNum < 10; lineNum++)
            {
                String line = scanner.nextLine();
                String[] cells = line.split(" ");

                for (int columnNum = 0; columnNum < 10; columnNum++)
                {
                    board[lineNum][columnNum] = Integer.parseInt(cells[columnNum]);
                }
            }

            scanner.close();
        } catch (FileNotFoundException e) {
            e.printStackTrace();
        }

        // Пример простой стратегии

        // Перебираем все клетки доски
        for (int row = 0; row < 10; row++) {
            for (int column = 0; column < 10; column++) {
                if (board[row][column] == 1) {  // Если в клетке наша фишка
                    if (column + 1 < 10 && board[row][column + 1] == 0) {   // Если клетка справа не выходит за пределы доски и в ней пусто
                        try {
                            PrintWriter writer = new PrintWriter("output.txt"); // Записываем ход в выходной файл
                            writer.println(
                                Integer.toString(column) + " " +        // Индекс столбца откуда ходим
                                Integer.toString(row) + " " +           // Индекс строки откуда ходим
                                Integer.toString(column + 1) + " " +    // Индекс столбца куда ходим
                                Integer.toString(row)                   // Индекс строки куда ходим
                            );
                            writer.close();
                        } catch (FileNotFoundException e) {
                            e.printStackTrace();
                        }

                        return;             // Завершаем программу
                    }

                    // Продолжаем перебор
                }
            }
        }
    }
}
