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
        
        private bool _isPageLoaded;

        private bool _isOffLine;

        public bool IsOffline
        {
            get { return _isOffLine; }
            set { _isOffLine = value;
                AppConstant.IsOnline = !_isOffLine;
                OnPropertyChanged("IsOffline");
            }
        }

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

        public PostList(string title, bool isOffLine = false)
        {
            InitializeComponent();
            IsOffline = isOffLine;
            Device.BeginInvokeOnMainThread(() =>
            {
                Title = (string.IsNullOrWhiteSpace(title) == false ? title : "Left Behind");
            });
            NavigationPage.SetHasBackButton(this, true);
            this.BindingContext = this;
        }

        private void AddToolBarItem()
        {
            if (IsOffline == false)
            {
                this.ToolbarItems.Clear();
                var toolBarItem = new ToolbarItem
                {
                    Icon = "add.png",
                    Priority = 1
                };
                toolBarItem.Clicked += ToolbarItemClicked;
                this.ToolbarItems.Add(toolBarItem);
            }
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            AddToolBarItem();
            if (_isOffLine)
            {
                GetOfflineSavedPosts();
            }
            else
            {
                if (_isPageLoaded)
                {
                    return;
                }
                _isPageLoaded = true;
                await GetPosts();
            }
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

        private void GetOfflineSavedPosts()
        {
            using (UserDialogs.Instance.Loading(AppConstant.PleaseWait))
            {
                var result = ApiCallHelper.GetOfflineSavedPosts();
                if (result != null)
                {
                    Images = new ObservableCollection<Post>(result);
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

        private async void ToolbarItemClicked(object sender, EventArgs e)
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
            AppConstant.IsOnline = true;
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
            var msg = string.Empty;
            if (result == HttpStatusCode.Created)
            {
                msg = "Post saved successfully";
            }
            else if (result == HttpStatusCode.Found)
            {
                msg = "Post already exists";
            }
            if (string.IsNullOrWhiteSpace(msg) == false)
            {
                var toastConfig = new ToastConfig(msg)
                {
                    BackgroundColor = Color.Green,
                    MessageTextColor = Color.White,
                    Position = ToastPosition.Top
                };
                UserDialogs.Instance.Toast(toastConfig);
            }
        }
    }
}