using System.Drawing;
using System.Runtime.CompilerServices;

namespace RemoverCS
{
    public class Remover
    {
        public static void checkPixels(int[] pixel, int[] userColor)
        {
            for (int i = 0; i < pixel.Length; i++)
            {
                if (pixel[i] == userColor[i])
                {
                    pixel[i] = 0x00000000;
                }
            }
        }
    }
}