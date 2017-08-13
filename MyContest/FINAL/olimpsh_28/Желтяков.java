import java.io.BufferedWriter;
import java.io.File;
import java.io.FileWriter;
import java.io.IOException;
import java.util.Scanner;

/**
 * Created by olimpsh_28 on 4/2/2017.
 */
public class app {

    public static void main(String[] args) throws Exception {
        try {
            Scanner scanner = new Scanner(new File("input.txt"));
            int n = scanner.nextInt();
            int my = scanner.nextInt();
            int he = scanner.nextInt();
            double[][] myPos = new double[5][2];
            double[][] himPos = new double[5][2];
            double ballX = scanner.nextDouble();
            double ballY = scanner.nextDouble();
            boolean flagBall = false;
            double futureBallX = scanner.nextDouble();
            double futureBallY = scanner.nextDouble();
            for (int i = 0; i < 5; i++) {
                myPos[i][0] = scanner.nextDouble();
                myPos[i][1] = scanner.nextDouble();
                flagBall = ((myPos[i][0] == ballX) && (myPos[i][1] == ballY) || (flagBall));
            }
            double lengthMin = Math.pow((myPos[0][0] - futureBallX), 2) + Math.pow((myPos[0][1] - futureBallY), 2);
            double lengthMinTwo = Math.pow((myPos[1][0] - futureBallX), 2) + Math.pow((myPos[1][1] - futureBallY), 2);
            int min = 0;
            int twoMin = 1;
            if (lengthMinTwo < lengthMin) {
                double i = lengthMin;
                lengthMin = lengthMinTwo;
                lengthMin = i;
                min = 1;
                twoMin = 0;
            }
            for (int i = 2; i < 5; i++) {
                double l = Math.pow((myPos[i][0] - futureBallX), 2) + Math.pow((myPos[i][1] - futureBallY), 2);
                if (l < lengthMinTwo) {
                    if (l < lengthMin) {
                        twoMin = min;
                        min = i;
                        lengthMinTwo = lengthMin;
                        lengthMin = l;
                    } else {
                        twoMin = i;
                        lengthMinTwo = l;
                    }
                }
            }
            for (int i = 0; i < 5; i++) {
                himPos[i][0] = scanner.nextDouble();
                himPos[i][1] = scanner.nextDouble();
            }
            int lastHaveBall = scanner.nextInt();
            BufferedWriter writer = new BufferedWriter(new FileWriter("output.txt"));
            if (flagBall && ballX >= 70) {
                int i = 0;
                while (!(((myPos[i][0] == ballX) && (myPos[i][1] == ballY)))) {
                    i++;
                }
                double x = 100;
                double y = myPos[i][1];
                double y1 = myPos[i][1];
                double y2 = myPos[i][1];
                while (!checkTrack(himPos, myPos[i], new double[]{x, y}) && !checkTrack(himPos, myPos[i], new double[]{x, y1})) {
                    y += 0.1;
                    y1 -= 0.1;
                    if (y - y2 > 100) {
                        break;
                    }
                }
                if (checkTrack(himPos, myPos[i], new double[]{x, y1}) && !checkTrack(himPos, myPos[i], new double[]{x, y})) {
                    y = y1;
                }
                for (i = 0; i < 5; i++) {
                    writer.write(myPos[i][0] + 2.5 + " " + myPos[i][1] + "\n");
                }
                writer.write(x + " " + y);
            } else {
                if (n < 3 && !flagBall) {
                    writer.write("40 10" + "\n");
                    writer.write("20 30" + "\n");
                    writer.write("40 50" + "\n");
                    writer.write(futureBallX + " " + futureBallY + "\n");
                    writer.write(futureBallX + " " + futureBallY + "\n");

                } else {
                    if (flagBall) {
                /*for (int i = 0; i < 5; i++) {
                    writer.write(myPos[i][0] + 2.5 + " " + myPos[i][1] + "\n");
                }
                writer.write(100 + " " + ballY);*/
                        int i = 0;
                        while (!(((myPos[i][0] == ballX) && (myPos[i][1] == ballY)))) {
                            i++;
                        }
                        int maxX = 0;
                        for (int i1 = 0; i1 < 5; i1++) {
                            if (myPos[i1][0] > myPos[maxX][0]) {
                                maxX = i1;
                            }
                        }
                        if (maxX == i || myPos[maxX][0] - myPos[i][0] <= 5) {
                            i = maxX;
                            int k = i;
                            double y = myPos[i][1];
                            double y1 = myPos[i][1];
                            double y2 = myPos[i][1];
                            double x = myPos[i][0] + 2.5;
                            while (!(checkTrack2(himPos, myPos[i], new double[]{x, y}) || checkTrack(himPos, myPos[i], new double[]{x, y1}))) {
                                y += 0.1;
                                y1 -= 0.1;
                                x = Math.sqrt(2.5 * 2.5 - (myPos[i][1] - y) * (myPos[i][1] - y)) + myPos[i][0];
                            }
                            if (checkTrack2(himPos, myPos[i], new double[]{x, y1}) && !checkTrack(himPos, myPos[i], new double[]{x, y})) {
                                y = y1;
                            }
                            int last = 0;
                            for (i = 1; i < 5; i++) {
                                if (myPos[i][0] < myPos[last][0]) {
                                    last = i;
                                }
                                for (i = 0; i < 5; i++) {
                                    if (i == k) {
                                        writer.write(x + " " + y + "\n");
                                    } else {
                                        if (i == last) {
                                            writer.write(myPos[i][0] + " " + myPos[i][1] + "\n");
                                        } else {
                                            writer.write(myPos[i][0] + 2.5 + " " + myPos[i][1] + "\n");
                                        }
                                    }
                                }
                                //writer.write(x+" "+y+"\n");
                                writer.write("memory " + k);
                            }
                        } else {
                            if (checkTrack(himPos, myPos[i], myPos[maxX])) {
                                for (int j = 0; j < 5; j++) {
                                    if (j != maxX) {
                                        writer.write(myPos[j][0] + 2.5 + " " + myPos[j][1] + "\n");
                                    } else {
                                        writer.write(myPos[j][0] + " " + myPos[j][1] + "\n");
                                    }
                                }
                                writer.write(myPos[maxX][0] + " " + myPos[maxX][0] + "\n");
                            } else {
                                int k = i;
                                int j = 0;
                                if (j == k) {
                                    j++;
                                }
                                if (j == lastHaveBall) {
                                    j++;
                                }
                                double length = 10000000;
                                j = -1;
                                for (i = 0; i < 5; i++) {
                                    if (i != k && i != j && i != lastHaveBall) {
                                        double l = Math.pow(myPos[k][0] - myPos[i][0], 2) + Math.pow(myPos[k][1] - myPos[i][1], 2);
                                        if (l < length) {
                                            int i1 = k;
                                            int j1 = i;
                                            if (myPos[k][0] < myPos[i][0]) {
                                                i1 = i;
                                                j1 = k;
                                            }
                                            if (checkTrack(himPos, myPos[j1], myPos[i1])) {
                                                j = i;
                                            }
                                        }
                                    }
                                }
                                int i1 = 0;
                                int j1 = 0;
                                try {


                                    i = k;
                                    i1 = i;
                                    j1 = j;
                                    if (myPos[i][0] < myPos[j][0]) {
                                        i1 = j;
                                        j1 = i;
                                    }
                                } catch (Exception e) {

                                }
                                try {
                                    if (checkTrack(himPos, myPos[j1], myPos[i1])) {
                                        int last = 0;
                                        for (i = 1; i < 5; i++) {
                                            if (myPos[i][0] < myPos[last][0]) {
                                                last = i;
                                            }
                                        }
                                        for (i = 0; i < 5; i++) {
                                            if (last == i || i == j) {
                                                writer.write(myPos[i][0] + " " + myPos[i][1] + "\n");
                                            } else {
                                                writer.write(myPos[i][0] + 2.5 + " " + myPos[i][1] + "\n");
                                            }
                                        }
                                        writer.write(myPos[j][0] + " " + myPos[j][1] + "\n");
                                        writer.write("memory " + k);
                                    } else {
                                        run(myPos, i, himPos, writer, k);
                                    }
                                } catch (Exception e) {
                                    run(myPos, i, himPos, writer, k);
                                }
                            }
                        }
                    } else {
                        defend(min, twoMin, futureBallX, futureBallY, myPos, writer, lastHaveBall);
                    }
                }
            }
            writer.close();
        } catch (Exception e) {

        }
    }

