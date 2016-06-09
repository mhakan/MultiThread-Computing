using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;
using System.Data;
using System.Collections.Concurrent;
using System.Threading;

namespace MapReduce
{
    class Program
    {
        
        //Verileri Al
        //Ara
        //Her dosyadan ayrı ayrı anahtar değer çifti oluştur
        //Anahtar değer çiftlerini birleştir

        static void Main(string[] args)
        {
            int dosyaSayisi = 3;
            string dosya = "File";
            string tamDosya;
            ArrayList al_tumIcerikler = new ArrayList();
            ArrayList[] al_icerik = new ArrayList[dosyaSayisi];
            for (int i = 0; i < dosyaSayisi; i++)
            {
                al_icerik[i] = new ArrayList();
            }
            object obj = new object();

            #region Verileri Al
            Console.WriteLine("1. ADIM: Dosya iceriklerinin alınması");
            Parallel.For(1, dosyaSayisi + 1, (i) =>
            {
                tamDosya = "../../" + dosya + i.ToString();

                al_icerik[i - 1] = DosyaOku(tamDosya);
                lock (obj)
                {
                    Console.WriteLine("\nFile{0} Icerigi:\n", i);
                    for (int j = 0; j < al_icerik[i - 1].Count; j++)
                    {
                        Console.WriteLine("{0}", al_icerik[i - 1][j]);
                    }
                }
            }
           );
            #endregion


            ConcurrentDictionary<string, int> tumElemanlar = new ConcurrentDictionary<string, int>();
            ConcurrentDictionary<string, int>[] Elemanlar = new ConcurrentDictionary<string, int>[dosyaSayisi];
            for (int i = 0; i < dosyaSayisi; i++)
            {
                Elemanlar[i] = new ConcurrentDictionary<string, int>();
            }
        

            #region Anahtar Değer Çiftleri Oluştur
            for (int i = 0; i < dosyaSayisi; i++)
            {
                //Paralel Arama 
                Elemanlar[i] = VerileriBelirle(al_icerik[i]);
                //Ekrana Yazdır
                Console.WriteLine("\n{0}. Dosya\n", i + 1);
                foreach (var item in Elemanlar[i])
                { 
                    Console.WriteLine("{0}", item); 
                }
            }
            #endregion
            

            ConcurrentDictionary<string, int> sonuclar = VerileriBirlestir(Elemanlar,dosyaSayisi);
            Console.WriteLine("\n\nSONUCLAR------\n"); 
            foreach (var item in sonuclar) 
            {
                Console.WriteLine("{0}", item); 
            }

            Console.ReadLine();
            //-------------------
        }

        private static ArrayList DosyaOku(string fileName)
        {
            ArrayList al_Satirlar = new ArrayList();
            string line;
            fileName=fileName+".txt";
            try
            {   // Open the text file using a stream reader.
                using (StreamReader sr = new StreamReader(fileName))
                {
                    while ((line = sr.ReadLine()) != null)
                    // Read the stream to a string, and write the string to the console.
                    al_Satirlar.Add(line.ToString());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
            return al_Satirlar;
        }

        private static ConcurrentDictionary<string, int> VerileriBelirle(ArrayList al_tumIcerik)
        {
            ConcurrentDictionary<string, int> Eleman = new ConcurrentDictionary<string, int>();
            object ob = new object();
            for(int k = 0; k<al_tumIcerik.Count;k++) 
            {
                string item = al_tumIcerik[k].ToString();
                int topEs=0;
                
                #region Paralel Arama işlemi
                Parallel.For(0, al_tumIcerik.Count,
                    () => 0,
                    (i, state, eslesme) =>
                    {
                        if (item == al_tumIcerik[i].ToString())
                            eslesme++;
                        return eslesme;
                    },eslesme=>{
                        Interlocked.Add(ref topEs, eslesme);
                    });
                #endregion

                Eleman.TryAdd(item, topEs);
                    topEs=0;
            }

            return Eleman;
        }

        private static ConcurrentDictionary<string, int> VerileriBirlestir(ConcurrentDictionary<string, int>[] Elemanlar, int dosyaSayi) 
        {
            ConcurrentDictionary<string, int> birlestirilmis = new ConcurrentDictionary<string,int>();
            int eslesme = 0;
            for (int i = 0; i < dosyaSayi; i++)
            {
                Parallel.ForEach(Elemanlar[i], item => 
                {
                        int top = 0, val = 0;
                        string key = "";
                        Parallel.ForEach(birlestirilmis, item2 =>
                        {
                            if (item.Key == item2.Key)
                            {
                                eslesme++;
                                top = item.Value + item2.Value;
                                val = item2.Value;
                                key = item2.Key;

                            }
                        });
                        if (eslesme == 0)
                        {
                            birlestirilmis.TryAdd(item.Key, item.Value);
                        }
                        else
                        {
                            birlestirilmis.TryUpdate(key, top, val);
                        }
                });
                
            }

            return birlestirilmis;
        }

    }
}

