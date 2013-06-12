/*
 * LedCube00.c
 *
 * Created: 15.03.2013 21:29:23
 *  Author: master
 */ 
#define F_CPU 8000000UL

#define LAY PORTA
#define LAYD DDRA
#define LAYP PINA
#define CTRL PORTB
#define CTRLD DDRB
#define SIN PINB0
#define CLK PB1
#define LATCH PB2
#define LEDD DDRD
#define LED PORTD
#define LEDPIN PD6

#include <avr/io.h>
#include <util/delay.h>
#include <avr/interrupt.h>
#include <stdlib.h>
#include "ledcube.h"
#include "uart.h"
#include <avr/wdt.h>

//ISR(USART_RXC_vect)
//{
	
//}

ISR( TIMER0_OVF_vect )
{
	show();
	//showCubeObj();
}


int main(void)
{
		LEDD |= 64;
		
		LAYD = 0xFF;
		LAY = 0x1;
		
		CTRLD |= 7;
		CTRL &= ~7;
		
		TCCR0 = (0<<CS02)|(1<<CS01)|(1<<CS00);
		TIMSK |= (1<<TOIE0);
		sei();
		
		
		/*
		char c[2][2] = {{3, 3}, {3, 3}};
		char p[2][3] = {{7, 7, 7}, {0, 2, 0}};
		char l[3][1] = {{1}, {1}, {1}};
		
		
		addCubeObj(1, 1, 4, 2, 2, c);
		addCubeObj(3, 2, 2, 3, 2, p);
		addCubeObj(5, 6, 1, 1, 3, l);
		
		int c_z = 1;
		int c_x = 1;
		int p_y = 1;
		int p_x = 1;
		int l_z = 1;
		
		char nm = 0;
		*/
		
		uart_init();
		//UCSRB |= (1 << RXCIE);
		
		char buf;
		
		while (1){
			buf = uart_getdata();
			switch(buf){
				case 1:
					wdt_enable(0);
				break;
				
				case 2:
					for(int i=0; i<8; i++){
						for(int j=0; j<8; j++){
							buf = uart_getdata();
							CUBE[i][j] = buf;
						}
					}
				break;
				
				case 3:
					for (int i=0; i<8; i++)
						for(int j =0; j<8; j++)
							CUBE[i][j] = 0;
				break;
			}
			
		}			
		/*_delay_ms(500);
			
			for (int y=0; y<8; y++)
				pushByte(rand() / (RAND_MAX / 255 + 1));
			latch();
			layup();
			if(laynum!=7)
			laynum++;
			else
			laynum=0;
			*/
			
			/*
			nm++;
			_delay_ms(50);
			if(CUBE.list_[0]->z_ == 6 || CUBE.list_[0]->z_ == 0)
				c_z *= -1;
			if(CUBE.list_[0]->x_ == 6 || CUBE.list_[0]->x_ == 0)
				c_x *= -1;
			moveCubeObj(CUBE.list_[0], c_x, 0, c_z);
			
			if(nm == 1)
				continue;
							
			if(CUBE.list_[1]->y_ == 5 || CUBE.list_[1]->y_ == 0)
				p_y *= -1;
			if(CUBE.list_[1]->x_ == 5 || CUBE.list_[1]->x_ == 0)
				p_x *= -1;
			moveCubeObj(CUBE.list_[1], p_x, p_y, 0);
			
			if(nm == 2)
				continue;
			nm = 0;
			
			if(CUBE.list_[2]->z_ == 5 || CUBE.list_[2]->z_ == 0)
			l_z *= -1;
			moveCubeObj(CUBE.list_[2], 0, 0, l_z);*/
		//}
		
		
}