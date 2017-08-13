#include <fstream.h>


const char
  *inputFileName = "input.txt",
  *outputFileName = "output.txt";


void main() {
  ifstream input(inputFileName);
  ofstream output(outputFileName);

  int a, b;
  input >> a >> b;

  int c = a + b;

  output << c << endl;

  input.close();
  output.close();
}
