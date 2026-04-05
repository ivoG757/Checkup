using System;
using System.Collections.Generic;
using System.Text;

namespace Checkup.App.Common
{
    public static class ColorsHolder
    {
        /// <summary>
        /// Represents the default color used for light squares on a chessboard.
        /// </summary>
        public static Color lightSquareColor = Color.FromArgb("#EEEED2");

        /// <summary>
        /// Represents the color used for dark squares on a chessboard.
        /// </summary>
        public static Color darkSquareColor = Color.FromArgb("#769656");

        /// <summary>
        /// Represents the color used to highlight a selected square.
        /// </summary>
        public static Color selectedSquareColor = Colors.Orange;

        /// <summary>
        /// Represents the default color used to highlight elements in the user interface.
        /// </summary>
        public static Color highlightColor = Colors.LightSkyBlue;
    }
}
