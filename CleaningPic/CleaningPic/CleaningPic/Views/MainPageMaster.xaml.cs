﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CleaningPic.Views
{
    public partial class MainPageMaster : ContentPage
    {
        public ListView ListView;

        public MainPageMaster()
        {
            InitializeComponent();

            BindingContext = new MainPageMasterViewModel();
            ListView = MenuItemsListView;
        }

        class MainPageMasterViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<MainPageMenuItem> MenuItems { get; set; }
            
            public MainPageMasterViewModel()
            {
                MenuItems = new ObservableCollection<MainPageMenuItem>(new[]
                {
                    new MainPageMenuItem { Id = 0, Title = "トップ",   TargetType = typeof(TopPage),            Params = new object[] { false } },
                    new MainPageMenuItem { Id = 1, Title = "カメラ" },
                    new MainPageMenuItem { Id = 2, Title = "やりたい", TargetType = typeof(CleaningTabbedPage), Params = new object[] { true  } },
                    new MainPageMenuItem { Id = 3, Title = "やった",   TargetType = typeof(CleaningTabbedPage), Params = new object[] { false } },
                    new MainPageMenuItem { Id = 4, Title = "豆知識" },
                    //new MainPageMenuItem { Id = 4, Title = "ヘルプ" },
                });
            }
            
            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }
    }
}