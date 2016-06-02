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
        
        static void Main(string[] args)
        {
            int dosyaSayisi = 3;
            string dosya = "File";
            string tamDosya;
            ArrayList al_tumIcerikler = new ArrayList();
            for (int i = 1; i < dosyaSayisi+1; i++)
            {
                tamDosya = "../../" + dosya;
                tamDosya = tamDosya + i.ToString();
                ArrayList al_f = DosyaOku(tamDosya);
                Console.WriteLine("\nFile{0} Icerigi:\n",i);
                foreach (var item in al_f)
                {
                    Console.WriteLine(item.ToString());
                    al_tumIcerikler.Add(item);
                }
                Console.Read();
            }
            
            
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
            foreach (var item in al_tumIcerik)
            {
                int veriBoyu = veriTablosu.Rows.Count; 
                for (int i = 0; i < veriBoyu; i++)
                {
                    if (veriTablosu.Rows["VeriAdi"][i].ToString() == item.ToString())
                    {
                        
                    }
                }
                if (true)
                {
                    
                }
            }
            return veriTablosu;
        }
    }
}

