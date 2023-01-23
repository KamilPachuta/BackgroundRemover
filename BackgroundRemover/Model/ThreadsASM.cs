using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BackgroundRemover.Model
{
    internal class ThreadsASM : Threads
    {

        private readonly int scope = 0;
        private int[] pixels;

        [DllImport(@"D:\Documents\GitHub\lll\BackgroundRemove\x64\Debug\BackgroundRemoverAsm.dll")]
        private static extern int MyProc1(int pixel, int color, int scope);
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
        public override Bitmap execute()
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
                bitPic.SetPixel(x, y, Color.FromArgb(pixels[i]));
            }

            return bitPic;
        }

        /**
         * check_Pixels metoda - Głowna logika aplikacji.
         * Sprawdza warunek wymazania koloru dla zadanego zakresu danych w tablixy pixeli. 
         */
        public void check_Pixels(int start, int end)
        {

            //tutaj wywolac biblioteke asm ktora bedzie przyjmowac wskaznik na tablice pixels typu int, indexsatart i indexend 
            for (int i = start; i < end; i++)
            {
                int color = userColor.ToArgb();
                pixels[i] = MyProc1(pixels[i], color, scope);
            }

        }
    }
}




