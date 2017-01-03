using System.Windows;
using System.IO;
using Newtonsoft.Json;

namespace Psalm_96
{
    public class Configuration
    {
        public int ScreenWidth { get; set; }
        public int ScreenHeight { get; set; }
        public VerticalAlignment TextVerticalAlignment { get; set; }
        public bool Bilingual { get; set; }

        public void Save()
        {
            //write to file
            File.WriteAllText(Common.CONFIG_FILE, JsonConvert.SerializeObject(this));
        }
    }
}
