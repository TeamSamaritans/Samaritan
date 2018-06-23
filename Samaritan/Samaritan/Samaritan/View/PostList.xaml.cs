using Acr.UserDialogs;
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
            //Images = new ObservableCollection<ImageList>();
            //Images.Add(new ImageList() { id = "1", image_src = "www.google.com" });
            //Images.Add(new ImageList() { id = "1", image_src = "www.google.com" });
            //Images.Add(new ImageList() { id = "1", image_src = "www.google.com" });
            //Images.Add(new ImageList() { id = "1", image_src = "www.google.com" });
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
    }
}