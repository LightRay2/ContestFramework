using System;
using System.Text;
using System.IO;


namespace Turncoat.Server
{
    public enum CellValue { Empty, WhiteChip, BlackChip, OutField }


    public class TurncoatGame
    {
        public const int DEFAULT_FIELD_SIZE = 8;

        public CellValue[,] Field() {
            return field;
        }

        private CellValue[,] field = null;
        private int colCount = 0,
                    rowCount = 0;


        public TurncoatGame(int rowCount, int colCount) {
            if (rowCount < 6 || rowCount % 2 == 1 || colCount < 6 || colCount % 2 == 1)
                throw new TurncoatGameException(string.Format("Incorrect Turncoat field size: {0} x {1}", rowCount, colCount));

            field = new CellValue[rowCount, colCount];
            ClearField();
            field[rowCount / 2 - 1, colCount / 2 - 1] = CellValue.WhiteChip;
            field[rowCount / 2 - 1, colCount / 2] = CellValue.BlackChip;
            field[rowCount / 2, colCount / 2 - 1] = CellValue.BlackChip;
            field[rowCount / 2, colCount / 2] = CellValue.WhiteChip;
        }

        public TurncoatGame(int size) : this(size, size) { }

        public TurncoatGame() : this(DEFAULT_FIELD_SIZE) { }


        public TurncoatGame(string configFile) {
            StreamReader sr = null;
            try {
                sr = new StreamReader(configFile, Encoding.Default);
                string line = sr.ReadLine();
                string[] parts = line.Split(' ');
                rowCount = int.Parse(parts[0]);
                colCount = int.Parse(parts[1]);

                field = new CellValue[rowCount, colCount];
                ClearField();
                for (int r = 0; r < rowCount; r++) {
                    line = sr.ReadLine();
                    for (int c = 0; c < colCount; c++)
                        switch (line[c].ToString().ToUpper()[0]) {
                            case ('W'):
                                field[r, c] = CellValue.WhiteChip;
                                break;
                            case ('B'):
                                field[r, c] = CellValue.BlackChip;
                                break;
                        }
                }
            } catch (Exception e) {
                throw new TurncoatGameException(string.Format("Error load Turncoat field from file \"{0}\"", configFile), e);
            } finally {
                try {
                    sr.Close();
                } catch (Exception) { }
            }
        }


        private void ClearField() {
            for (int r = 0; r < rowCount; r++)
                for (int c = 0; c < colCount; c++)
                    field[r, c] = CellValue.Empty;
        }


        /*
          Проверяет возможность игрока сделать ход в [row2, col2] фишкой [row1, col1];
        */
        public bool IsStepAllow(CellValue chip, int row1, int col1, int row2, int col2) {
            if (this[row2, col2] != CellValue.Empty ||
                chip != CellValue.WhiteChip && chip != CellValue.BlackChip ||
                chip != this[row1, col1])
                return false;
            int dRow = row2 - row1,
                dCol = col2 - col1;
            if (this[row2, col2] == CellValue.Empty && ((Math.Abs(dRow) <= 2 && Math.Abs(dCol) <= 2) && !(dRow == 0 && dCol == 0))
                && Math.Abs(dRow) + Math.Abs(dCol) != 3
                )
                return true;
            return false;
        }


        /*
          Проверяет возможность игрока сделать ход
        */
        public bool IsStepAllow(CellValue chip) {
            if (NoEmptyCells) return false;
            for (int r1 = 0; r1 < RowCount; r1++)
                for (int c1 = 0; c1 < ColCount; c1++)
                    if (field[r1, c1] == chip)
                        for (int r2 = r1 - 2; r2 <= r1 + 2; r2++)
                            for (int c2 = c1 - 2; c2 <= c1 + 2; c2++)
                                if (c2 >= 0 && c2 < this.colCount && r2 >= 0 && r2 < this.rowCount)
                                    if (IsStepAllow(chip, r1, c1, r2, c2))
                                        return true;
            return false;
        }

        /* 
          Попытка выполнения хода;
          возвращает на сколько фишек у игрока стало больше в результате данного хода:
            если 1 - то фражеские фишки захвачены не были;
            если 0 - то ход недопустимый, даже в [row, col] фишка не добавлена;
          если fieldUpdate - то поле изменяется, иначе нет (проходит только проверка эффективности хода)
        */
        private int TryStep(CellValue chip, int row1, int col1, int row2, int col2, bool fieldUpdate) {
            if (!IsStepAllow(chip, row1, col1, row2, col2))
                return 0;
            int dRow = row2 - row1,
                dCol = col2 - col1;
            CellValue concurrentChip = chip == CellValue.WhiteChip ? CellValue.BlackChip : CellValue.WhiteChip;
            int result = 1;
            if (fieldUpdate)
                field[row2, col2] = chip;
            if (Math.Abs(dCol) == 2 || Math.Abs(dRow) == 2)
                field[row1, col1] = CellValue.Empty;

            for (int i = row2 - 1; i <= row2 + 1; i++)
                for (int j = col2 - 1; j <= col2 + 1; j++) {
                    if (i >= 0 && i < this.RowCount && j >= 0 && j < this.ColCount && !(row2 == i && col2 == j))
                        if (field[i, j] != CellValue.Empty && field[i, j] != CellValue.OutField && field[i, j] != chip) {
                            field[i, j] = chip;
                        }
                }

            return result;
        }


        /* 
          Выполнения хода;
          возвращает на сколько фишек у игрока стало больше в результате данного хода:
            если 1 - то фражеские фишки захвачены не были;
            если 0 - то ход недопустимый, даже в [row, col] фишка не добавлена
        */
        public int Step(CellValue chip, int row1, int col1, int row2, int col2) {
            return TryStep(chip, row1, col1, row2, col2, true);
        }


        /*
          Проверка, есть ли на поле пустые клетки
        */
        public bool NoEmptyCells {
            get {
                for (int r = 0; r <= RowCount; r++)
                    for (int c = 0; c <= ColCount; c++)
                        if (this[r, c] == CellValue.Empty)
                            return false;

                return true;
            }
        }


        /*
          Проверка, завершена ли игра;
        */
        public bool Finished {
            get {
                return this[CellValue.WhiteChip] == 0 || this[CellValue.BlackChip] == 0 ||
                       !IsStepAllow(CellValue.WhiteChip) && !IsStepAllow(CellValue.BlackChip);
            }
        }



        public int RowCount { get { return rowCount; } }
        public int ColCount { get { return rowCount; } }

        public CellValue this[int row, int col] {
            get {
                return field == null ||
                       row < 0 || row >= RowCount ||
                       col < 0 || col >= ColCount
                         ? CellValue.OutField
                         : field[row, col];
            }
        }


        /*
          Количество фишек на поле определенного игрока (или пустых)
        */
        public int this[CellValue player] {
            get {
                int count = 0;
                for (int r = 0; r <= RowCount; r++)
                    for (int c = 0; c <= ColCount; c++)
                        if (this[r, c] == player)
                            count++;

                return count;
            }
        }
    }


    public class TurncoatGameException : ApplicationException
    {
        public TurncoatGameException()
            : base() {
        }

        public TurncoatGameException(string message)
            : base(message) {
        }

        public TurncoatGameException(string message, Exception innerException)
            : base(message, innerException) {
        }
    }

    
}
