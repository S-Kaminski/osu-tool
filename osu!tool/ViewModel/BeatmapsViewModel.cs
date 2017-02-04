using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osu_tool.ViewModel
{
    using osu_tool.Model;
    using Service;
    using System.Collections;
    using System.IO;
    using System.Windows.Input;

    public class BeatmapsViewModel : BaseViewModel
    {
        public string TabName { get { return "Beatmaps"; } }

        private readonly SettingsModel settingsModel = Settings.SettingsProp;
        private readonly BeatmapsModel beatmapsModel = new BeatmapsModel();

        public DirectoryInfo[] matches;
        string list = "";
        int matchCount = 0;
        List<FileInfo> items = new List<FileInfo>();


        public string Path { get { return settingsModel.Path + @"\Songs"; } }
        public string List
        {
            get
            {
                string temp = "";
                foreach(var item in items)
                {
                    temp += item.FullName + "\n";
                }
                return temp;
            }
            set
            {
                list += value;
                OnPropertyChanged("List");
            }
        }

        public string Query
        {
            get
            {

                return beatmapsModel.BeatmapsQuery;
            }
            set
            {
                beatmapsModel.BeatmapsQuery = value;
                OnPropertyChanged("Query");
            }
        }

        private void Matching()
        {
            #region creates array from search result - #matching song folders
            DirectoryInfo info = new DirectoryInfo(Path);
            DirectoryInfo[] catalogs = info.GetDirectories();
            matches = new DirectoryInfo[catalogs.Length-1];
            int i = 0;
            foreach(var cat in catalogs)
            {
                //if(cat.Name.Contains(Query)) --case sensitive
                //if(String.Equals(cat.Name, Query, StringComparison.OrdinalIgnoreCase)) --doesnt work
                if(cat.Name.Contains(Query, StringComparison.OrdinalIgnoreCase)) //This is what user's looking for --case insensitive
                    matches[i++] = cat;
                
                else continue; 
            }
            
            foreach (var mct in matches)//debug purposes
            {

                if(mct != null) list += mct.ToString();

            }
            #endregion
            #region files in eachfolder
            List<FileInfo> files = new List<FileInfo>();
            foreach(var match in matches)
            {
                if (match != null)
                {
                    FileInfo[] infos = match.GetFiles();
                    foreach (var ins in infos)
                    {
                        if (ins.Extension == ".osu") //ins.Name.Contains(".osu")
                        {
                            matchCount++;
                            files.Add(ins);
                        }
                    }
                }

            }
            
            FileStream fs;
            StreamReader sr;
            foreach (var item in files)
            { 
                try
                {
                    fs = new FileStream(item.FullName, FileMode.Open);
                    sr = new StreamReader(fs);
                    string tempString;
                    while((tempString = sr.ReadLine()) != null)
                    {
                        if (tempString.Contains("AudioFilename: "))
                        {
                            string mp3 = tempString.Remove(0, 15);
                            FileInfo fi = new FileInfo(item.DirectoryName + "\\"+mp3);
                            //if (!items.Contains(fi))
                            //{
                                items.Add(fi);
                            //}
                        }
                        if(tempString.Contains("0,0,\""))
                        {
                            string bg = tempString;
                            if (tempString.Contains(",0,0"))
                            {
                                bg = bg.Remove(bg.Length - 4, 4);
                            }
                            bg = bg.Remove(0, 5);
                            bg = bg.Remove(bg.Length-1,1);
                            FileInfo fi = new FileInfo(item.DirectoryName + "\\" + bg);
                            //if (!items.Contains(fi))
                            //{
                                items.Add(fi);
                            //}
                        }
                       
                        else continue;
                    }

                    sr.Close();
                    fs.Close();
                }
                catch
                {
                    continue;
                }

            }

            #endregion
        }

        private ICommand searchCommand;
        public ICommand Search
        {
            get
            {
                if (searchCommand == null)
                {
                    searchCommand = new RelayCommand(argument =>
                    {
                        Matching();
                        //System.Windows.MessageBox.Show(items[1].FullName);

                    });
                }
                return searchCommand;
            }

        }

    }
}
