using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackgroundRemover.Model
{
    abstract class Threads
    {
        protected readonly Color userColor;
        protected Bitmap bitPic;
        


        /**
         * Threads konstruktor - przypisauje zapisauje podany przez uzytkownika kolor oraz obraz zaladaowany do modelu.
         */
        public Threads(Color uColor, Bitmap picture)
        {
            userColor = uColor;
            bitPic = picture;
        }

        abstract public (Bitmap, double) execute();
    }
}
