#include <iostream>
#include <fstream>
#include <cmath>
#include <algorithm>
#include <limits>
#include <sstream>
#include <vector>
#include <map>
#include<set>
#include <string>
#include<cstdlib>
#include <queue>
#include <time.h>


using namespace std;



#define int long long
#define fi(i, a, b) for(int i = (a); (i) < (b); (i)++)

const int N = 12; //horizontal count
const int M = 12;
const int turns = 200;
const int maxPrice = 30000;

struct unit{
	int t, state, x, hp;
};
int prevType = 5;
int turn, ourMoney, enemyMoney, ourScore, enemyScore;
unit our[N], enemy[N];
int stones[N];
const int memorySize = 200;
int memory[memorySize];
void easy(){
	ofstream cout("output.txt");
	bool buyNow = ourMoney >= maxPrice;
	if (buyNow){
		int t = (prevType + 1);
		if (t > 5) t = 1;
		for (int i = 0; i < 1; i++){
			int hor = rand() % N;
			int vert = rand() % M + 1;
			cout << 0 << " " << t << " " << hor << " " << vert << endl;
			if (our[hor].t == 0){
				prevType = t;
				break;
			}
		}
	}
	fi(hor, 0, N){
		if (our[hor].t != 0 && our[hor].hp < 30)
			cout << 2 << " " << hor << endl;
		cout << 1 << " " << hor << " " << rand() % M + 1 << endl;
	}
	cout << "memory" << prevType;
}

void normal(){
	ofstream cout("output.txt");

	fi(i, 0, N){
		if (stones[i] != 0){
			bool needCoverUp = i != 0;
			bool needCoverDown = i != N - 1;

			bool waitForCoverUp = needCoverUp && our[i - 1].t != 0 && our[i - 1].state > 2;
			bool waitForCoverDown = needCoverDown && our[i + 1].t != 0 && our[i + 1].state > 2;
			bool waitForHarvester = our[i].t != 0 && our[i].state > 2;

			bool readyCoverUp = !needCoverUp || (our[i - 1].x == 1 && our[i - 1].state == 3);
			bool readyCoverDown = !needCoverDown || (our[i + 1].x == 1 && our[i + 1].state == 3);
			if (!waitForHarvester){
				if (readyCoverUp && readyCoverDown){
					cout << 0 << " " << rand() % 5 + 1 << " " << i << " " << stones[i] << endl;
					cout << 1 << " " << i << " " << stones[i] << endl;
				}
				else{
					if (!waitForCoverDown && !waitForCoverUp){
						if (ourMoney < maxPrice * 2 && our[i].t==0)
							return;
						cout << 0 << " " << rand() % 5 + 1 << " " << i + 1 << " " << stones[i] + 1 << endl;
						cout << 1 << " " << i + 1 << " " << stones[i] + 1 << endl;
						cout << 0 << " " << rand() % 5 + 1 << " " << i - 1 << " " << stones[i] + 1 << endl;
						cout << 1 << " " << i - 1 << " " << stones[i] + 1 << endl;
					}
				}
			}
		}
	}
}

