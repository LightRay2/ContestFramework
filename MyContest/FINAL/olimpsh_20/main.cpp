#include <fstream>
#include<string>
#include<map>
#include<math.h>

/*map<int,int> test_map;
	for(map<int,int>::iterator it=test_map.begin(); it!=test_map.end(); it++)
	{
		(*it).second=0;
	}*/

using namespace std;
ifstream fin("input.txt"); 
//ifstream cin2("input2.txt"); 
ofstream fout("output.txt");
bool is_zero(double a)
{
	double eps=0.000001;
	if (abs(a)<eps)
	{
		return true;
	}
	return false;
}
bool is_equal(double a,double b)
{
	double eps=0.000001;
	if (abs(a-b)<eps)
	{
		return true;
	}
	return false;
}
double dist(double x1,double y1,double x2, double y2)
{
	return sqrt((x1-x2)*(x1-x2)+(y1-y2)*(y1-y2)); //Todo check
}
class point{
public:
	double x;
	double y;
	bool exists;
	point(double o_x,double o_y)
	{
		this->x=o_x;
		this->y=o_y;
		exists=true;
	}
	point() {};
};

class segment{
public:
	double start_point_X;
	double start_point_Y;
	double end_point_X;
	double end_point_Y;
	double A,B,C; //Ax+By+C=0
	segment(double x1,double y1,double x2,double y2)
	{
		start_point_X=x1;
		start_point_Y=y1;
		end_point_X=x2;
		end_point_Y=y2;
		A=y1-y2;
		B=x2-x1;
		C=x1*y2-x2*y1;
	}
	point intersection(segment o_seg)
	{
		point res;
		//man in a bad suit
		if (is_zero(A)&&is_zero(B))
		{
			res.exists=false;
			return res;
		}
		if (is_zero(o_seg.A)&&is_zero(o_seg.B))
		{
			res.exists=false;
			return res;
		}
		//paralel,nuli
		if (is_zero(A)&&is_zero(o_seg.A))
		{
			res.exists=false;
			return res;
		}
		if (is_zero(B)&&is_zero(o_seg.B))
		{
			res.exists=false;
			return res;
		}
		//perpendicular, nuli
		if (is_zero(A)&&is_zero(o_seg.B))
		{
			res.y=-C/B;
			res.x=-o_seg.C/o_seg.A;
			if ( ((res.x>=start_point_X && res.x<=end_point_X) ||(res.x>=end_point_X && res.x<=start_point_X))&&
			((res.y>=o_seg.start_point_Y && res.y<=o_seg.end_point_Y) ||(res.y>=o_seg.end_point_Y && res.y<=o_seg.start_point_Y)) )
			{
				res.exists=true;
			}
			else
			{
				res.exists=false;
			}
			return res;
		}
		else if (is_zero(B)&&is_zero(o_seg.A))
		{
			res.x=-C/A;
			res.y=-o_seg.C/o_seg.B;
			if ( ((res.x>=o_seg.start_point_X && res.x<=o_seg.end_point_X) ||(res.x>=o_seg.end_point_X && res.x<=o_seg.start_point_X))&&
			((res.y>=start_point_Y && res.y<=end_point_Y) ||(res.y>=end_point_Y && res.y<=start_point_Y)) )
			{
				res.exists=true;
			}
			else
			{
				res.exists=false;
			}
			return res;
		} else
		{
			//ne nuli
			double a,c,b,d;
			a=-this->A/this->B;
			c=-this->C/this->B;
			b=-o_seg.A/o_seg.B;
			d=-o_seg.C/o_seg.B;
			if (is_equal(a,b))
			{
				res.exists=false;
				return res;
			}
			res.x=(d-c)/(a-b);
			res.y=(a*d-b*c)/(a-b);
		}
		if (  ((res.x>=start_point_X && res.x<=end_point_X) ||(res.x>=end_point_X && res.x<=start_point_X)) &&
		((res.x>=o_seg.start_point_X && res.x<=o_seg.end_point_X) ||(res.x>=o_seg.end_point_X && res.x<=o_seg.start_point_X)) &&
		((res.y>=start_point_Y && res.y<=end_point_Y) ||(res.y>=end_point_Y && res.y<=start_point_Y)) &&
		((res.y>=o_seg.start_point_Y && res.y<=o_seg.end_point_Y) ||(res.y>=o_seg.end_point_Y && res.y<=o_seg.start_point_Y))  )
		{
			res.exists=true;
		}
		else
		{
			res.exists=false;
		}
		return res;
	}
};

class player{
public:
	double x;
	double y;
	bool has_ball;
	bool already_has_aim;
	double aim_x;
	double aim_y;
	bool taken_by_protector;
};

