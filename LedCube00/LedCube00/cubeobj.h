#include "ledcube.h"

typedef struct{
	char x_;
	char y_;
	char z_;
	char yL_;
	char zL_;
	char* obj_;
} CubeObj;

typedef struct{
	CubeObj* list_[3];
	char N;
} Cube;

Cube CUBE;

void addCubeObj(char x, char y, char z, int yL, int zL, char* obj){
	CubeObj *cubeObj = malloc(sizeof(CubeObj));
	
	cubeObj->obj_ = malloc(yL*zL*sizeof(char));

	cubeObj->x_ = x;
	cubeObj->y_ = y;
	cubeObj->z_ = z;
	cubeObj->yL_ = yL;
	cubeObj->zL_ = zL;
	
	for(char i=0; i<zL*yL; i++){
			cubeObj->obj_[i] = (char)(obj[i])<<x;			
	}
	
	//CUBE.list_[0] = cubeObj;
	CUBE.list_[CUBE.N] = cubeObj;
	CUBE.N++;
}

void pushByte(char pByte){
	for(char i=0; i<8; i++){
		if((pByte&(1<<i)) != 0)
			CTRL |= 1<<SIN;
		else
			CTRL &= ~(1<<SIN);
		tick();
	}
}

void showCubeObj(){
	

	CubeObj* cubeObj;
	char t;
	char pByte;
	
	for (int y=0; y<8; y++){
		pByte = 0;
		for(int n=0; n< CUBE.N; n++){
			cubeObj = CUBE.list_[n];
				
			if(y>=cubeObj->y_ && y<cubeObj->y_+cubeObj->yL_)
				if(laynum>=cubeObj->z_ && laynum<cubeObj->z_+cubeObj->zL_)
					pByte |= (char)cubeObj->obj_[(laynum-cubeObj->z_)*(cubeObj->yL_)+y-cubeObj->y_];
		}			
		pushByte(pByte);
		
	}		
	latch();
	layup();
	if(laynum!=7)
	laynum++;
	else
	laynum=0;
	
	
	/*
	for(char z=cubeObj->z_; z<cubeObj->zL_; z++){
		for (char y=cubeObj->y_; y<cubeObj->yL_; y++){
			CUBE[z][y] |= (char)cubeObj->obj_[z*(cubeObj->yL_)+y];
		}
	}	*/		
}

void moveCubeObj(CubeObj* cubeObj, int x, int y, int z){
	if(x>0)
		for (char i=0; i<cubeObj->yL_*cubeObj->zL_; i++)
			cubeObj->obj_[i] = cubeObj->obj_[i]<<x;		
	if(x<0)
	for (int i=0; i<cubeObj->yL_*cubeObj->zL_; i++)
		cubeObj->obj_[i] = cubeObj->obj_[i]>>(-1*x);
		
	cubeObj->x_ += x;
	cubeObj->y_ += y;
	cubeObj->z_ += z;
}