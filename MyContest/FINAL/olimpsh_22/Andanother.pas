var fin,fout:text;
    myplayerx, myplayery, enemyplayerx, enemyplayery, turnx, turny: array[1..5] of real;
    ballx, bally, balldestx, balldesty, makepasx, makepasy:real;
    myscore, enemyscore, pas, pastime, turn, i, nothing, own, danger, initiate, k, aim:integer;
begin
assign(fin,'input.txt');
assign(fout,'output.txt');                                              
reset(fin);                                                           
rewrite(fout);
readln(fin, turn, myscore, enemyscore);
readln(fin, ballx, bally, balldestx, balldesty);               
for i:=1 to 5 do
readln(fin, myplayerx[i], myplayery[i]);                 
for i:=1 to 5 do                                           
readln(fin, enemyplayerx[i], enemyplayery[i]);
pas:=0; pastime:=0;
initiate:=0;
danger:=0;
//if turn=0 then
readln(fin, initiate); 
initiate:=initiate-1;
//if turn>0 then
//readln(pas, pastime);
own:=0;
for i:=1 to 5 do
begin 
if (myplayerx[i]=ballx) and (myplayery[i]=bally) then
own:=i;
end;


if (own=0) and (initiate<1)  then
begin
turnx[5]:=ballx;
turny[5]:=bally;
turnx[4]:=myplayerx[5]-10;
turny[4]:=myplayery[5]-10;
turnx[3]:=myplayerx[5]-10;
turny[3]:=myplayery[5]+10;
turnx[2]:=myplayerx[5]-20;
turny[2]:=myplayery[5]-10;
turnx[1]:=myplayerx[5]-20;
turny[1]:=myplayery[5]+10;
end else
begin
if own>0 then
initiate:=3;
for i:=1 to 5 do
if own>0 then
if (abs(enemyplayerx[i]-myplayerx[own])<10) and (abs(enemyplayery[i]-myplayery[own])<10) and (enemyplayerx[i]-myplayerx[own]>0) then
danger:=1;
turnx[5]:=myplayerx[5]+3;
turny[5]:=myplayery[5];
turnx[4]:=myplayerx[5]-10;
turny[4]:=myplayery[5]-10;
turnx[3]:=myplayerx[5]-10;
turny[3]:=myplayery[5]+10;
turnx[2]:=myplayerx[5]-20;
turny[2]:=myplayery[5]-10;
turnx[1]:=myplayerx[5]-20;
turny[1]:=myplayery[5]+10;
if own =0 then
for i:=1 to 5 do
begin
turnx[i]:=myplayerx[i];
turny[i]:=myplayery[i];
end;
end;




for i:=1 to 5 do
writeln(fout, turnx[i],' ',turny[i]);


if own>0 then
if (danger=1) and (myplayerx[own]<80)   then
begin
if own=5 then
writeln(fout, myplayerx[4],' ', myplayery[4]);
if own=4 then
writeln(fout, myplayerx[1],' ', myplayery[1]);
if own=3 then
writeln(fout, myplayerx[5],' ', myplayery[5]);




end else
begin
if own=2 then
writeln(fout, myplayerx[1],' ', myplayery[1]);
if own=1 then
writeln(fout, myplayerx[3],' ', myplayery[3]);
end;





if own>0 then
if myplayerx[own]>80 then
writeln(fout, myplayerx[own]+30, ' ',myplayery[own]+10);



writeln(fout, 'memory ', initiate);
close(fin);
close(fout);
end.