int num_tic=0;
int our_score=0;
int enemy_score=0;
player our_team[5];
player enemy_team[5];
double ball_x;
double ball_y;
double ball_aim_x;
double ball_aim_y;
bool diside_to_shoot;
double shoot_x;
double shoot_y;
bool we_have_ball=false;
bool enemy_has_ball=false;
bool nobody_has_ball=false;
bool ball_is_flying;
bool ball_is_flying_enemesticly;

//todo find them
//magic_constant
double safety_move_forward=12.50;
double friend_finder=35.0;
double safety_pass=12.50;


int main()
{
	fin>>num_tic;
	fin>>our_score;
	fin>>enemy_score;
	fin>>ball_x;
	fin>>ball_y;
	fin>>ball_aim_x;
	fin>>ball_aim_y;
	for(int i=0; i<5; i++)
	{
		fin>>our_team[i].x;
		fin>>our_team[i].y;
		our_team[i].already_has_aim=false;
		if (is_equal(our_team[i].x,ball_x)&&is_equal(our_team[i].y,ball_y))
		{
			our_team[i].has_ball=true;
			we_have_ball=true;
		}
		else
		{
			our_team[i].has_ball=false;
		}
	}
	for(int i=0; i<5; i++)
	{
		fin>>enemy_team[i].x;
		fin>>enemy_team[i].y;
		if (is_equal(enemy_team[i].x,ball_x)&&is_equal(enemy_team[i].y,ball_y))
		{
			enemy_has_ball=true;
			enemy_team[i].has_ball=true;
		}
		else
		{
			enemy_team[i].has_ball=false;
		}
	}
	if ((!we_have_ball)&&(!enemy_has_ball))
	{
		if (is_equal(ball_aim_x,ball_x)&&is_equal(ball_aim_y,ball_y))
		{
			ball_is_flying=true;
		}
		else
		{
			nobody_has_ball=true;
		}
	}
	if ((nobody_has_ball)||(enemy_has_ball)) //Todo maube split
	{
		double min_dist1=100500;
		int index1=1;
		double min_dist2=100500;
		int index2=1;
		for(int i=0; i<5; i++)
		{
			double distance=dist(ball_x,ball_y,our_team[i].x,our_team[i].y);
			if (distance<min_dist1)
			{
				index2=index1;
				index1=i;
				min_dist2=min_dist1;
				min_dist1=distance;
			}
			else if (distance<min_dist2)
			{
				index2=i;
				min_dist2=distance;
			}
		}
		our_team[index1].already_has_aim=true;
		our_team[index1].aim_x=ball_x;
		our_team[index1].aim_y=ball_y;
		our_team[index2].already_has_aim=true;
		our_team[index2].aim_x=ball_x;
		our_team[index2].aim_y=ball_y;
		for(int i=0; i<5; i++)
		{
			if (our_team[i].already_has_aim) continue;
			double closest_dist=100500;
			int closest_index=1;
			for(int j=0; j<5; j++)
			{
				if (enemy_team[j].has_ball) continue;
				double distance=dist(enemy_team[j].x,enemy_team[j].y,our_team[i].x,our_team[i].y);
				if (distance<closest_dist)
				{
					closest_dist=distance;
					closest_index=j;
				}
			}
			our_team[i].already_has_aim=true;
			our_team[i].aim_x=enemy_team[closest_index].x;
			our_team[i].aim_y=enemy_team[closest_index].y;
		}
	}
	if (we_have_ball)
	{
		for(int i=0; i<5; i++)
		{
			if (our_team[i].has_ball)
			{
				//can make +1 mazefucka
				//Todo shoooting acroos defenders
				if (our_team[i].x>70)
				{
					//check right in line
					bool aah=false;
					for(int j=0; j<5; j++)
					{
						if ((abs(enemy_team[j].y-our_team[i].y)<4)&&(enemy_team[j].x>our_team[i].x))
						{
							aah=true;
						}
					}
					if (!aah)
					{
						diside_to_shoot=true;
						shoot_x=100;
						shoot_y=our_team[i].y;
						continue;
					}
				}
				if (our_team[i].x>80)
				{
					diside_to_shoot=true;
					shoot_x=100;
					if (our_team[i].y>30)
					{
						shoot_y=our_team[i].y+20;
					}
					else
					{
						shoot_y=our_team[i].y-20;
					}
					continue;
				}
				//can just_ataack
				bool can_just_atack=false;
				double min_dist=100500;
				for(int j=0; j<5; j++)
				{
					if (our_team[i].x>60) continue;//todo??
					if (enemy_team[j].x<our_team[i].x) continue;
					double distance=dist(enemy_team[j].x,enemy_team[j].y,our_team[i].x,our_team[i].y);
					if (abs(enemy_team[j].x-our_team[i].x)<distance/1.5) continue;
					if (distance<min_dist)
					{
						min_dist=distance;
					}
				}
				if (min_dist>safety_move_forward)
				{
					can_just_atack=true;
					our_team[i].already_has_aim=true;
					our_team[i].aim_x=100;
					our_team[i].aim_y=our_team[i].y;
					continue;
				}
				//try to pass
				bool can_pass=false;
				for(int j=0; j<5; j++)
				{
					if (i==j) continue;
					//Todo check oponents in the line
					bool cant_pass=false;
					segment pass_line=*new segment(our_team[i].x,our_team[i].y,our_team[j].x,our_team[j].y);
					double distance=dist(our_team[i].x,our_team[i].y,our_team[j].x,our_team[j].y);
					if (distance<30)
					{
						double min_dist=100500;
						for(int q=0; q<5; q++)
						{
							if (  ((enemy_team[q].x>=pass_line.start_point_X && enemy_team[q].x<=pass_line.end_point_X) ||(enemy_team[q].x+2>=pass_line.end_point_X && enemy_team[q].x<=pass_line.start_point_X)) &&
								((enemy_team[q].y>=pass_line.start_point_Y && enemy_team[q].y<=pass_line.end_point_Y) ||(enemy_team[q].y+2>=pass_line.end_point_Y && enemy_team[q].y<=pass_line.start_point_Y)) 
								 )
							{
								cant_pass=true;
							}
							if (  ((enemy_team[q].x+5>=pass_line.start_point_X && enemy_team[q].x-5<=pass_line.end_point_X) ||(enemy_team[q].x+10>=pass_line.end_point_X && enemy_team[q].x-5<=pass_line.start_point_X)) &&
								((enemy_team[q].y+5>=pass_line.start_point_Y && enemy_team[q].y-5<=pass_line.end_point_Y) ||(enemy_team[q].y+10>=pass_line.end_point_Y && enemy_team[q].y-5<=pass_line.start_point_Y)) &&(distance>20)
								 )
							{
								cant_pass=true;
							}
							double loc_distance=dist(our_team[j].x,our_team[j].y,enemy_team[q].x,enemy_team[q].y);
							if (loc_distance<min_dist)
							{
								min_dist=loc_distance;
							}
						}
						if (cant_pass)
						{
							continue;
						}
						if (min_dist>safety_pass)
						{
							can_pass=true;
							diside_to_shoot=true;
							shoot_x=our_team[j].x;
							shoot_y=our_team[j].y;
							break;
						}
					}
				}
				//todo wrote normal
				//try to obmotka
				if (!can_pass)
				{
					our_team[i].already_has_aim=true;
					our_team[i].aim_x=100;
					if (our_team[i].y>30)
					{
						our_team[i].aim_y=our_team[i].y+25;
					}
					else
					{
						our_team[i].aim_y=our_team[i].y-25;
					}
					continue;
				}
			} else {
				//without_ball
				//if has friend nearly
				bool friend_is_here=false;
				bool too_far=false;
				if (ball_x+30<our_team[i].x)
				{
					too_far=true;
				}
				for(int j=0; j<5; j++)
				{
					if (i==j) continue;
					double distance=dist(our_team[j].x,our_team[j].y,our_team[i].x,our_team[i].y);
					if (distance<friend_finder)
					{
						friend_is_here=true;
					}
				}
				//go to closest alience
				if (!friend_is_here)
				{
					double min_dist=100500;
					int index1=1;
					for(int j=0; j<5; j++)
					{
						double loc_distance=dist(our_team[j].x,our_team[j].y,our_team[i].x,our_team[i].y);
						if (loc_distance<min_dist)
						{
							min_dist=loc_distance;
							index1=j;
						}
					}
					our_team[i].already_has_aim=true;
					our_team[i].aim_x=our_team[index1].x;
					our_team[i].aim_y=our_team[index1].y;
				}
				else
				{
					//go to ball
					if (too_far)
					{
						our_team[i].already_has_aim=true;
						our_team[i].aim_x=ball_x;
						our_team[i].aim_y=ball_y;
					}
					//move forward
					else
					{
						our_team[i].already_has_aim=true;
						our_team[i].aim_x=100;
						our_team[i].aim_y=our_team[i].y;
					}
				}
				//two brovochnika))
				//for(int 
			}
		}
	}
	if (ball_is_flying) //Todo stop only target itp
	{
		for(int i=0; i<5; i++)
		{
			our_team[i].already_has_aim=true;
			our_team[i].aim_x=our_team[i].x;
			our_team[i].aim_y=our_team[i].y;
		}
	}
	//output;
	for(int i=0; i<5; i++)
	{
		if (our_team[i].already_has_aim)
		{
			fout<<our_team[i].aim_x<<" "<<our_team[i].aim_y<<endl;
		}
		else
		{
			fout<<our_team[i].x<<" "<<our_team[i].y<<endl;
		}
		if (diside_to_shoot)
		{
			fout<<shoot_x<<" "<<shoot_y<<endl;
		}
	}
	return 0;
}