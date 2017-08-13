#define _CRT_SECURE_NO_WARNINGS
#include <iostream>
#include <algorithm>
#include <cmath>
#include <vector>
#include <fstream>

using namespace std;

vector<pair<double, double>> pl1, pl2;
pair<double, double> bol1, bol2;
int n, g1, g2;

double dist(pair<double, double> a, pair<double, double> b)
{
	return (sqrt(pow(a.first - b.first, 2) + pow(a.second - b.second, 2)));
}

bool sort_Y(pair<double, double> a, pair<double, double> b)
{
	return(a.second < b.second);
}

int num_line(int k)
{
	vector<pair<double, double>> line;
	line = pl1;
	sort(line.begin() + 1, line.end(), sort_Y);
	for (int i = 1; i < 5; i++)
		if (pl1[k].second == line[i].second)
			return (i + 1);
}

void print_ud(int k)
{
	for (int i = 1; i < 5; i++)
	{
		cout << 70 << " " << num_line(i) * 12 - 12 << endl;
	}
	cout << pl1[k].first + 50 << " " << pl1[k].second;
}

void pus_mid(int k)
{
	vector<int> lineP;
	int inv[6];
	lineP.push_back(0);
	for (int i = 1; i < 5; i++)
	{
		lineP.push_back(num_line(i));
		inv[num_line(i)] = i;
	}


	for (int i = 1; i < 5; i++)
		cout << pl1[i].first + 4 << " " << num_line(i) * 12 - 12 << endl;

	if (lineP[k] != 2)
	{
		if (lineP[k] != 5)
		{
			double d1, d2;
			int u, d;
			u = inv[lineP[k] - 1];
			d = inv[lineP[k] + 1];
			d1 = 500;
			d2 = 500;
			for (int i = 0; i < 5; i++)
			{
				d1 = min(d1, dist(pl1[inv[lineP[k] - 1]], pl2[i]));
				d2 = min(d2, dist(pl1[inv[lineP[k] + 1]], pl2[i]));
			}
			if (d1 < d2)
				cout << pl1[inv[lineP[k] + 1]].first + 4 << " " << num_line(inv[lineP[k] + 1]) * 12 - 12 << endl;
			else
				cout << pl1[inv[lineP[k] - 1]].first + 4 << " " << num_line(inv[lineP[k] - 1]) * 12 - 12 << endl;
			cout << "memory 2" << endl;
		}
		else
		{
			double d1, d2;
			int u, d;
			u = inv[lineP[k] - 1];
			d = inv[lineP[k] + 1];
			d1 = 500;
			d2 = 500;
			for (int i = 0; i < 5; i++)
			{
				d1 = min(d1, dist(pl1[inv[lineP[k] - 1]], pl2[i]));
				d2 = min(d2, dist(pl1[inv[lineP[k] - 2]], pl2[i]));
			}
			if (d1 < d2)
				cout << pl1[inv[lineP[k] - 2]].first + 4 << " " << num_line(inv[lineP[k] - 2]) * 12 - 12 << endl;
			else
				cout << pl1[inv[lineP[k] - 1]].first + 4 << " " << num_line(inv[lineP[k] - 1]) * 12 - 12 << endl;
			cout << "memory 2" << endl;
		}
	}
	else
	{
		{
			double d1, d2;
			int u, d;
			u = inv[lineP[k] - 1];
			d = inv[lineP[k] + 1];
			d1 = 500;
			d2 = 500;
			for (int i = 0; i < 5; i++)
			{
				d1 = min(d1, dist(pl1[inv[lineP[k] + 2]], pl2[i]));
				d2 = min(d2, dist(pl1[inv[lineP[k] + 1]], pl2[i]));
			}
			if (min(d1, d2) > 6)
			{
				if (d1 < d2)
					cout << pl1[inv[lineP[k] + 1]].first + 4 << " " << num_line(inv[lineP[k] + 1]) * 12 - 12 << endl;
				else
					cout << pl1[inv[lineP[k] + 2]].first + 4 << " " << num_line(inv[lineP[k] + 2]) * 12 - 12 << endl;
				cout << "memory 2" << endl;
			}
		}
	}
}

void go_line(int k)
{
	for (int i = 1; i < 5; i++)
	{
		cout << pl1[k].first + 5 << " " << num_line(i) * 12 - 12 << endl;
	}
}

void go_bol(int k)
{
	vector<int> lineP;
	lineP.push_back(0);
	for (int i = 1; i < 5; i++)
		lineP.push_back(num_line(i));
	for (int i = 1; i < 5; i++)
	{
		if (i != k)
			cout << bol2.first << " " << bol2.second << endl;
		else
			cout << bol2.first << " " << bol2.second << endl;
	}
}

