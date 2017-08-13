import java.io.*;
import java.util.Scanner;


public class FileHelper {
    private static boolean f = false;
    public static void write(String text) {

        try (FileWriter writer = new FileWriter("output.txt", f)) {
            f = true;
            writer.write(text);

            writer.flush();
        } catch (IOException ex) {

            System.out.println(ex.getMessage());
        }
    }

    public static double read() throws FileNotFoundException {
        double d;
        File file = new File("input.txt");
        Scanner scan = new Scanner(file);

        d = scan.nextDouble();

        return d;
    }
}


