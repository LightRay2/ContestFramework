#include <fstream>

using namespace std;


const string
  inputFileName = "input.txt",
  outputFileName = "output.txt";


void main() {
  ifstream input(inputFileName.c_str());
  ofstream output(outputFileName.c_str());

  int a, b;
  input >> a >> b;

  int c = a + b;

  output << c << endl;

  input.close();
  output.close();
}
