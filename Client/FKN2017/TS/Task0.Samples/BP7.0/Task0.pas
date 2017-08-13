program Task0;

const
  InputFileName = 'input.txt';
  OutputFileName = 'output.txt';

var
  A, B, C: Integer;
  Input, Output: Text;

begin
  Assign(Input, InputFileName);
  Reset(Input);
  Assign(Output, OutputFileName);
  Rewrite(Output);

  ReadLn(Input, A, B);

  C := A + B;

  WriteLn(Output, C);

  Close(Input);
  Close(Output);
end.
