/*
 * LedCube.h
 *
 * Created: 19.03.2013 21:25:41
 *  Author: master
 */ 


#ifndef LEDCUBE_H_
#define LEDCUBE_H_

 char CUBE[8][8] = {{0, 0, 0, 0, 0, 0, 0, 0},
 				{0, 0, 0, 0, 0, 0, 0, 0},
 				{0, 0, 0, 0, 0, 0, 0, 0},
 				{0, 0, 0, 0, 0, 0, 0, 0},
 				{0, 0, 0, 0, 0, 0, 0, 0},
 				{0, 0, 0, 0, 0, 0, 0, 0},
 				{0, 0, 0, 0, 0, 0, 0, 0},
 				{0, 0, 0, 0, 0, 0, 0, 0}};

//char CUBE[8][8] = {{0, 0, 0, 0, 0, 0, 0, 0},
//				{0, 0, 0, 0, 0, 0, 0, 0},
//				{0, 0, 60, 60, 60, 60, 0, 0},
//				{0, 0, 60, 60, 60, 60, 0, 0},
//				{0, 0, 60, 60, 60, 60, 0, 0},
//				{0, 0, 60, 60, 60, 60, 0, 0},
//				{0, 0, 0, 0, 0, 0, 0, 0},
//				{0, 0, 0, 0, 0, 0, 0, 0}};

// char CUBE[8][8] = {{255, 128, 128, 128, 128, 128, 128, 128},
// 				{128, 0, 0, 0, 0, 0, 0, 0},
// 				{128, 0, 0, 0, 0, 0, 0, 0},
// 				{128, 0, 0, 0, 0, 0, 0, 0},
// 				{128, 0, 0, 0, 0, 0, 0, 0},
// 				{128, 0, 0, 0, 0, 0, 0, 0},
// 				{128, 0, 0, 0, 0, 0, 0, 0},
// 				{128, 0, 0, 0, 0, 0, 0, 0}};

/*char CUBE[8][8] = {{0, 0, 0, 0, 0, 0, 0, 0},
				{0, 0, 0, 0, 0, 0, 0, 0},
				{0, 0, 0, 0, 0, 0, 0, 0},
				{0, 98, 108, 112, 104, 102, 98, 0},
				{0, 98, 108, 112, 104, 102, 98, 0},
				{0, 0, 0, 0, 0, 0, 0, 0},
				{0, 0, 0, 0, 0, 0, 0, 0},
				{0, 0, 0, 0, 0, 0, 0, 0}};*/
//char CUBE[8][8] = {{255, 255, 255, 255, 255, 255, 255, 255},
// //				{255, 255, 255, 255, 255, 255, 255, 255},
// 				{255, 255, 255, 255, 255, 255, 255, 255},
// 				{255, 255, 255, 255, 255, 255, 255, 255},
// 				{255, 255, 255, 255, 255, 255, 255, 255},
// 				{255, 255, 255, 255, 255, 255, 255, 255},
// 				{255, 255, 255, 255, 255, 255, 255, 255},
// 				{255, 255, 255, 255, 255, 255, 255, 255}};

int laynum = 0;

void ledon(){
	LED |= 1<<LEDPIN;
}

void ledoff(){
	LED &= ~(1<<LEDPIN);
}

void tick(){
	CTRL |= 1<<CLK;
	CTRL &= ~(1<<CLK);
}

void latch(){
	CTRL |= 1<<LATCH;
	CTRL &= ~(1<<LATCH);
}

void layup(){
	if (LAYP != 1){
		LAY = LAYP>>1;
		//laynum++;
	}		
	else{
		LAY = 0x80;
		//laynum = 0;
	}		
}
void show(){
	for (int i=7; i>=0; i--){
		for(char j=0; j<8;j++){
			if((CUBE[laynum][i]&(1<<j)) != 0)
				CTRL |= 1<<SIN;
			else
				CTRL &= ~(1<<SIN);
			tick();
		}
	}
	latch();
	layup();
	if(laynum!=7)
		laynum++;
	else
		laynum=0;
}

void move_z(char up, char loop){
	char temp[8];
	char edge = up?7:0;
	
	for (int j=0; j<8; j++){
		temp[j] = CUBE[edge][j];
	}
	
	if(up){
		for (int i=7; i>0; i--){
			for (int j=0; j<8; j++){
				CUBE[i][j] = CUBE[i-1][j];
			}
		}
	}else{
		for (int i=0; i<7; i++){
			for (int j=0; j<8; j++){
				CUBE[i][j] = CUBE[i+1][j];
			}
		}
	}		
		
	if(loop)
		for (int j=0; j<8; j++){
			CUBE[7-edge][j] = temp[j];
		}
	else
		for (int j=0; j<8; j++){
			CUBE[7-edge][j] = 0;
		}
}

void move_y(char up, char loop){
	char temp[8];
	char edge = up?7:0;
	
	for (int j=0; j<8; j++){
		temp[j] = CUBE[j][edge];
	}
	
	if(up){
		for (int i=0; i<8; i++){
			for (int j=7; j>0; j--){
				CUBE[i][j] = CUBE[i][j-1];
			}
		}
	}else{
		for (int i=0; i<8; i++){
			for (int j=0; j<7; j++){
				CUBE[i][j] = CUBE[i][j+1];
			}
		}
	}
	
	if(loop)
	for (int j=0; j<8; j++){
		CUBE[j][7-edge] = temp[j];
	}
	else
	for (int j=0; j<8; j++){
		CUBE[j][7-edge] = 0;
	}
}

void move_x(char up, char loop){
	char temp[8];
	char edge = up?1:128;
	

	for(int i=0; i<8; i++){
		for(int j=0; j<8; j++){
			if(CUBE[i][j] & edge)
			temp[i] |= 1<<j;
			else
			temp[i] &= ~(1<<j);
		}
	}		
	
	for(int i=0; i<8; i++){
		for(int j=0; j<8; j++){
			if(up)
				CUBE[i][j] >>= 1;
			else
				CUBE[i][j] <<= 1;
		}
	}
	
	for(int i=0; i<8; i++){
		for(int j=0; j<8; j++){
			if(temp[i] & (1<<j))
				CUBE[i][j] |= 129-edge;
			else
				CUBE[i][j] &= ~(129-edge);
		}
	}
}

#endif /* LEDCUBE_H_ */