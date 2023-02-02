using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Windows.UI.Input.Spatial;

namespace BackgroundRemover.Model
{
    internal class Model1
    {
        private Bitmap bitPic;
        

        /**
         * Model1 konstruktor - przypsuje zaladowany przez uzytkowanika obraz do pola prywatnego
         */
        public Model1(Bitmap bitPic)
        {
             this.bitPic = bitPic;             
        }

        /**
         * calculate_Image metoda - Odpowiedzialna za pomiar czasu,
         * utowrzenie obiektu klasy odpowiedzialnej za przeprowadzenie obliczen za pomoca podanej przez uzytkownika biblioteki,
         * zapisanie przetowrzonego obrazu oraz zwrocenie czasu oraz obrazu w formie krotki
         */
        internal (Bitmap, double) calculate_Image(Color uColor, int nThreads, bool choice)
        {
            
            Threads threads;
            if (choice)
                threads = new ThreadsCS(uColor, nThreads, bitPic); // konstrutor menadzera watkow C#
            else
                threads = new ThreadsASM(uColor, nThreads, bitPic); // konstruktor menadzera watkow Asm
 
            (bitPic, double time) = threads.execute();

           

            return (bitPic, time);
        }

       

    }
}
