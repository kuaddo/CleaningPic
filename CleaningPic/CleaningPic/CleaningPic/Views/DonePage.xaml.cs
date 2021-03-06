﻿using CleaningPic.Data;
using CleaningPic.Utils;
using CleaningPic.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace CleaningPic.Views
{
    public partial class DonePage : ContentPage
	{
        public DonePage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            (BindingContext as DoneViewModel).OnAppearing();

            MessagingCenter.Subscribe<DoneViewModel, Cleaning>(
                this,
                DoneViewModel.navigateWebBrowserMessage,
                (sender, args) => DisplayLinkAsync(args));
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<DoneViewModel, Cleaning>(this, DoneViewModel.navigateWebBrowserMessage);
        }

        public void ReversedFalseMenu_Clicked(object sender, EventArgs e)
        {
            var vm = BindingContext as DoneViewModel;
            vm.Reversed = false;
            vm.OnAppearing();
        }

        public void ReversedTrueMenu_Clicked(object sender, EventArgs e)
        {
            var vm = BindingContext as DoneViewModel;
            vm.Reversed = true;
            vm.OnAppearing();
        }

        public void ListViewItem_Clicked(object sender, EventArgs e)
        {
            var listView = ((ListView)sender);
            if (listView.SelectedItem != null)
            {
                var cleaning = listView.SelectedItem as Cleaning;
                ((ListView)sender).SelectedItem = null;
                Navigation.PushAsync(new DetailPage(cleaning, false, false));
            }
        }

        private async void DisplayLinkAsync(Cleaning c)
        {
            var tools = new List<string>();
            var links = new List<string>();
            for (int i = 0; i < c.Tools.Count; i++)
            {
                if (!string.IsNullOrEmpty(c.Links[i]))
                {
                    tools.Add(c.Tools[i]);
                    links.Add(c.Links[i]);
                }
            }
            if (tools.Count == 0) return;
            var result = await DisplayActionSheet("買いたい物を選択してください", "キャンセル", null, tools.ToArray());
            if (result == null || result == "キャンセル") return;
            var link = links[tools.IndexOf(result)];
            DependencyService.Get<IWebBrowser>().Open(new Uri(link));
        }
    }
}