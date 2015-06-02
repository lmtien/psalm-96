using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using Newtonsoft.Json;

namespace Psalm_96
{
    public class Song
    {
        public string SongName { get; set; }
        public string VideoName { get; set; }
        public double VideoSpeed { get; set; }
        public double TransitionSpeed { get; set; }
        public string Content { get; set; }
        public bool Playlist { get; set; }

        public void Save()
        {
            //write to file
            File.WriteAllText(System.IO.Path.Combine(Common.DATA_DIR, SongName + Common.DATA_EXTS), JsonConvert.SerializeObject(this));
        }
    }
}
