using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Data;

namespace Psalm_96
{
    public class Common
    {
        //Define constant
        public const string DATA_DIR = "data";
        public const string DATA_EXTS = ".lmt";

        public const bool CURRENT_BACKGROUND = true;

        public const string VIDEO_DIR = "video";
        public static string[] VIDEO_EXTS = new string[] { ".wmv" };
        public const string VIDEO_NONE = "--- NONE ---";

        public const int VIDEO_SPEED = 100;
        public const double TRANSITION_SPEED = 0.3;

        public static Binding vlcBinding = new Binding("VideoSource");
    }
}
