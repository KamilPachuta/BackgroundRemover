using System.Drawing;

namespace RemoverCS
{
    public class Remover
    {
        public static Color checkPixel(Color pixel, Color userColor, int scope)
        {
            if (pixel.R - scope < userColor.R && pixel.R + scope > userColor.R &&
                    pixel.G - scope < userColor.G && pixel.G + scope > userColor.G &&
                    pixel.B - scope < userColor.B && pixel.B + scope > userColor.B)
            {
                return Color.Empty;
            }
            return pixel;
        }

    }
}