void hard(){
	ofstream cout("output.txt");
	bool buyNowTransporter = ((turn) % 15 == 0);
	bool buyNowAll = ((turn) % 15 == 1);
	int finishTile = M / 2 + 2;
	if (buyNowTransporter){
		int ok = 100;
		while (ok > 0){
			int hor = rand() % N;
			if (stones[hor] == 0 && our[hor].t == 0){
				cout << 0 << " " << 1 << " " << hor << " " << finishTile << endl;
				ok = 0;
			}
			ok--;
		}
	}

	if (buyNowAll){
		//send everything
		fi(i, 0, N){
			cout << 1 << " " << i << " " << finishTile << endl;

		}

		//tanks and guns
		fi(i, 0, N){
			int t = (rand() % 3);

			cout << 0 << " " << t + 2 << " " << i << " " << finishTile << endl;
		}


	}
}
int replaceCountOnTurn = 0;
#define send(t,line,pos, hpToReplace) if(ourMoney > 60000 && our[line].hp<=hpToReplace && replaceCountOnTurn==0) {cout<<2<<" "<<line<<endl; replaceCountOnTurn++;}cout<<0<<" "<<t<<" "<<line<<" "<<pos<<endl <<1<<" "<<line<<" "<<pos<<endl
void veryHard(){
	ofstream cout("output.txt");
	int gunPos = 4, mainPos = 7;
	if (memory[2] == 0)
		memory[2] = 1;
	bool carefulTactic = memory[2] == 1;
	bool agressiveTactic = memory[2] == 2 || memory[2] == 4;
	bool waitBeforeAgressive = memory[2] == 3;
	if (carefulTactic){
		//can take
		fi(i, 3, N){
			if (memory[50 + i] == 1){
				if (enemy[i].state <= 2)
				{
					send(1, i, stones[i], 60);
					if (our[i].state == 0)
						ourMoney -= 15000;
				}
				memory[50 + i] = 0;
			}
			if (stones[i] == 7 && enemy[i].state <= 2) memory[50 + i] = 1;
		}
		fi(i, 3, N){
			if (stones[i] != 0 && our[i].state <= 2 && enemy[i].state != 3 && stones[i] <= 6 || (enemy[i].state == 4 && stones[i] <= 7))
			{
				send(1, i, stones[i], 60);
				if (our[i].state == 0)
					ourMoney -= 15000;
			}
		}

		int needRepairCount = 0; fi(i, 0, 3){ if (our[i].state == 2 && our[i].hp < 40) needRepairCount++; }
		int stoneCount = 0, enemyStarted = 0;
		fi(i, 0, 5) {
			if (stones[i] != 0 && stones[i] <= 7) stoneCount++;
			if (enemy[i].state == 3 && enemy[i].x >= 9) enemyStarted++;
		}

		int rushStarted = memory[1];
		bool rushBegins = false; bool rushEnded = false;
		if (rushStarted > 0){
			if (rushStarted == 3){
				send(1, 4, 7, 100);
			}
			if (rushStarted == 2){
				//int tmpGunPos = gunPos;
				//if (stones[0] <= 7 && stones[1] <= 7 && stones[2] <= 7)
				//	gunPos = 7;
				send(4, 0, gunPos, -1);
				send(4, 1, gunPos, -1);
				send(4, 2, gunPos, -1);
				//gunPos = tmpGunPos;
			}
			else if (rushStarted > 3){
				int gunsInField = 0, allInField = 0;
				fi(i, 0, 6) { if (our[i].x > 0) { allInField++; if (our[i].t == 4) gunsInField++; } }
				if (allInField == 0)
					rushEnded = true;
				else{
					int enemyInCenter = 0; fi(i, 0, 6){ if (enemy[i].x <= 8) enemyInCenter++; }
					bool needMoreGun = enemyStarted > 0 && stoneCount > 0 && enemyInCenter < allInField;
					if ((our[3].t == 4 || our[3].x >= 3) &&
						(our[4].t == 4 || our[4].x >= 3) &&
						(our[5].t == 4 || our[5].x >= 3))
					{
						needMoreGun = false;
					}
					if (needMoreGun)
					{
						send(4, 0, gunPos, (ourMoney > 100000 ? 20 : -1));
						send(4, 1, gunPos, (ourMoney > 100000 ? 20 : -1));
						send(4, 2, gunPos, (ourMoney > 100000 ? 20 : -1));
					}
					bool needHelp = gunsInField > 2;
					if (needHelp || needMoreGun){
						send(1, 3, mainPos, 70);
						if (our[5].state != 4 && our[5].x < 2){
							send(3, 4, mainPos, 70);
						}
						else {
							send(1, 4, mainPos, 70);
						}
						send(1, 5, mainPos, 100);
					}
				}
			}
		}
		else{
			if (needRepairCount == 0){

				int needMoney = 0;
				fi(i, 0, 6){
					if (i <= 2 && our[i].state == 0) needMoney += 25000;
					else if (i >= 3 && our[i].hp <= 100) needMoney += 15000;
				}
				bool enoughMoney = ourMoney >= needMoney;
				if (stoneCount > 0 && enemyStarted > 0 && enoughMoney){
					rushBegins = true;
					send(1, 3, mainPos, 100);
					send(1, 5, mainPos, 100);
				}
			}
		}

		if (rushBegins || memory[1] > 0)
			memory[1]++;
		if (rushEnded)
			memory[1] = 0;

		int needSave = 0;
		fi(i, 0, N){ if (our[i].state == 0) needSave += 25000; }
		if (needSave > 100000) needSave = 100000;
		if (turn > 90 && turn < 180){
			if (ourScore - 2 <= enemyScore)
				memory[2] = ourMoney >= needSave ? 2 : 3; //agressive
		}
	}
	else if (waitBeforeAgressive){
		int needSave = 0;
		fi(i, 0, N){ if (our[i].state == 0) needSave += 25000; }
		if (needSave > 100000) needSave = 100000;
		if (ourMoney >= needSave - 5000) memory[2] = 2;
	}
	else if (agressiveTactic){
		if (memory[2] == 2){
			memory[2] = 4;

			fi(i, 0, N){
				if (i != 0 && i != N - 1 && (our[i - 1].t == 1 || our[i + 1].t == 1)){
					send(4, i, 3, -1);
				}
			}
		}
		int hpToReplace[N]; fi(i, 0, N){ hpToReplace[i] = our[i].t == 1 ? 160 : (our[i].t == 4 ? 20 : 60); }
		int tcount[6]; fi(i, 0, 6) tcount[i] = 0;
		fi(i, 0, N){ if (our[i].t > 0) tcount[our[i].t]++; }
		bool minesEnough = tcount[5] == 2;
		bool gunsEnough = tcount[4] == 2;
		bool combinesEnough = tcount[1] == 2;
		//mines
		/*if (!minesEnough){
			fi(aim, 2, 6){
				int enemiesCount = 0, coverCount = 0;
				fi(line, aim - 2, aim + 3){
					if (enemy[line].t == 3 && enemy[line].x >= 9) enemiesCount++;
					if (our[line].state == 0 || our[line].state == 3 && our[line].x <= 3)
						coverCount++;
				}

				if (enemiesCount >= 2 && coverCount >= 2){
					send(2, aim - 1, 7, hpToReplace[aim]);
					send(2, aim + 1, 7, hpToReplace[aim]);
					send(5, aim, 7, hpToReplace[aim]);
					break;
				}
			}
			fi(aim, 8, 10){
				int enemiesCount = 0, coverCount = 0;
				fi(line, aim - 2, aim + 3){
					if (enemy[line].t == 3 && enemy[line].x >= 10 || enemy[line].t == 4 && enemy[line].state == 3) enemiesCount++;
					if (our[line].state == 0 || our[line].state == 3 && our[line].x <= 2)
						coverCount++;
				}

				if (enemiesCount >= 2 && coverCount >= 2){
					send(2, aim - 1, 6, hpToReplace[aim]);
					send(2, aim + 1, 6, hpToReplace[aim]);
					send(5, aim, 6, hpToReplace[aim]);
					break;
				}
			}
		}*/
		//combines
		if (!combinesEnough){
			int line = rand() % N;
			send(1, line, 7, hpToReplace[line]);
		}
		//guns
		if (!gunsEnough){
			int line = rand() % N;
			send(4, line, (stones[line]==0? 3:7), hpToReplace[line]);
		}

		//tanks
		fi(i, 0, N){
			if (stones[i] > 0 && stones[i] < 9){
				send(3, i, stones[i], hpToReplace[i]);
			}
		}
		fi(i, 0, N){
			if (!(stones[i] > 0 && stones[i] < 9)){
				send(3, i, 8, hpToReplace[i]);
			}
		}

	}
	cout << "memory" << prevType << " ";
	fi(i, 0, memorySize){
		cout << memory[i] << " ";
	}
}


void main()
{
	ifstream cin("input.txt");
	srand(time(NULL));
	cin >> turn >> ourScore >> enemyScore >> ourMoney >> enemyMoney;
	fi(i, 0, N) cin >> stones[i];
	fi(i, 0, N){ cin >> our[i].t >> our[i].state >> our[i].x >> our[i].hp; }
	fi(i, 0, N){ cin >> enemy[i].t >> enemy[i].state >> enemy[i].x >> enemy[i].hp; }
	int memoryExists;
	cin >> memoryExists;
	if (memoryExists != -1){
		prevType = memoryExists;
		fi(i, 0, memorySize){
			cin >> memory[i];
		}
	}

	int t = 0; //change it!

	if (t == 0) easy();
	else if (t == 1) normal();
	else if (t == 2) hard();
	else if (t == 3) veryHard();






}