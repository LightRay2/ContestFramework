program SimpleStrategyPas;

var
  // Номер хода
  turnNum: Integer;
  // Расположение фишек на доске(двумерный массив)
  board: array[0..9, 0..9] of integer;
  
  input, output: TextFile;
  column, row: integer;
 
begin
  // Чтение входныех данных
  AssignFile(input, 'input.txt');
  Reset(input);

  // Номер хода
  Read(input, turnNum);

  // Состояние доски
  for row := 0 to 9 do
    for column := 0 to 9 do
      Read(input, board[row, column]);
  
  Close(input);

  // Пример простой стратегии

  // Перебираем все клетки доски
  for row := 0 to 9 do
    for column := 0 to 9 do
      if board[row, column] = 1 then  // Если в клетке наша фишка
        if (column + 1 < 10) and (board[row, column + 1] = 0) then  // Если клетка справа не выходит за пределы доски и в ней пусто
        begin
          Assign(output, 'output.txt'); // Записываем ход в выходной файл
          Rewrite(output);
          Write(output,
            column, ' ',      // Индекс столбца откуда ходим
            row, ' ',         // Индекс строки откуда ходим
            column + 1, ' ',  // Индекс столбца куда ходим
            row);             // Индекс строки куда ходим
          Close(output);
          Exit;               // Завершаем программу
        end;

        // Продолжаем перебор
end.
