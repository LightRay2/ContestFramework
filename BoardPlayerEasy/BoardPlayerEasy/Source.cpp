#define _CRT_SECURE_NO_WARNINGS
#include<iostream>
#include<string>
#include<vector>
#include<algorithm>
#include<cstdlib>
#include<cmath>
#include <time.h>
#define fi(i,a,b) for(int i = a; i < (b); i++)

using namespace std;

bool val(int x, int  y) {
	return x >= 0 && y >= 0 && x < 10 && y < 10;
}



void main() {
	freopen("input.txt", "r", stdin)
		;
	freopen("output.txt", "w", stdout);
	int turn; cin >> turn;
	int a[10][10];
	fi(i, 0, 10) {
		fi(j, 0, 10)
			cin >> a[i][j];
	}

	int dx[] = { -1,0,1,0 };
	int dy[] = { 0,1,0,-1 };

	vector<int > fromx, fromy, tox, toy;
	fi(y, 0, 10) {
		fi(x, 0, 10) {
			if (a[y][x] == 1) {
				fi(k, 0, 4) {
					int nextx = x + dx[k];
					int nexty = y + dy[k];
					if (val(nextx, nexty) && a[nexty][nextx] == 0)
					{
						fromx.push_back(x);
						fromy.push_back(y);
						tox.push_back(nextx);
						toy.push_back(nexty);
					}
				}
			}
		}
	}

	srand(time(NULL));
	int index = rand() % fromx.size();
	cout << fromx[index] << " " << fromy[index] << " " << tox[index] << " " << toy[index];
}