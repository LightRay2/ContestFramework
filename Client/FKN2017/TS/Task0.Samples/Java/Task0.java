import java.util.*;
import java.io.*;


public class Task0
{
  public static final String
    InputFileName = "input.txt",
    OutputFileName = "output.txt";


  public static void main(String[] args) throws Exception
  {
    Scanner input = new Scanner(new File(InputFileName));
    PrintWriter output = new PrintWriter(new File(OutputFileName));

    int a = input.nextInt();
    int b = input.nextInt();

    int c = a + b;

    output.println(c);

    input.close();
    output.close();
  }
}
