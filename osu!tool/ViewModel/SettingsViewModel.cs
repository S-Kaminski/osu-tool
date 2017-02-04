using osu_tool.Model;
using osu_tool.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace osu_tool.ViewModel
{
    public class SettingsViewModel : BaseViewModel
    {
        public string TabName { get { return "Settings"; } }

        private readonly SettingsModel settingsModel = Settings.SettingsProp;
        

        public string Path
        {
            get { return settingsModel.Path; }
            set
            {
                settingsModel.Path = value;
                OnPropertyChanged("Path");
            }
        }
        public bool PathValidation
        {
            get
            {
                //checks for path validation -> these characters are unusable in paths (windows)
                if (Regex.IsMatch(Path, @"^(?:[a-zA-Z]\:|\\\\[\w\.]+\\[\w.$]+)\\(?:[\w]+\\)*\w([\w.])+$"))
                {
                    return true;
                }
                else return false;
            }
        }

        #region ICommand saveCommand
        private ICommand saveCommand;
        public ICommand Save
        {
            get
            {
                if (saveCommand == null)
                {
                    saveCommand = new RelayCommand(argument => 
                    {
                        Settings.SettingsProp = settingsModel;
                    },
                    argument=> (PathValidation)
                    );
                }
                return saveCommand;
            }
            
        }
        #endregion

        #region ICommand detectCommand
        private ICommand detectCommand;
        public ICommand Detect
        {
            get
            {
                if (detectCommand == null)
                {
                    detectCommand = new RelayCommand(argument =>
                    {
                        string dir;
                        var processes = System.Diagnostics.Process.GetProcessesByName("osu!");
                        if (processes.Length > 0) //gets path if osu!process is running
                        {
                            dir = processes[0].Modules[0].FileName;
                            dir = dir.Remove(dir.LastIndexOf('\\'));
                        }
                        else //if not running, gets it from RegEdit
                        {
                            using (Microsoft.Win32.RegistryKey osureg = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey("osu\\DefaultIcon"))
                            {
                                if (osureg != null)
                                {
                                    string osukey = osureg.GetValue(null).ToString();
                                    string osupath;
                                    osupath = osukey.Remove(0, 1);
                                    osupath = osupath.Remove(osupath.Length - 12);
                                    dir = string.Format(osupath);
                                }
                                else { dir = "osu! not detected"; }
                            }
                        }
                        Path = dir;
                    });
                }
                return detectCommand;
            }
            
        }

        #endregion

        

    }
}
