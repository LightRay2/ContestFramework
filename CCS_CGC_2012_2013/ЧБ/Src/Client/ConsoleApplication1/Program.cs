using System;
using System.IO;
using System.Collections.Generic;

public class Random8x8
{
    public enum CellValue { Empty, WhiteChip, BlackChip, OutField }
    public static CellValue[,] field;
    public static int RowCount;
    public static int ColCount;

     struct Step
    {
        public int row1, col1, row2, col2, stik;
        public Step( int r1, int c1, int r2, int c2, int stik) {
            row1 = r1;
            col1 = c1;
            row2 = r2;
            col2 = c2;
            this.stik = stik;
        }
    }

     struct Figure
     {
         public int row, col;
         public CellValue player;
         public Figure(CellValue player, int row, int col){
             this.player = player;
             this.row = row;
             this.col = col;
         }
     }


     public static void Main(string[] args) {
         //string[] text = File.ReadAllLines(@"C:\Users\Ворон\AppData\Local\Temp\Turncoat\4B609968\input.txt");
         List<Figure> Figures = new List<Figure>();

         string[] text = File.ReadAllLines("input.txt");
         if (text[0].Length > 2) {
             string[] parts = text[0].Split(' ');
             string sr1 = Convert.ToString(parts[0]),
                    sc1 = Convert.ToString(parts[1]);
             RowCount = Convert.ToInt16(sr1);
             ColCount = Convert.ToInt16(sc1);
             field = new CellValue[RowCount, ColCount];
             for (int i = 1; i < text.Length; i++) {
                 for (int j = 0; j < text[i].Length; j++) {
                     field[i - 1, j] = text[i][j] == '.' ? CellValue.Empty : text[i][j] == 'W' ? CellValue.WhiteChip : CellValue.BlackChip;
                     if (field[i - 1, j] != CellValue.Empty) {//add figure
                         CellValue c = field[i - 1, j] == CellValue.WhiteChip ? CellValue.WhiteChip : CellValue.BlackChip;
                         Figures.Add(new Figure(c, i - 1, j));
                     }
                 }
             }
         }

         Step step = DoStep(Figures);
         File.WriteAllText("output.txt", string.Format("{0} {1}{4}{2} {3}{4}", step.row1 + 1, step.col1 + 1, step.row2 + 1, step.col2 + 1, Environment.NewLine));
     }

    /// <summary>
    /// Выберает рациональный ход
    /// </summary>
    /// <param name="fs">Все фигуры</param>
    /// <returns></returns>
    static Step DoStep(List<Figure> fs) {
        List<Step> steps =new List<Step>();
        Step bestStep;
        foreach (Figure f in fs) {//по всем фигурам
            if (f.player == CellValue.WhiteChip) {//по всем своим фигурам
                for (int r1 = f.row - 2; r1 <= f.row + 2; r1++)
                    for (int c1 = f.col - 2; c1 <= f.col + 2; c1++)
                        if (c1 >= 0 && c1 < ColCount && r1 >= 0 && r1 < RowCount)
                            if (IsStepAllow(CellValue.WhiteChip, f.row, f.col, r1, c1)) {//по всем возможным ходам
                                int stik = 0;
                                int dRow = r1 - f.row,
                                    dCol = c1 - f.col;
                                if (((Math.Abs(dRow) <= 1 && Math.Abs(dCol) <= 1) && !(dRow == 0 && dCol == 0)))
                                    stik++;
                                for (int r2 = r1 - 1; r2 <= r1 + 1; r2++)//сколько вокруг чужих фигур?
                                    for (int c2 = c1 - 1; c2 <= c1 + 1; c2++)
                                        if (c2 >= 0 && c2 < ColCount && r2 >= 0 && r2 < RowCount) {
                                            if (field[r2, c2] == CellValue.BlackChip)
                                                stik++;
                                        }
                                steps.Add(new Step(f.row, f.col, r1, c1,stik));
                            }
            }
        }
        bestStep = steps[0];
        int best = 0;
        foreach (Step s in steps) {
            if (s.stik > best){
                best = s.stik;
                bestStep = s;
            }
        }

        return bestStep;
    }

    /*
      Проверяет возможность игрока сделать ход в [row2, col2] фишкой [row1, col1];
    */
    public static bool IsStepAllow(CellValue chip, int row1, int col1, int row2, int col2) {
        if (field[row2, col2] != CellValue.Empty ||
            chip != CellValue.WhiteChip && chip != CellValue.BlackChip ||
            chip != field[row1, col1])
            return false;
        int dRow = row2 - row1,
            dCol = col2 - col1;
        if (field[row2, col2] == CellValue.Empty && ((Math.Abs(dRow) <= 2 && Math.Abs(dCol) <= 2) && !(dRow == 0 && dCol == 0)))
            return true;
        return false;
    }


    /*
      Проверяет возможность игрока сделать ход
    */
    public static bool IsStepAllow(CellValue chip) {
        for (int r1 = 0; r1 < RowCount; r1++)
            for (int c1 = 0; c1 < ColCount; c1++)
                if (field[r1, c1] == chip)
                    for (int r2 = r1 - 2; r2 <= r1 + 2; r2++)
                        for (int c2 = c1 - 2; c2 <= c1 + 2; c2++)
                            if (c2 >= 0 && c2 < ColCount && r2 >= 0 && r2 < RowCount)
                                if (IsStepAllow(chip, r1, c1, r2, c2))
                                    return true;
        return false;
    }
}
