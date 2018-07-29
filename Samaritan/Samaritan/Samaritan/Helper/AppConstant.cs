using Samaritan.Classes;

namespace Samaritan.Helper
{
    public class AppConstant
    {
        public static int UserId { get; set; }

        /// <summary>
        /// Gets or sets is user logout
        /// </summary>
        public static bool IsLogOut { get; set; }

        /// <summary>
        /// Gets or set login provider
        /// </summary>
        public static string Provider { get; set; }

        /// <summary>
        /// Gets or sets login user
        /// </summary>
        public static User LoginUser { get; set; }

        /// <summary>
        /// Gets acceptance text
        /// </summary>
        public readonly static string TextOk = "OK";


        /// <summary>
        /// Gets reject text
        /// </summary>
        public readonly static string TextCancel = "CANCEL";

        /// <summary>
        /// Key for authtoken
        /// </summary>
        public readonly static string TextAuthToken = "AuthToken";

        public static string Source { get; set; }

        public static double Longitude { get; set; }

        public static double Latitude { get; set; }

        /// <summary>
        /// Gets authtoken value
        /// </summary>
        //public static string AuthToken
        //{
        //    get
        //    {
        //        return (LoginUser == null ? string.Empty : LoginUser.Token);
        //    }
        //}

        /// <summary>
        /// Text for message key incase of error 
        /// </summary>
        public readonly static string ErrorResponseMessageKey = "message";

        /// <summary>
        /// Error message for not found
        /// </summary>
        public readonly static string NotFoundErrorMessage = "The information you are trying to view could not be found, it may have been recently removed.";

        /// <summary>
        /// Error message for forbidden
        /// </summary>
        public readonly static string ForBiddenErrorMessage = "Your account does not have sufficient permissions, please contact your administrator.";

        /// <summary>
        /// Error message which occurs due to unauthorized access
        /// </summary>
        public readonly static string UnAuthorizedErrorMessage = "Your account is unauthorized to access this resource, please contact your administrator.";

        /// <summary>
        /// Error message when other error occurs.
        /// </summary>
        public readonly static string DefaultErrorMessage = $"Unfortunately an error has occurred ";

        /// <summary>
        /// Error message when error occurrs.
        /// </summary>
        public readonly static string ErrorMessageErrorOccurred = "Unable to complete your request at this time. Please try again.";

        /// <summary>
        /// Gets please wait text
        /// </summary>
        public readonly static string PleaseWait = "Please Wait";

        /// <summary>
        /// Gets getting location text
        /// </summary>
        public readonly static string GettingLocation = "Getting location";

        /// <summary>
        /// Gets default logo path
        /// </summary>
        public static string DefaultLogoPath = "defaultlogo.png";

        /// <summary>
        /// property to show error heading
        /// </summary>
        public static readonly string ErrorHeading = string.Empty;

        /// <summary>
        /// property to show an error message
        /// </summary>
        public static readonly string ErrorMessage = "Sorry, Left Behine is not reachable. Please check your internet connection and try again";


        /// <summary>
        /// property to show an invalid credentails error message
        /// </summary>
        public static readonly string InvalidCredentialsErrorMessage = "Please enter valid credentials.";

        /// <summary>
        /// property to show an username already exists error message
        /// </summary>
        public static readonly string UsernameAlreadyExistsErrorMessage = "Username already exists.";

        /// <summary>
        /// property to show something went wrong error message
        /// </summary>
        public static readonly string SomethingWentWrongErrorMessage = "Something went wrong, please try again.";

        /// <summary>
        /// Field to show please register to proceed error message
        /// </summary>
        public static string PromptForLogin = "Please login to proceed";

        /// <summary>
        /// property to accept user acceptance for an error
        /// </summary>
        public static string ErrorAcceptance = "OK";

        public static readonly string AuthenticationFailed = "Authentication failed";
        internal static int RecordCount = 10;

        internal static int CommentCount = 10;

        /// <summary>
        /// Multiline text box height
        /// </summary>
        public static readonly double MultilineTextHeight = Xamarin.Forms.Device.OnPlatform(70, 75, 115);

        /// <summary>
        /// Gets or sets a value indicating whether or not events should be refreshed.
        /// </summary>
        public static bool ShouldRefreshComments { get; set; } = true;
        public static bool IsUserCredentialsAvailable()
        {
            //var userSettings = Settings.UserProfileSettings;
            //if (userSettings != null && string.IsNullOrWhiteSpace(userSettings) == false)
            //{
            //    return true;
            //}
            return false;
        }

    }
}