// OpenMpDene1.cpp : Defines the entry point for the console application.
//
#include "stdafx.h"
#include <cstdlib>
#include <iostream>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
// posix thread kütüphanesi
#include <pthread.h>
#define THREAD_ADET 4
using namespace std;
void *thread_routine(void* deger){
	printf("----- Cocuk Thread %d Faktoriyeli ----- \n", deger);
	int sayi = (int)((int *)deger);
	int toplam = 1;
	int i;
	for (i = 1; i <= sayi; i++)
	{
		toplam *= i;
	}
	return (void*)(toplam);
}
int _tmain(int argc, _TCHAR* argv[])
{
	pthread_t thread_id[4];
	int *sayi = (int *)3;
	void *thread_result = 0; 
	int dizi[] = {1, 2, 3, 4};
	
	int i, length=4;
	int Toplam=0;
	for (i = 0; i < length; i++)
	{
		int *sayi = (int *)dizi[i];
		pthread_create(&thread_id[i], NULL, thread_routine, (void *)sayi);
		printf("Ana Thread\n");
	}
	for ( i = 3; i > -1; i--)
	{
		pthread_join(thread_id[i], &thread_result);
		if (thread_result != 0)
			printf("Cocuk Thread 'den gelen sonuc:  %d\n",
			thread_result);
		Toplam = Toplam + (int)thread_result;
	}
	printf("\nSONUC TOPLAM = %d\n", Toplam);
	
	system("PAUSE");
}