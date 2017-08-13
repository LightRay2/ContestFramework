#include <stdio.h>


const char
  *inputFileName = "input.txt",
  *outputFileName = "output.txt";


void main() {
  int a, b, c;

  freopen(inputFileName, "r", stdin);
  freopen(outputFileName, "w", stdout);

  scanf("%d %d", &a, &b);

  c = a + b;

  printf("%d\n", c);

  fclose(stdin);
  fclose(stdout);
}
