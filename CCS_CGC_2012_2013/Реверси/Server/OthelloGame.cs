using System;
using System.Text;
using System.IO;


namespace Othello.Server
{
  public enum CellValue { Empty, WhiteChip, BlackChip, OutField }


  public class OthelloGame
  {
    public const int DEFAULT_FIELD_SIZE = 8;


    private CellValue[,] field = null;
    private int colCount = 0,
                rowCount = 0;


    public OthelloGame(int rowCount, int colCount) {
      if (rowCount < 6 || rowCount % 2 == 1 ||
          colCount < 6 || colCount % 2 == 1)
        throw new OthelloGameException(string.Format("Incorrect Othello field size: {0} x {1}", rowCount, colCount));

      field = new CellValue[rowCount, colCount];
      ClearField();
      field[rowCount / 2 - 1, colCount / 2 - 1] = CellValue.WhiteChip;
      field[rowCount / 2 - 1, colCount / 2] = CellValue.BlackChip;
      field[rowCount / 2, colCount / 2 - 1] = CellValue.BlackChip;
      field[rowCount / 2, colCount / 2] = CellValue.WhiteChip;
    }

    public OthelloGame(int size): this(size, size) {
    }

    public OthelloGame(): this(DEFAULT_FIELD_SIZE) {
    }


    public OthelloGame(string configFile) {
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
      }
      catch (Exception e) {
        throw new OthelloGameException(string.Format("Error load Othello field from file \"{0}\"", configFile), e);
      }
      finally {
        try {
          sr.Close();
        }
        catch (Exception) { }
      }
    }


    private void ClearField() {
      for (int r = 0; r < rowCount; r++)
        for (int c = 0; c < colCount; c++)
          field[r, c] = CellValue.Empty;
    }


    /*
      ��������� ����������� ������ ������� ��� � [row, col];
      � ������� �� ������� ��������� ������ ���� ������ ����� � ������� ����������, ������ �� ����������
    */
    public bool IsStepAllow(CellValue chip, int row, int col) {
      if (this[row, col] != CellValue.Empty ||
          chip != CellValue.WhiteChip && chip != CellValue.BlackChip)
        return false;

      CellValue concurrentChip = chip == CellValue.WhiteChip ? CellValue.BlackChip : CellValue.WhiteChip;
      bool concurrentChipFound = false;
      for (int r = row - 1; r <= row + 1; r++)
        for (int c = col - 1; c <= col + 1; c++)
          if (this[r, c] == concurrentChip)
            concurrentChipFound = true;
      if (!concurrentChipFound)
        return false;

      return true;
    }


    /*
      ��������� ����������� ������ ������� ��� ���� �� � ���� (�����) ������ ����;
      � ������� �� ������� ��������� ������ ���� ������ ����� � ������� ����������, ������ �� ����������
    */
    public bool IsStepAllow(CellValue chip) {
      for (int r = 0; r <= RowCount; r++)
        for (int c = 0; c <= ColCount; c++)
          if (IsStepAllow(chip, r, c))
            return true;

      return false;
    }


    /*
      ��������� ����������� ������ ������� ��� � [row, col];
      �� �������� ������������ ����
    */
    public bool IsStepAllowClassic(CellValue chip, int row, int col) {
      if (!IsStepAllow(chip, row, col))
        return false;

      return TryStep(chip, row, col, false) >= 2;
    }


    /*
      ��������� ����������� ������ ������� ��� ���� �� � ���� (�����) ������ ����;
      �� �������� ������������ ����
    */
    public bool IsStepAllowClassic(CellValue chip) {
      for (int r = 0; r <= RowCount; r++)
        for (int c = 0; c <= ColCount; c++)
          if (IsStepAllowClassic(chip, r, c))
            return true;

      return false;
    }


    /* 
      ������� ���������� ����;
      ���������� �� ������� ����� � ������ ����� ������ � ���������� ������� ����:
        ���� 1 - �� ��������� ����� ��������� �� ����;
        ���� 0 - �� ��� ������������, ���� � [row, col] ����� �� ���������;
      ���� fieldUpdate - �� ���� ����������, ����� ��� (�������� ������ �������� ������������� ����)
    */
    private int TryStep(CellValue chip, int row, int col, bool fieldUpdate) {
      if (!IsStepAllow(chip, row, col))
        return 0;

      CellValue concurrentChip = chip == CellValue.WhiteChip ? CellValue.BlackChip : CellValue.WhiteChip;
      int result = 1;
      if (fieldUpdate)
        field[row, col] = chip;
      for (int dr = -1; dr <= 1; dr++)
        for (int dc = -1; dc <= 1; dc++) {
          if (dr == 0 && dc == 0)
            continue;

          int r = row + dr,
              c = col + dc;
          int count = 0;
          while (this[r, c] == concurrentChip) {
            count++;
            r += dr;
            c += dc;
          }
          if (this[r, c] == chip) {
            result += count;
            if (fieldUpdate)
              for (int i = 1; i <= count; i++)
                field[row + dr * i, col + dc * i] = chip;
          }
        }

      return result;
    }


    /* 
      ���������� ����;
      ���������� �� ������� ����� � ������ ����� ������ � ���������� ������� ����:
        ���� 1 - �� ��������� ����� ��������� �� ����;
        ���� 0 - �� ��� ������������, ���� � [row, col] ����� �� ���������
    */
    public int Step(CellValue chip, int row, int col) {
      return TryStep(chip, row, col, true);
    }


    /*
      ��������, ���� �� �� ���� ������ ������
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
      ��������, ��������� �� ����;
      � ������� �� ������� ��������� ������ ���� ������ ����� � ������� ����������, ������ �� ����������
    */
    public bool Finished {
      get {
        return this[CellValue.WhiteChip] == 0 || this[CellValue.BlackChip] == 0 ||
               !IsStepAllow(CellValue.WhiteChip) && !IsStepAllow(CellValue.BlackChip);
      }
    }


    /*
      ��������, ��������� �� ����;
      �� �������� ������������ ����
    */
    public bool FinishedClassic {
      get {
        return this[CellValue.WhiteChip] == 0 || this[CellValue.BlackChip] == 0 ||
               !IsStepAllowClassic(CellValue.WhiteChip) && !IsStepAllowClassic(CellValue.BlackChip);
      }
    }


    public int RowCount { get { return rowCount; } }
    public int ColCount { get { return rowCount; } }

    public CellValue this[int row, int col] {
      get {
        return field == null ||
               row < 0 || row >= RowCount ||
               col<0 || col >= ColCount
                 ? CellValue.OutField
                 : field[row, col];
      }
    }


    /*
      ���������� ����� �� ���� ������������� ������ (��� ������)
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


  public class OthelloGameException: ApplicationException
  {
    public OthelloGameException() : base() {
    }

    public OthelloGameException(string message): base(message) {
    }

    public OthelloGameException(string message, Exception innerException) : base(message, innerException) {
    }
  }
}
