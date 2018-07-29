using Acr.UserDialogs;
using Newtonsoft.Json;
using Samaritan.Classes;
using Samaritan.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Samaritan.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            this.loginBtn.TappedCommand = new Command(this.LoginButtonTapped);
            this.loginBtn.SkipTappedCommand = new Command(this.SkipButtonTapped);
        }

        private async void SkipButtonTapped(object obj)
        {
            AppConstant.UserId = 0;
            await Navigation.PushAsync(new PostList());
        }

        //login button clicked event
        public async void LoginButtonTapped(object obj)
        {
            string email;
            string password;
            email = emailEntry.TextValue;
            password = passwordEntry.TextValue;
            //check for empty Email
            if (!string.IsNullOrEmpty(email))
            {
                // Check for valid Email format
                Regex regexEmail = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                Match matchEmail = regexEmail.Match(email);
                if (!matchEmail.Success)
                {
                    // show error 
                    await DisplayAlert("Error", "Please enter a valid Email id", "ok");
                    return;
                }
            }
            else
            {
                // show error 
                await DisplayAlert("Error", "Please enter your Email.", "ok");
                return;
            }
            if (string.IsNullOrEmpty(password))
            {
                //show error
                await DisplayAlert("Error", "Please enter your Password.", "ok");
                return;
            }
            await ValidateAndSaveCredentials(email, password);
        }
        public void EmailEntryCompleted(object obj)
        {
            if (!string.IsNullOrEmpty(emailEntry.TextValue))
            {
                this.passwordEntry.Focus();
            };
        }
        private async Task ValidateAndSaveCredentials(string email, string password)
        {
            AppConstant.LoginUser = null;
            using (UserDialogs.Instance.Loading(AppConstant.PleaseWait))
            {
                var response = await ApiCallHelper.ValidateUser(new LoginRequest { Email = email, Password = password });
                if (response != null)
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var userDetails = JsonConvert.DeserializeObject<ResponseObject<User>>(response.Content);
                        if (userDetails != null)
                        {
                            if (userDetails.StatusCode == HttpStatusCode.BadRequest)
                            {
                                return;
                            }
                            else
                            {
                                var userId = userDetails.Response.id;
                                //Plugin.Settings.UserProfileSettings = (loggedInSwitch.IsToggled ? JsonConvert.SerializeObject(new LoginRequest { Email = email, Password = password, UserId = userId, Provider = AuthenticatorProvider.NSL }) : string.Empty);
                                AppConstant.UserId = userId;
                                //AppConstant.Provider = AuthenticatorProvider.NSL.ToString();
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
        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
    }
}