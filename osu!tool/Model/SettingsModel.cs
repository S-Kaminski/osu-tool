using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osu_tool.Model
{
    public class SettingsModel
    {
        public string Path { get; set; }

        public SettingsModel(string path) //ctor
        {
            this.Path = path;
        }
    }
}
