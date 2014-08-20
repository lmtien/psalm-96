using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows;
using System.Windows.Media.Animation;

namespace Psalm_96
{
    public class CustomAnimation
    {
        /// <summary>
        /// Return a Double Animation
        /// </summary>
        public static DoubleAnimation GetDouble(double from, double to, double duration)
        {
            DoubleAnimation result = new DoubleAnimation();
            result.From = from;
            result.To = to;
            result.Duration = new Duration(TimeSpan.FromSeconds(duration));

            return result;
        }
    }
}
