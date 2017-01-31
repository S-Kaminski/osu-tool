using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osu_tool.Model
{
    public class Settings
    {
        #region SettingsModel
        public static SettingsModel SettingsProp
        {
            get { return ReadSettings(); }
            set { SaveSettings(value); }
        }

            #region SettingsModel Methods
            private static void SaveSettings(SettingsModel settingsModel)
            {
                Properties.Settings settings = Properties.Settings.Default;
                settings.Path = settingsModel.Path;
                settings.Save();
            }
            private static SettingsModel ReadSettings()
            {
                Properties.Settings settings = Properties.Settings.Default;
                return new SettingsModel(settings.Path);
            }
            #endregion
        #endregion 

    }
}