    public static void defend(int min, int twoMin, double futureBallX, double futureBallY, double[][] myPos, BufferedWriter writer, int lastHaveBall) throws IOException {
        for (int i = 0; i < 5; i++) {
            if (i == min || i == twoMin) {

                writer.write(futureBallX + " " + futureBallY + "\n");

            } else {
                if (myPos[i][0] > 50) {
                    writer.write((myPos[i][0] - 2) + " " + myPos[i][1] + "\n");
                } else {
                    writer.write((myPos[i][0]) + " " + myPos[i][1] + "\n");
                }

            }

        }
        writer.write("memory " + lastHaveBall + "\n");
    }

    public static void run(double[][] myPos, int i, double[][] himPos, BufferedWriter writer, int k) throws IOException {
        try {
            double y = myPos[i][1];
            double y1 = myPos[i][1];
            double y2 = myPos[i][1];
            double x = myPos[i][0] + 2.5;
            while (!(checkTrack2(himPos, myPos[i], new double[]{x, y}) || checkTrack(himPos, myPos[i], new double[]{x, y1}))) {
                y += 0.1;
                y1 -= 0.1;
                x = Math.sqrt(2.5 * 2.5 - (myPos[i][1] - y) * (myPos[i][1] - y)) + myPos[i][0];
            }
            if (checkTrack2(himPos, myPos[i], new double[]{x, y1}) && !checkTrack(himPos, myPos[i], new double[]{x, y})) {
                y = y1;
            }
            int last = 0;
            for (i = 1; i < 5; i++) {
                if (myPos[i][0] < myPos[last][0]) {
                    last = i;
                }
                for (i = 0; i < 5; i++) {
                    if (i == k) {
                        writer.write(x + " " + y + "\n");
                    } else {
                        if (i == last) {
                            writer.write(myPos[i][0] + " " + myPos[i][1] + "\n");
                        } else {
                            writer.write(myPos[i][0] + 2.5 + " " + myPos[i][1] + "\n");
                        }
                    }
                }
                //writer.write(x+" "+y+"\n");
                writer.write("memory " + k);
            }
        } catch (Exception e) {

        }
    }

