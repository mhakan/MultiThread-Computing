

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

pthread_mutex_t mutex1 = PTHREAD_MUTEX_INITIALIZER;
pthread_mutex_t mutex2 = PTHREAD_MUTEX_INITIALIZER;
int  counter = 0;

int TUM_TOPLAM = 0;

int FaktoriyelAl(int deger){

	
	int sayi = (int)((int *)deger);
	int toplam = 1;
	int i;
	for (i = 1; i <= sayi; i++)
	{
		toplam *= i;
	}

	printf("----- %d Faktoriyeli = %d ----- \n", deger, toplam);
	
	return toplam;
}

void *thread_routine(void* deger){
	
	int sayi = (int)((int *)deger);
	int faktSonuc = FaktoriyelAl(sayi);
	sayi = sayi + 1;
	int sonuc = sayi*faktSonuc;
	pthread_mutex_lock(&mutex1);
	TUM_TOPLAM = TUM_TOPLAM + sonuc;
	printf("*** Cocuk Thread %d Sonucu (k!(k+1)) = %d ***\n\n#### TUM TOPLAM  = %d oldu #### \n\n", deger, sonuc, TUM_TOPLAM);
	pthread_mutex_unlock(&mutex1);
	return (void*)(sonuc);
}

int _tmain(int argc, _TCHAR* argv[])
{

	printf("Ana Threaddeyim\n");
	void *thread_result = 0;
	int k, N = 4;
	int bas = 1;
	printf("Baslangic degerini seciniz.\n");
	scanf_s("%d", &N);

	printf("Bitis (N) degerini seciniz\n");
	scanf_s("%d", &N);

	pthread_t *thread_id;
	thread_id = new pthread_t[N];
	int Toplam = 0;
	
	printf("\n%d tane Thread olusturuluyor...\n\n",N);
	for (k = bas; k <= N; k++)
	{
		pthread_create(&thread_id[k], NULL, thread_routine, (void *)k);
		printf("%d Nolu Thread olusturuldu-------------\n", k);
	}
	

	system("PAUSE");
}



