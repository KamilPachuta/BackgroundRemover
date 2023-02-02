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
        public override (Bitmap, double) execute()
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

            //pobiera dokładną datę do pomiaru czasu
            DateTime begin = DateTime.Now;

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

            //oblicza czas wykonywania asynchronicznych zadan
            TimeSpan time = DateTime.Now - begin;

            //petla odpowiedzialna za wpisanie przetowrzonych pixeli do wyjsciowej bitmapy
            for (int i = 0; i < number_Pixels; i++)
            {
                int y = i / bitPic.Width;
                int x = i % bitPic.Width;
                bitPic.SetPixel(x, y, pixels[i]);
            }

            return (bitPic, time.TotalMilliseconds);
        }

        /**
         * check_Pixels metoda - Głowna logika aplikacji.
         * Sprawdza warunek wymazania koloru dla zadanego zakresu danych w tablixy pixeli. 
         */
        public void check_Pixels(int start, int end)
        {
            int iterator = 0;
            int[] fourPixels = new int[4];
          
            for (int i = start; i < end; i++)
            {
                fourPixels[iterator] = pixels[i].ToArgb();

                if (++iterator == 4 || i == end)
                {
                    int[] color = new int[4];
                    for (int x = 0; x < 4; x++)
                    {
                        color[x] = userColor.ToArgb();
                    }
                    Remover.checkPixels(fourPixels, color); // scope wrzucic bezposrednio dom asma
                    iterator = 0;

                    pixels[i - 3] = Color.FromArgb(fourPixels[0]);
                    pixels[i - 2] = Color.FromArgb(fourPixels[1]);
                    pixels[i - 1] = Color.FromArgb(fourPixels[2]);
                    pixels[i] = Color.FromArgb(fourPixels[3]);


                }
            }

        }
    }
}

