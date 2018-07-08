using Acr.UserDialogs;
using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions.Abstractions;
using Samaritan.Classes;
using Samaritan.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Samaritan.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddPost : ContentPage
    {
        private string _imageSource;

        public AddPost()
        {
            InitializeComponent();
        }

        private async void OnImageTapped(object sender, EventArgs e)
        {
            var hasPermission = await Utils.CheckPermissions(Permission.Camera);
            if (!hasPermission)
            {
                return;
            }

            var result = await UserDialogs.Instance.ActionSheetAsync("Upload Image", "Cancel", null, null, "Camera", "Gallery");
            if (result == "Camera")
            {
                UploadImageViaCamera();
            }
            if (result == "Gallery")
            {
                UploadImageViaGallery();
            }
        }

        private async void UploadImageViaCamera()
        {
            await CrossMedia.Current.Initialize();
            var mediaOptions = new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                Directory = "LeftBehind",
                Name = "Camera.jpg",
            };

            var file = await CrossMedia.Current.TakePhotoAsync(mediaOptions);

            if (file != null)
            {
                byte[] imageArray = System.IO.File.ReadAllBytes(file.Path);// convert path to byte array
                _imageSource = Convert.ToBase64String(imageArray);

                // Show image on UI 
                switch (Device.RuntimePlatform)
                {
                    case Device.iOS:
                        image.Source = ImageSource.FromFile(file.Path);
                        break;

                    case Device.Android:
                        image.Source = ImageSource.FromStream(() =>
                        {
                            var stream = file.GetStream();
                            return stream;
                        });
                        break;
                }
            }
        }

        private async void UploadImageViaGallery()
        {
            await CrossMedia.Current.Initialize();
            var pickMediaOptins = new PickMediaOptions()
            {
                MaxWidthHeight = 800,
                RotateImage = true
            };
            var file = await CrossMedia.Current.PickPhotoAsync(pickMediaOptins);

            if (file == null)
                return;
            byte[] imageArray = System.IO.File.ReadAllBytes(file.Path);// convert path to byte array
            _imageSource = Convert.ToBase64String(imageArray);

            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    image.Source = ImageSource.FromFile(file.Path);
                    break;
                case Device.Android:
                    image.Source = file.Path;
                    break;
            }
        }

        private void toolbarItemUpload_Clicked(object sender, EventArgs e)
        {
            UploadPost();
        }

        private async void UploadPost()
        {
            using (UserDialogs.Instance.Loading(AppConstant.PleaseWait))
            {
                var response = await ApiCallHelper.UploadPost(new Post {id = AppConstant.UserId.ToString(), file = _imageSource, latitude = "23", longitude = "73" });
                if (response != null)
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var userDetails = JsonConvert.DeserializeObject<ResponseObject<string>>(response.Content);
                        if (userDetails != null)
                        {
                            if (userDetails.StatusCode == HttpStatusCode.BadRequest)
                            {
                                return;
                            }
                            else
                            {
                                await Navigation.PushAsync(new PostList());
                                return;
                            }
                        }
                    }
                }
                await DisplayAlert(AppConstant.ErrorHeading, AppConstant.InvalidCredentialsErrorMessage, AppConstant.ErrorAcceptance);
                return;
            }
        }
    }
}