using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;
using RemoverCS;

namespace BackgroundRemover.Model
{
    internal class ThreadsCS : Threads
    {
        private readonly int scope = 20;
        private Color[] pixels;

        /**
         * ThreadsCS konstruktor - Przypisauje wartość minimalną oraz 
         * maksymalna używanych wątkow oraz wywołuje konstruktor klasy bazowej
         */
        public ThreadsCS(Color uColor, int nThreads, Bitmap picture) : base(uColor, picture) 
        {
            ThreadPool.SetMaxThreads(nThreads, 1);
            ThreadPool.SetMinThreads(nThreads, 1);

        }
        
        /**
         * execute metoda - Tworzy tablice pixeli oraz przypisuje jej wartosci z bitmapy ( iteracja na tablicy jednowymiarowej )
         * Tworzy tablice zadan oraz przypisuje im zakresy obliczen
         * Zapisuje przetowrzone pixele do bitmapy
         * Zwraca przetowrzona bitmape
         */
        public override Bitmap execute()
        {
            int number_Pixels = bitPic.Width * bitPic.Height;
            int number_Divisions = 150;
            int number_Data = (number_Pixels / number_Divisions) + 1;
            
            
            pixels = new Color[number_Pixels];
            
            // petla odpowiedzialna za wpisanie pixeli z bitmapy do tablicy pixels[]
            for (int i = 0; i < number_Pixels; i++)
            {
                int y = i / bitPic.Width;
                int x = i % bitPic.Width;
                pixels[i] = bitPic.GetPixel(x, y);
            }

            //delegat na metode check_Pixels do uzywania w wyrazeniu regularnym nizej
            Action<int, int> delegater = check_Pixels;

            var tasks = new Task[number_Divisions];

            //petla odpowiedzialna za utworzenie poszczeglonych zadan oraz przypisanie im zakresu do obliczen
            for (int i = 0; i < number_Divisions; i++)
            {
                int start = i * number_Data;
                int end = start + number_Data - 1;
                if (end > number_Pixels)
                    end = number_Pixels; 
                tasks[i] = Task.Factory.StartNew(() => delegater(start, end));
            }

            Task.WaitAll(tasks);

            //petla odpowiedzialna za wpisanie przetowrzonych pixeli do wyjsciowej bitmapy
            for (int i = 0; i < number_Pixels; i++)
            {
                int y = i / bitPic.Width;
                int x = i % bitPic.Width;
                bitPic.SetPixel(x, y, pixels[i]);
            }

            return bitPic;
        }

        /**
         * check_Pixels metoda - Głowna logika aplikacji.
         * Sprawdza warunek wymazania koloru dla zadanego zakresu danych w tablixy pixeli. 
         */
        public void check_Pixels(int start, int end)
        {
            for (int i = start; i < end; i++)
            {
                pixels[i] = Remover.checkPixel(pixels[i], userColor, scope);
            }

        }
    }
}

