using Acr.UserDialogs;
using Newtonsoft.Json;
using Plugin.Connectivity;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
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
        private bool _isPostUploaded;
        public double Latitude { get; private set; }
        public double Longitude { get; private set; }

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
                PhotoSize = PhotoSize.Full,
                CompressionQuality = 50,
                AllowCropping = true,
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
            if (AppConstant.UserId <= 0)
            {
                await DisplayAlert(AppConstant.ErrorHeading, "Login required for upload post", AppConstant.ErrorAcceptance);
                return;
            }
            using (UserDialogs.Instance.Loading(AppConstant.PleaseWait))
            {
                if (await GeoLocation())
                {
                    var response = await ApiCallHelper.UploadPost(new Post { id = AppConstant.UserId.ToString(), file = _imageSource, latitude = Latitude.ToString(), longitude = Longitude.ToString() });
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
                                    await Navigation.PushAsync(new PostList(string.Empty));
                                    return;
                                }
                            }
                        }
                    }
                    await DisplayAlert(AppConstant.ErrorHeading, AppConstant.InvalidCredentialsErrorMessage, AppConstant.ErrorAcceptance);
                    return;
                }
                else if (Longitude <= 0 && Latitude <= 0)
                {
                    await DisplayAlert(AppConstant.ErrorHeading, "Location not found please try agin later", AppConstant.ErrorAcceptance);
                    return;
                }
            }
        }

        private async Task<bool> GeoLocation()
        {
            try
            {
                using (UserDialogs.Instance.Loading(AppConstant.GettingLocation))
                {
                    var hasPermission = await Utils.CheckPermissions(Permission.Location);
                    if (!hasPermission)
                    {
                        return false;
                    }

                    var locator = CrossGeolocator.Current;
                    locator.DesiredAccuracy = 500;
                    var position = new Position();

                    position = await locator.GetPositionAsync(TimeSpan.FromSeconds(10), null, true);
                    if (position?.Longitude > 0 && position.Latitude > 0)
                    {
                        Longitude = position.Longitude;
                        Latitude = position.Latitude;
                        return true;
                    }

                    position = await locator.GetLastKnownLocationAsync();
                    if (position != null)
                    {
                        Longitude = position.Longitude;
                        Latitude = position.Latitude;
                        return true;
                    }

                    if (AppConstant.Longitude > 0 && AppConstant.Latitude > 0)
                    {
                        Longitude = AppConstant.Longitude;
                        Latitude = AppConstant.Latitude;
                        return true;
                    }

                    if (CrossConnectivity.Current.IsConnected == false)
                    {
                        await UserDialogs.Instance.AlertAsync("no inetrnet", "LeftBehind", "OK");
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync("Unable to get your location, please check if location service is ON..", "LeftBehind", "OK");
            }
            return false;
        }

        private async void Upload()
        {
            var response = await ApiCallHelper.UploadPost(new Post { id = AppConstant.UserId.ToString(), file = _imageSource, latitude = Latitude.ToString(), longitude = Longitude.ToString() });
            if (response != null)
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var userDetails = JsonConvert.DeserializeObject<ResponseObject<string>>(response.Content);
                    if (userDetails != null)
                    {
                        if (userDetails.StatusCode == HttpStatusCode.BadRequest)
                        {
                            await DisplayAlert(AppConstant.ErrorHeading, userDetails.ErrorMessage, AppConstant.ErrorAcceptance);
                            return;
                        }
                        else
                        {
                            await Navigation.PushAsync(new PostList(string.Empty));
                            return;
                        }
                    }
                }
            }
        }


        //public void SetControlValues()
        //{
        //    if (Longitude > 0 && Latitude > 0)
        //    {
        //        GeoPromptText.IsVisible = false;
        //        GridLatLong.IsVisible = true;
        //        LabelLatitude.Text = "Latitude: " + Latitude;
        //        LabelLongitude.Text = "Longitude: " + Longitude;
        //    }
        //}
    }
}