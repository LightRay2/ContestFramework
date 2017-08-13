{$APPTYPE CONSOLE}
program OthelloClient;
(*
  Предполагается, что ход делается белой фишкой
*)

uses
  SysUtils;

const
  INPUT_FILENAME = 'input.txt';
  OUTPUT_FILENAME = 'output.txt';

type
  TCellValue = ( Empty, WhiteChip, BlackChip, OutField );
  TFieldRow = array of TCellValue;
  TField = array of TFieldRow;

var
  Field: TField;
  RowCount, ColCount: Integer;
  ResultRow, ResultCol: Integer;


  procedure Load;
  var
    InputFile: Text;
    Str: String;
    R, C: Integer;
  begin
    Assign(InputFile, INPUT_FILENAME);
    FileMode := 0;
    Reset(InputFile);
    ReadLn(InputFile, RowCount, ColCount);
    SetLength(Field, RowCount, ColCount);
    for R := 0 to RowCount - 1 do begin
      ReadLn(InputFile, Str);
      for C := 0 to ColCount - 1 do begin
        Field[R, C] := Empty;
        if UpperCase(Str[C + 1]) = 'W' then
          Field[R, C] := WhiteChip
        else if UpperCase(Str[C + 1]) = 'B' then
          Field[R, C] := BlackChip;
      end;
    end;
    Close(InputFile);
  end;


  procedure Save;
  var
    OutputFile: Text;
  begin
    Assign(OutputFile, OUTPUT_FILENAME);
    Rewrite(OutputFile);
    if (ResultRow >= 0) and (ResultCol >= 0) then
      WriteLn(OutputFile, ResultRow + 1, ' ', ResultCol + 1)
    else
      WriteLn(OutputFile, 'NOT');
    Close(OutputFile);
  end;


  function This(Row, Col: Integer): TCellValue;
  begin
    Result := OutField;
    if (0 <= Row) and (Row < RowCount) and
       (0 <= Col) and (Col < ColCount) then
      Result := Field[Row, Col];
  end;


  (*
    Проверяет возможность игрока сделать ход в [row, col];
    в отличие от оригина допускает делать ходы просто рядом с фишками соперником, ничего не захватывая
  *)
  function IsStepAllow(Row, Col: Integer): Boolean; overload;
  var
    R, C: Integer;
  begin
    Result := False;
    if This(Row, Col) <> Empty then
      Exit;

    for R := Row - 1 to Row + 1 do
      for C := Col - 1 to Col + 1 do
        if This(R, C) = BlackChip then begin
          Result := True;
          Exit;
        end;
  end;


  (*
    Проверяет возможность игрока сделать ход хотя бы в одну (любую) клетку поля;
    в отличие от оригина допускает делать ходы просто рядом с фишками соперником, ничего не захватывая
  *)
  function IsStepAllow: Boolean; overload;
  var
    R, C: Integer;
  begin
    Result := False;
    for R := 0 to RowCount - 1 do
      for C := 0 to ColCount - 1 do
        if IsStepAllow(R, C) then begin
          Result := True;
          Exit;
        end;
  end;


  function TryStep(Row, Col: Integer; FieldUpdate: Boolean): Integer; forward;


  (*
    Проверяет возможность игрока сделать ход в [row, col];
    по правилам оригинальной игры
  *)
  function IsStepAllowOriginal(Row, Col: Integer): Boolean; overload;
  begin
    Result := False;
    if not IsStepAllow(Row, Col) then
      Exit;;

    Result := TryStep(Row, Col, False) >= 2;
  end;


  (*
      Проверяет возможность игрока сделать ход хотя бы в одну (любую) клетку поля;
      по правилам оригинальной игры
  *)
  function IsStepAllowOriginal: Boolean; overload;
  var
    R, C: Integer;
  begin
    Result := False;
    for R := 0 to RowCount - 1 do
      for C := 0 to ColCount - 1 do
        if IsStepAllowOriginal(R, C) then begin
          Result := True;
          Exit;
        end;
  end;


  (*
    Попытка выполнения хода;
    возвращает на сколько фишек у игрока стало больше в результате данного хода:
      если 1 - то фражеские фишки захвачены не были;
      если 0 - то ход недопустимый, даже в [row, col] фишка не добавлена;
    если fieldUpdate - то поле изменяется, иначе нет (проходит только проверка эффективности хода)
  *)
  function TryStep(Row, Col: Integer; FieldUpdate: Boolean): Integer;
  var
    R, C, DR, DC, Count, I: Integer;
  begin
    Result := 0;
    if not IsStepAllow(Row, Col) then
      Exit;

    Result := 1;
    for DR := -1 to 1 do
      for DC := -1 to 1 do begin
        if (DR = 0) and (DC = 0) then
          Continue;

        R := Row + DR;
        C := Col + DC;
        Count := 0;
        while This(R, C) = BlackChip do begin
          Inc(Count);
          Inc(R, DR);
          Inc(C, DC);
        end;
        if This(R, C) = WhiteChip then begin
          Inc(Result, Count);
          if FieldUpdate then
            for I := 0 to Count do
              Field[Row + DR * I, Col + DC * I] := WhiteChip;
        end;
      end;
  end;


  (*
    Выбор оптимального хода
  *)
  procedure Solve;
  var
    R, C, Max, MaxCount, MaxIndex, I: Integer;
    ResultField: array of array of Integer;
  begin
    ResultRow := -1;
    ResultCol := -1;

    Randomize;
    SetLength(ResultField, RowCount, ColCount);

    Max := 0;
    MaxCount := 0;
    for R := 0 to RowCount - 1 do
      for C := 0 to ColCount - 1 do begin
        ResultField[R, C] := TryStep(R, C, False);
        if Max = ResultField[R, C] then
          Inc(MaxCount)
        else if Max < ResultField[R, C] then begin
          Max := ResultField[R, C];
          MaxCount := 1;
        end;
      end;
    if Max > 0 then begin
      MaxIndex := Random(MaxCount);
      I := 0;
      for R := 0 to RowCount - 1 do
        for C := 0 to ColCount - 1 do
          if ResultField[R, C] = Max then begin
            if I = MaxIndex then begin
              ResultRow := R;
              ResultCol := C;
            end;
            Inc(I);
          end;
    end;

    SetLength(ResultField, 0, 0);
  end;


begin
  ResultRow := -1;
  ResultCol := -1;

  Load;

  Solve;

  Save;
end.

