using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;

namespace DrawerGraphics.Utils
{
    public static class GeneratorColors
    {
        public static ReadOnlyCollection<Color> UsedColors => _usedColors.AsReadOnly();

        private static readonly List<Color> _usedColors = new List<Color>();

        public static void GenerateColors(int number)
        {
            for (var i = 0; i < number; i++)
            {
                GetRandomColor();
            }
        }
        
        private static Color GetRandomColor()
        {
            while (true)
            {
                var randomGen = new Random();
                var randomColor = Color.FromArgb((byte)randomGen.Next(255), (byte)randomGen.Next(255), (byte)randomGen.Next(255), (byte)randomGen.Next(255));

                if (!_usedColors.Contains(randomColor))
                {
                    _usedColors.Add(randomColor);
                }
                else
                {
                    continue;
                }

                return randomColor;
            }
        }
    }
}