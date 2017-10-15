﻿using CleaningPic.Data;
using CleaningPic.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CleaningPic.Views
{
	public partial class TopPage : CarouselPage
	{
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
                        var square = DependencyService.Get<IImageEditor>().Square(imageData);
                        var data = DependencyService.Get<IImageEditor>().Resize(square, 144);   // 72 * 2 = 144
                        await Navigation.PushAsync(new UploadPage(data));
                    }
                }
            };
		}

        public void CameraMenu_Clicked(object sender, EventArgs e)
        {
            CurrentPage = cameraPage;
        }

        public void DeleteButton_Clicked(object sender, EventArgs e)
        {
            using (var ds = new DataSource())
            {
                ds.RemoveAllCleaning();
            }
        }
    }
}