    public static boolean checkTrack(double[][] himPos, double[] fromPos, double[] toPos) {
        try {
            for (double x = fromPos[0]; x < toPos[0]; x += 0.1) {
                double y = (x - fromPos[0]) / (toPos[0] - fromPos[0]) * (toPos[1] - fromPos[1]) + fromPos[1];
                for (int i = 0; i < 5; i++) {
                    double l = Math.sqrt(Math.pow(himPos[i][0] - x, 2) + Math.pow(himPos[i][1] - y, 2));
                    if (l <= (x - fromPos[0]) / 6 * 2.5 + 3.5) {
                        return false;
                    }
                }
            }
        }finally {

        }
        return true;
    }

    public static boolean checkTrack2(double[][] himPos, double[] fromPos, double[] toPos) {
        try{
        for (double x = fromPos[0]; x < toPos[0]; x += 0.1) {
            double y = (x - fromPos[0]) / (toPos[0] - fromPos[0]) * (toPos[1] - fromPos[1]) + fromPos[1];
            for (int i = 0; i < 5; i++) {
                double l = Math.sqrt(Math.pow(himPos[i][0] - x, 2) + Math.pow(himPos[i][1] - y, 2));
                if (l <= (x - fromPos[0]) + 3.5) {
                    return false;
                }
            }
        }}
        finally {
            
        }
        return true;
    }

}