﻿using Acr.UserDialogs;
using Newtonsoft.Json;
using Samaritan.Classes;
using Samaritan.Helper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Samaritan.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PostList : ContentPage
	{
        private ObservableCollection<ImageList> _images;

        public ObservableCollection<ImageList> Images
        {
            get { return _images; }
            set
            {
                _images = value;
                OnPropertyChanged("Images");
            }
        }

        public int ImageId { get; set; }

        public ImageList ImageObject { get; set; }

        public PostList ()
		{
			InitializeComponent ();
            this.BindingContext = this;
		}

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await GetPosts();
        }

        private async Task GetPosts()
        {
            using (UserDialogs.Instance.Loading(AppConstant.PleaseWait))
            {
                var response = await ApiCallHelper.GetAllPosts();
                if (response != null)
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var imageData = JsonConvert.DeserializeObject<ResponseObject<List<ImageList>>>(response.Content);
                        if (imageData != null)
                        {
                            if (imageData.StatusCode == HttpStatusCode.BadRequest)
                            {
                                return;
                            }
                            else
                            {
                                Images = new ObservableCollection<ImageList>(imageData.Response);
                            }
                        }
                    }
                }
            }
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var value = e as TappedEventArgs;
            if (value != null)
            {
                await Navigation.PushAsync(new ShowPost(value.Parameter.ToString()));
            }
        }

        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddPost());
        }

        private void ToolbarItem_Clicked_1(object sender, EventArgs e)
        {
            
        }
    }
}