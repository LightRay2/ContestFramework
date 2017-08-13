import java.awt.*;
import java.io.File;
import java.io.FileNotFoundException;
import java.util.Scanner;

import static java.lang.Math.sqrt;


public class Main {
    private static Point[] m = new Point[5];

    private static int number;
    private static int st1;
    private static int st2;

    private static Point ball = new Point();
    private static Point ball2 = new Point();

    private static Point[] myCommand = new Point[5];
    private static Point[] nextMc = new Point[5];
    private static Point[] badCommand = new Point[5];
    private static boolean flag = false;
    private static Point face = new Point();

    private static boolean ff = false;

    private static double v = 2;

    private static boolean attack = false;
    private static int ballflag = -1;



    public static void main(String[] args) {

        init();
        inp();

        strat();




        output();


    }

    private static void strat() {
        int f1 = 0;
        for(int i = 0; i < 5; i++){
            if (myCommand[i].getX()==ball.getX()&&myCommand[i].getY()==ball.getY()){
                f1 = 1;
                ballflag = i;
                break;
            }
            if (badCommand[i].getX()==ball.getX()&&badCommand[i].getY()==ball.getY()){
                f1 = -1;
                ballflag = i;
                break;
            }
        }
        if(f1 == 1) stratA(); else if (f1 == -1) stratD(); else StratG();

    }

    private static void StratG() {
        if (attack) stratA();else stratD();
        //поиск ближ игроков к мячу
        //бег к нему на перехват
        double arr[] = new double[5];
        Point arp[] = new Point[5];
        for (int i = 0; i < 5; i++){
            boolean b = false;
            for(int j = 0; j < 6; j++){

                Point p = new Point();
                p.setLocation(ball.getX()+(ball2.getX() - ball.getX())/6*j,
                              ball.getY()+(ball2.getY() - ball.getY())/6*j);
                double length = sqrt(Math.pow(p.getX() - myCommand[i].getX(),2) +
                                     Math.pow(p.getY() - myCommand[i].getY(),2));
                //временно поставим v 2, в идеале заменить на точно 2 или 2.5
                if (length/v <= j){
                    //myCommand[i].setLocation(p);
                    arr[i] = length;
                    arp[i] = p;
                    b = true;
                    break;
                }

            }
            if (!b){
                //myCommand[i].setLocation(ball2);
                arr[i] = sqrt(Math.pow(ball2.getX() - myCommand[i].getX(),2) +
                        Math.pow(ball2.getY() - myCommand[i].getY(),2));
                arp[i] = ball2.getLocation();
            }

        }
        double min = 9999999;
        int ind = 0;
        for(int i = 0; i < 5; i++){
            if (arr[i] < min){
                min = arr[i];
                ind = i;
            }
        }
        nextMc[ind] = arp[ind];

    }



    private static void stratD() {
        flag = false;
        for(int i = 0; i < 5; i++){
            int ind = 0;
            double minLenght = 9999999;
            for(int j = 0; j < 5; j++){
                double l =sqrt(Math.pow(myCommand[i].getX()-badCommand[j].getX(),2)+Math.pow(myCommand[i].getY()-badCommand[j].getY(),2));
                if (minLenght>l){
                    ind = j;
                    minLenght = l;
                }
            }
            nextMc[i].setLocation(badCommand[ind]);
            badCommand[ind].setLocation(999999,999999);
        }
    }

    private static void stratA() {
      //stratD();
        if (myCommand[ballflag].getX() >= 80) {
            flag = true;
            face.setLocation(200,myCommand[ballflag].getY()+10);
        }
        else {
            if(isPass()){

            }else sort();
        }

    }

    private static void sort() {
        int [] at = new  int [5];
        at[0] = 20;
        at[1] = 40;
        at[2] = 50;
        at[3] = 60;
        at[4] = 80;
        int [] ay = new  int [5];
        at[0] = 70;
        at[1] = 80;
        at[2] = 60;
        at[3] = 80;
        at[4] = 70;
        for(int i = 0,j = 0; i < 5; i++){
            double min = 99999999;
            int ind = 0;
            for(int z = 0; z < 5; z++ ){
                if(myCommand[z].getX() < min){
                    min = myCommand[z].getX();
                    ind = z;
                    myCommand[z].setLocation(9999999,myCommand[z].getY());
                }
            }
            nextMc[ind].setLocation(at[j],ay[j]);
            j++;
        }


    }

    private static void init() {
        for(int i = 0; i < 5; i++){
            myCommand[i] = new Point();
            badCommand[i] = new Point();
            nextMc[i] = new Point();
            m[i] = new Point();
        }
        m[0].setLocation(7,3);
        m[1].setLocation(7,7);
        m[2].setLocation(3,7);
        m[3].setLocation(12,12);
    }


    private static void output() {
        for(int i = 0; i < 5; i++){
            FileHelper.write(nextMc[i].getX() + " " + nextMc[i].getY() + "\r\n");
        }
        if(flag){
            FileHelper.write(face.getX() + " " + face.getY() + '\n');
        }
    }
    private static void output(String s){
        for(int i = 0; i < 5; i++){
            FileHelper.write(nextMc[i].getX() + " " + nextMc[i].getY() + "\r\n");
        }
        if(flag){
            FileHelper.write(face.getX() + " " + face.getY() + '\n');
        }
        FileHelper.write("memory " + s);
    }


    private static void inp(){

        File file = new File("input.txt");
        Scanner scan = null;
        try {
            scan = new Scanner(file);
        } catch (FileNotFoundException e) {
            e.printStackTrace();
        }

        //d = scan.nextDouble();

            number = (int) scan.nextDouble();
            st1 = (int) scan.nextDouble();
            st2 = (int) scan.nextDouble();

            ball.setLocation(scan.nextDouble(),scan.nextDouble());
            ball2.setLocation(scan.nextDouble(),scan.nextDouble());

            for(int i = 0; i < 5; i++){
                myCommand[i].setLocation(scan.nextDouble(),scan.nextDouble());
            }
            for(int i = 0; i < 5; i++){
                badCommand[i].setLocation(scan.nextDouble(),scan.nextDouble());
            }
            int p = scan.nextInt();
            if (p == 1) ff = true; else ff = false;

    }


    public static boolean isPass() {
        boolean mf = false;
        double x = myCommand[ballflag].getX();
        double y = myCommand[ballflag].getY();
        for(int i = 0; i<5; i++ ){
            if (x < myCommand[i].getX()&&sqrt(Math.pow(x - myCommand[i].getX(),2) +
                    Math.pow(y - myCommand[i].getY(),2))<=30 ){
                flag = true;
                face.setLocation(myCommand[i].getLocation());
                mf = true;
                break;
            }
        }

        return mf;
    }
}