void bol_mid(int k)
{
	vector<int> lineP;
	int inv[6];
	lineP.push_back(0);
	for (int i = 1; i < 5; i++)
	{
		lineP.push_back(num_line(i));
		inv[num_line(i)] = i;
	}
	for (int i = 1; i < 5; i++)
	{
		cout << 100 << " " << num_line(i) * 12 - 12 << endl;
	}
	if (lineP[k] < 3)
	{
		double dbol = 500;
		for (int i = 0; i < 5; i++)
			dbol = min(dbol, dist(pl1[inv[lineP[k] + 1]], pl2[i]));
		if (dbol > 12)
		{
			cout << pl1[inv[lineP[k] + 1]].first + 4 << " " << pl1[inv[lineP[k] + 1]].second << endl;
			cout << "memory 2" << endl;
		}

	}
	else
		if (lineP[k] > 3)
		{
			double dbol = 500;
			for (int i = 0; i < 5; i++)
				dbol = min(dbol, dist(pl1[inv[lineP[k] - 1]], pl2[i]));
			if (dbol > 12)
			{
				cout << pl1[inv[lineP[k] - 1]].first + 4 << " " << pl1[inv[lineP[k] - 1]].second << endl;
				cout << "memory 2" << endl;
			}

		}
		else
			if (lineP[k] == 2)
			{
				cout << pl1[k].first - 10 << " " << pl1[k].second + 20;
			}
			else
				if (lineP[k] == 5)
					cout << pl1[k].first - 10 << " " << pl1[k].second - 20;
}

int main()
{
	ifstream cin;
	freopen("output.txt", "w", stdout);

	cin.open("input.txt");

	cin >> n >> g1 >> g2;

	cin >> bol1.first >> bol1.second;
	cin >> bol2.first >> bol2.second;

	for (int i = 0; i < 5; i++)
	{
		pair<double, double> t;
		cin >> t.first >> t.second;
		pl1.push_back(t);
	}

	for (int i = 0; i < 5; i++)
	{
		pair<double, double> t;
		cin >> t.first >> t.second;
		pl2.push_back(t);
	}

	int m;
	cin >> m;

	if (m == 2)
	{
		for (int i = 0; i < 5; i++)
		{
			cout << 100 << " " << num_line(i) * 10 << endl;
		}
		return (0);
	}
	else
		if (m == 1)
		{
			for (int i = 0; i < 5; i++)
			{
			cout << 100 << " " << num_line(i) * 10 << endl;
			}
			return (0);
		}
	
	if (bol1 == pl1[0])
	{
		double d = 0;
		int k = 0;
		for (int i = 1; i < 5; i++)
		{
			double dt = 5000;
			for (int j = 0; j < 5; j++)
			{
				dt = min(dt, dist(pl1[i], pl2[j]));
			}
			if (dt > d)
			{
				d = dt;
				k = i;
			}
		}
		for (int i = 0; i < 5; i++)
		{
			cout << pl1[i].first << " " << pl1[i].second << endl;
		}
		cout << pl1[k].first << " " << pl1[k].second << endl;
		return (0);
	}
	else
	{
		if (bol2.first > 30)
		{
			cout << 30 << " " << pl1[0].second + (bol2.second - pl1[0].second) * 10 << endl;
		}
		else
		{
			cout << bol2.first << " " << pl1[0].second + (bol2.second - pl1[0].second) * 10 << endl;
		}
	}

	bool fb = false;
	int how_bol = -1;
	for (int i = 1; i < 5; i++)
	{
		if (bol1 == pl1[i])
		{
			fb = true;
			how_bol = i;
		}
	}
	if (fb)
	{
		if (pl1[how_bol].first > 72)
		{
			print_ud(how_bol);
			return (0);
		}
		else
		{
			bool fff = true;
			for (int i = 0; i < 5; i++)
				if (pl1[how_bol].first < pl2[i].first)
					fff = false;
			if (fff)
			{
				for (int i = 1; i < 5; i++)
				{
					cout << 70 << " " << pl1[i].second << endl;
				}
				cout << pl1[how_bol].first + 50 << " " << pl1[how_bol].second << endl;
				return (0);
			}
			double dbol = 500;
			for (int i = 1; i < 5; i++)
				dbol = min(dbol, dist(pl1[how_bol], pl2[i]));
			if (dbol < 10)
			{
				pus_mid(how_bol);
				return (0);
			}
			else
				if (dbol > 20)
				{
					bol_mid(how_bol);
				}
				else
				{
					go_line(how_bol);
					return (0);
				}
		}
	}
	else
	{
		double dbol = dist(pl1[1], bol2);
		how_bol = 1;
		for (int i = 2; i < 5; i++)
			if (dbol > dist(pl1[i], bol2))
			{
				dbol = dist(pl1[i], bol2);
				how_bol = i;
			}
		go_bol(how_bol);
		return (0);
	}
	return (0);
}