#include<iostream>
#include<fstream>
#include<cstdlib>
#include<cmath>
#include<algorithm>
#include<vector>
#include<queue>
#include<set>
#include<map>
#include<string>
#include <time.h>

using namespace std;
#define int long long
#define fi(i,a,b) for(int i=a; i<(b);i++)
const int N = 200001;

void main(){

	ifstream cin("input.txt");
	ofstream cout("output.txt");


	srand(time(NULL));
	cout << (rand() % 2)+1 << endl;
}