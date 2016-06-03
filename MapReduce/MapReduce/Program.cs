using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;
using System.Data;

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

            #region Verileri Al
            for (int i = 1; i < dosyaSayisi+1; i++)
            {
                tamDosya = "../../" + dosya + i.ToString();
                al_icerik[i-1] = DosyaOku(tamDosya);
                
                Console.WriteLine("\nFile{0} Icerigi:\n",i);
                Console.Read();
                for (int j = 0; j < al_icerik[i-1].Count; j++)
                {
                    Console.WriteLine("{0}",al_icerik[i-1][j]);
                }
            }
            #endregion
            
            DataTable []dt = new DataTable[dosyaSayisi];
            for (int i = 0; i < dosyaSayisi; i++)
            {
                dt[i] = new DataTable();
            }
            #region Anahtar Değer Çiftleri Oluştur
            for (int i = 0; i < dosyaSayisi; i++)
            {
                dt[i] = VerileriBelirle(al_icerik[i]);
            }
            #endregion


            //Reduce: Anahtar değer çiftlerini oluştur ve birleştir
            
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

        private static DataTable VerileriBelirle(ArrayList al_tumIcerik)
        {
            DataTable veriTablosu = new DataTable();
            veriTablosu.Columns.Add("VeriAdi",typeof(string));
            veriTablosu.Columns.Add("VeriSayisi", typeof(int));

            for(int k = 0; k<al_tumIcerik.Count;k++) 
            {
                int eslesme = 0;
                int sayi = 1;
                int veriBoyu = veriTablosu.Rows.Count;
                string item = al_tumIcerik[k].ToString();

                for (int i = 0; i < veriBoyu; i++)
                {
                    string str = veriTablosu.Rows[i]["VeriAdi"].ToString();
                    if (str == item.ToString())
                    {
                        eslesme += 1;
                        sayi = Convert.ToInt32(veriTablosu.Rows[i]["VeriSayisi"]);
                        sayi = sayi + 1;
                        veriTablosu.Rows[i]["VeriSayisi"] = sayi;
                    }   
                }

                if (eslesme == 0)
                {
                    veriTablosu.Rows.Add(item, sayi);
                }
               
            }
            return veriTablosu;
        }


    }
}

