using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osu_tool.ViewModel
{
    public class MainWindowViewModel
    {
        ObservableCollection<object> _pages;

        public ObservableCollection<object> Pages
        {
            get { return _pages; }
            set { _pages = value; }
        }

        public MainWindowViewModel()
        {
            _pages = new ObservableCollection<object>();
            _pages.Add(new BeatmapsViewModel());
            _pages.Add(new SettingsViewModel());
        }

    }
}
