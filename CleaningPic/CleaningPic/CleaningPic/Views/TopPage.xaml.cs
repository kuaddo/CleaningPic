﻿using CleaningPic.Data;
using CleaningPic.Utils;
using CleaningPic.ViewModels;
using System;
using System.IO;

using Xamarin.Forms;

namespace CleaningPic.Views
{
    // このページのMVVMは妥協することにした
    public partial class TopPage : CarouselPage
	{
        public const string navigateWantToDoPageMessage = "navigateWantToDoPageMessage";
        private const int imageSize = 163;

        public TopPage ()
		{
			InitializeComponent();
            CurrentPageChanged += async (sender, e) =>
            {
                if (CurrentPage is CameraPage cameraPage)
                {
                    CurrentPage = topPage;      // カメラ処理を立ち上げる前に元ページに戻す
                    var imageData = await cameraPage.LaunchCamera();
                    if (imageData != null)
                    {
                        var data = DependencyService.Get<IImageEditor>().SquareAndResize(imageData, imageSize);
                        await Navigation.PushAsync(new UploadPage(data));
                    }
                }
            };

            MessagingCenter.Subscribe<TopViewModel, (bool, string)>(
                this,
                TopViewModel.navigateNotificationSettingPageMessage,
                async (sender, args) => { await Navigation.PushAsync(new NotificationSettingPage(args.Item1, args.Item2)); });
        }

        public async void SelectImageButton_Clicked(object sender, EventArgs e)
        {
            var photo = await Plugin.Media.CrossMedia.Current.PickPhotoAsync();
            if (photo != null)
            {
                using (var stream = photo.GetStream())
                using (var ms = new MemoryStream())
                {
                    if (stream == null) return;
                    stream.CopyTo(ms);
                    var data = DependencyService.Get<IImageEditor>().SquareAndResize(ms.ToArray(), imageSize);
                    await Navigation.PushAsync(new UploadPage(data));
                }
            }
        }

        public void ListViewItem_Clicked(object sender, EventArgs e)
        {
            var listView = ((ListView)sender);
            if (listView.SelectedItem != null)
            {
                var cleaning = listView.SelectedItem as Cleaning;
                ((ListView)sender).SelectedItem = null;
                Navigation.PushAsync(new DetailPage(cleaning));
            }
        }

        public void GoWantToDoLabel_Clicked(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, navigateWantToDoPageMessage);
        }
    }
}