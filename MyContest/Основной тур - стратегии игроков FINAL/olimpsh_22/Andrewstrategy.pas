var fin,fout:text;
    myplayerx, myplayery, enemyplayerx, enemyplayery, turnx, turny: array[1..5] of real;
    ballx, bally, balldestx, balldesty:real;
    myscore, enemyscore, pas, pastime, turn, i, nothing:integer;
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

//if turn=0 then
readln(fin, nothing); 

//if turn>0 then
//readln(pas, pastime);



turnx[5]:=ballx;
turny[5]:=bally;

/////////////////////////
if (myscore+enemyscore) mod 2=0  then
begin
turnx[3]:=myplayerx[1]+30;
turny[3]:=myplayery[1]+12;
end;
if (myscore+enemyscore) mod 2=1  then
begin
turnx[3]:=myplayerx[1]+30;
turny[3]:=myplayery[1]-12;
end;



if (abs(balldestx-myplayerx[3])<2.6) and (abs(balldesty-myplayery[3])<2.6) then
begin
turnx[3]:=balldestx;
turny[3]:=balldesty;
end;

if (ballx=myplayerx[3]) and (bally=myplayery[3]) then


begin 
turnx[3]:=ballx+20;
turny[3]:=bally;
end;





///////////////////////

turnx[4]:=myplayerx[3]-30;
turny[4]:=myplayery[3];

if (abs(balldestx-myplayerx[4])<2.6) and (abs(balldesty-myplayery[4])<2.6) then
begin
turnx[4]:=balldestx;
turny[4]:=balldesty;
end;

turnx[2]:=ballx-25;
turny[2]:=bally;



turnx[1]:=ballx-10;
turny[1]:=bally+10;




if (abs(balldestx-myplayerx[1])<2.6) and (abs(balldesty-myplayery[1])<2.6) then
begin
turnx[5]:=myplayerx[1];
turny[5]:=myplayery[1]-10;
turnx[4]:=myplayerx[1];
turny[4]:=myplayery[1]+30;
turnx[1]:=balldestx;
turny[1]:=balldesty;
end;

{if (abs(ballx-myplayerx[1])<10) and (abs(bally-myplayery[1])<10) then
begin
if (balldestx>myplayerx[1]) or (ballx<myplayerx[1]) then
begin
turnx[1]:=balldestx;
turny[1]:=balldesty;
end
else
begin    
turnx[1]:=myplayerx[1];
turny[1]:=((balldestx-ballx)/(balldestx-myplayerx[1]))*(balldesty-bally)+myplayery[1];
end
end;}



{if (abs(ballx-myplayerx[2])<10) and (abs(bally-myplayery[2])<10) then
begin
if (balldestx>myplayerx[2])  or (ballx<myplayerx[2])then
begin
turnx[2]:=balldestx;
turny[2]:=balldesty;
end
else
begin    
turnx[2]:=myplayerx[2];
turny[2]:=((balldestx-ballx)/(balldestx-myplayerx[2]))*(balldesty-bally)+myplayery[2];
end
end;}
if (abs(ballx-myplayerx[2])<10) and (abs(bally-myplayery[2])<10) and (abs(myplayerx[1]-myplayerx[2])>7) then
begin
turnx[2]:=balldestx;
turny[2]:=balldesty;
end;


for i:=1 to 5 do
writeln(fout, turnx[i], ' ',turny[i]);

//if (ballx-myplayerx[3]<5) and (bally-myplayery[3]<5) then
//begin
//if (myplayerx[3]>70) then
//writeln(fout, 120, myplayery[3]);

//end
//else
if ((abs(ballx-myplayerx[4])<2) and (abs(bally-myplayery[4])<2))  {or (abs(ballx-myplayerx[2])<2.6) and (abs(bally-myplayery[2])<2.6) }  then
writeln(fout, myplayerx[3], ' ', myplayery[3]) else

if ((abs(ballx-myplayerx[1])<2) and (abs(bally-myplayery[1])<2))  {or (abs(ballx-myplayerx[2])<2.6) and (abs(bally-myplayery[2])<2.6) }  then
writeln(fout, myplayerx[4], ' ', myplayery[4]) else


if ((abs(ballx-myplayerx[3])>8) or (abs(bally-myplayery[3])>8)) and ((abs(ballx-myplayerx[4])>8) or (abs(bally-myplayery[4])>8)) then
writeln(fout, myplayerx[1], ' ', myplayery[1])


else 
if myplayerx[3]>75 then
writeln(fout, 101,' ',bally+15);


//writeln('memory ',pas, pastime

close(fin);
close(fout);
end.