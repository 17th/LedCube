/***************************************************************************************************************************************************
TARGET SYSTEM: AVR-Atmega16 @ 16Mhz
COMPILER: avr-gcc(Winavr)
***********************************************************************************/
#ifndef _UART_H_
#define _UART_H_
#include<avr/io.h>
#include<avr/pgmspace.h>
#define UCSRA _SFR_IO8(0x0b)
#define UCSRB _SFR_IO8(0x0a)
#define UCSRC _SFR_IO8(0x20)
#define UDR _SFR_IO8(0x0c)
#define UBRRL _SFR_IO8(0x09)
#define UBRRH _SFR_IO8(0x20)
#define TX_NEWLINE {uart_putdata(0x0d); uart_putdata(0x0a);}

void uart_init()     //To initialize UART
{
	UCSRB=0x18; // enabling the RXEN and TXEN bit to enable the receiver and transmitter
	UCSRC=0x86; // Writing one to URSEL bit to read UCSRC/ UCSZ1 and OCSZ2 both are 1 to select 8bit
	UBRRH=0x00;
	UBRRL=0x33; // baud rate=9600
	
}

void uart_putdata(unsigned char data) //To Transmit a single byte of Data
{ 
	while((UCSRA&0x20)==0x00);
	UDR=data;
}

unsigned char uart_getdata(void) //To Receive a Byte of Data
{
	while((UCSRA&0x80)==0x00);
	return UDR;
}
  
void uart_num(unsigned long int num) //To Transmit integer
{
	unsigned char r=0,j=0,y=0,a[10];
	
	while(num>0)
	{
		r=num%10;
		a[j]=r;
		j++;
		num=num/10;
	}
	for(y=j-1;y!=255;y--)
		uart_putdata(a[y]+48);
}

uart_string(unsigned char str[]) //To Transmit a String 
{
	unsigned char i=0;
	for(i=0;str[i]!='\0';i++)
	uart_putdata(str[i]);
}

void string_pgmspace(char* string) //To Transmit a String which is in Programm Space
{
	while (pgm_read_byte(&(*string)))
	uart_putdata(pgm_read_byte(&(*string++)));
}
 #endif
