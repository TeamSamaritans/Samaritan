using Acr.UserDialogs;
using Newtonsoft.Json;
using Realms;
using Samaritan.Classes;
using Samaritan.Helper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Samaritan.View
{
    public partial class PostList : ContentPage
    {
        private ObservableCollection<Post> _images;
        private string base64String;

        public ObservableCollection<Post> Images
        {
            get { return _images; }
            set
            {
                _images = value;
                OnPropertyChanged("Images");
            }
        }

        public int ImageId { get; set; }

        public Post ImageObject { get; set; }

        public PostList()
        {
            InitializeComponent();
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
                        var imageData = JsonConvert.DeserializeObject<ResponseObject<List<Post>>>(response.Content);
                        if (imageData != null)
                        {
                            if (imageData.StatusCode == HttpStatusCode.BadRequest)
                            {
                                return;
                            }
                            else
                            {
                                Images = new ObservableCollection<Post>(imageData.Response);
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
            if (AppConstant.UserId <= 0)
            {
                await DisplayAlert(AppConstant.ErrorHeading, "Login required for upload post", AppConstant.ErrorAcceptance);
                return;
            }
            await Navigation.PushAsync(new AddPost());
        }

        private void MenuItemsListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            MenuItemsListView.SelectedItem = null;
        }

        private async Task tapGestureRecognizerSave_Tapped(object sender, EventArgs e)
        {
            var value = e as TappedEventArgs;
            var post = (Post)value.Parameter;
            if (post != null)
            {
               await DownLoadPost(post);
            }
        }

        private async Task DownLoadPost(Post post)
        {
            var result = await ApiCallHelper.DownLoadPost(post);
            if (!result)
            {
                var toastConfig = new ToastConfig("Post saved successfully!");
                toastConfig.BackgroundColor = Color.Green;
                toastConfig.MessageTextColor = Color.White;
                toastConfig.Position = ToastPosition.Top;
                UserDialogs.Instance.Toast(toastConfig);
            }
        }
    }
}