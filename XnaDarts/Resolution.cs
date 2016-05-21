using System;

namespace XnaDarts
{
    [Serializable]
    public class Resolution
    {
        public int Height;
        public int Width;

        public Resolution(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public override string ToString()
        {
            return Width + ", " + Height;
        }
    }
}