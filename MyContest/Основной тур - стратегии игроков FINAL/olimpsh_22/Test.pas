var fin,fout:text;
    myplayersx, myplayersy, enemyplayersx, enemyplayersy:array[1,,5] of real;
    ballx, bally, balldestx, balldesty:real
begin
assign(fin,'input.txt');
assign(fout,'output.txt');
reset(fin);
rewrite(fout);
readln();






close(fin);
close(fout);
end.