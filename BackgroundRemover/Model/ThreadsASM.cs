using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace BackgroundRemover.Model
{
    internal class ThreadsASM : Threads
    {

        private readonly int scope = 0;
        private int[] pixels;

        [DllImport(@"C:\Users\Hiroshi\Desktop\TODAY\BackgroundRemover\x64\Debug\BackgroundRemoverAsm.dll")]
        private static extern void MyProc1(int[] pixels, int[] color);
        /**
         * ThreadsCS konstruktor - Przypisauje wartość minimalną oraz 
         * maksymalna używanych wątkow oraz wywołuje konstruktor klasy bazowej
         */
        public ThreadsASM(Color uColor, int nThreads, Bitmap picture) : base(uColor, picture)
        {
            ThreadPool.SetMaxThreads(nThreads, 1);
            ThreadPool.SetMinThreads(nThreads, 1);

        }

        /**
         * execute metoda - 
         * Tworzy tablice pixeli oraz przypisuje jej wartosci z bitmapy ( iteracja na tablicy jednowymiarowej )
         * Tworzy tablice zadan oraz przypisuje im zakresy obliczen
         * Zapisuje przetowrzone pixele do bitmapy
         * Zwraca przetowrzona bitmape
         */
        public override (Bitmap, double) execute()
        {
            int number_Pixels = bitPic.Width * bitPic.Height;
            int number_Divisions = 150;
            int number_Data = (number_Pixels / number_Divisions) + 1;


            pixels = new int[number_Pixels];

            // petla odpowiedzialna za wpisanie pixeli z bitmapy do tablicy pixels[]
            for (int i = 0; i < number_Pixels; i++)
            {
                int y = i / bitPic.Width;
                int x = i % bitPic.Width;
                pixels[i] = bitPic.GetPixel(x, y).ToArgb();
            }

            //delegat na metode check_Pixels do uzywania w wyrazeniu regularnym nizej
            Action<int, int> delegater = check_Pixels;

            var tasks = new Task[number_Divisions];

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

            TimeSpan time = DateTime.Now - begin;

            //petla odpowiedzialna za wpisanie przetowrzonych pixeli do wyjsciowej bitmapy
            for (int i = 0; i < number_Pixels; i++)
            {
                int y = i / bitPic.Width;
                int x = i % bitPic.Width;
                bitPic.SetPixel(x, y, Color.FromArgb(pixels[i]));
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
                fourPixels[iterator] = pixels[i];

                if (++iterator == 4 || i == end)
                {
                    int[] color = new int[4];
                    for(int x = 0; x < 4; x++)
                    {
                        color[x] = userColor.ToArgb();
                    }
                    MyProc1(fourPixels, color); 
                    iterator = 0;

                    pixels[i - 3] = fourPixels[0];
                    pixels[i - 2] = fourPixels[1];
                    pixels[i - 1] = fourPixels[2];
                    pixels[i] = fourPixels[3];
                    

                }
            }
        }
    }
}




