using Newtonsoft.Json.Linq;
using RestSharp.Portable;
using RestSharp.Portable.HttpClient;
using Samaritan.Classes;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Samaritan.Helper
{
    public static class ApiCallHelper
    {
        /// <summary>
        /// Error message response timed out.
        /// </summary> 
        public static readonly string ErrorMessageResponseTimedOut = "Unfortunately a timeout has occurred.";

        /// <summary>
        /// Timeout in seconds for poor networks.
        /// </summary>
        public static readonly double TimeoutInSecondsForPoorNetworks = 30;

        /// <summary>
        /// Base address.
        /// </summary>
        public static string BaseAddress = string.Empty;

        public static async Task<IRestResponse<T>> BaseRequest<T>(RestRequest request, string action = "Action", bool withAuthentication = true, bool showError = true, bool excludeTimeout = false)
        {
            BaseAddress = "http://taritas.in/left-behind/api/";

            var client = new RestClient(BaseAddress);
            IRestResponse<T> response;
            try
            {
                client.IgnoreResponseStatusCode = true;

                // If no network is available just show the message
                //if (CrossConnectivity.Current.IsConnected == false)
                //{
                //    UserDialogs.Instance.HideLoading();
                //    if (showError)
                //    {
                //        await UserDialogs.Instance.AlertAsync($"{action} is unavailable because your device isn't connected to the internet.", okText: AppConstants.TextOk);
                //    }
                //    return null;
                //}

                //if (withAuthentication)
                //{
                //    request.AddHeader(AppConstants.TextAuthToken, AppConstants.AuthToken);
                //}

                //client.UserAgent = "Expenses360";

                if (excludeTimeout)
                {
                    response = await client.Execute<T>(request, new CancellationTokenSource().Token);
                }
                else
                {
                    response = await client.Execute<T>(request, new CancellationTokenSource(TimeSpan.FromSeconds(TimeoutInSecondsForPoorNetworks)).Token);
                }
                if (!response.IsSuccess)
                {
                    if (response.StatusCode == HttpStatusCode.RequestTimeout && showError)
                    {
                        //await UserDialogs.Instance.AlertAsync(ErrorMessageResponseTimedOut, okText: AppConstants.TextOk);
                    }
                    else if (response.IsSuccess == false || response.StatusCode != HttpStatusCode.OK)
                    {
                        var message = string.Empty;

                        switch (response.StatusCode)
                        {
                            // handle specific status codes and create appropriate error message text 
                            case HttpStatusCode.NotFound:
                                {
                                    //message = AppConstants.NotFoundErrorMessage;
                                    break;
                                }

                            case HttpStatusCode.BadRequest:
                                {
                                    var responseMessage = JObject.Parse(response.Content);
                                    //message = (string)responseMessage[AppConstants.ErrorResponseMessageKey];
                                    break;
                                }
                            case HttpStatusCode.Forbidden:
                                {
                                    // message = AppConstants.ForBiddenErrorMessage;
                                    break;
                                }

                            // these status codes aren't "OK" but aren't errors either, so should be handled accordingly in the code that called this
                            case HttpStatusCode.Unauthorized:
                                {
                                    // message = AppConstants.UnAuthorizedErrorMessage;
                                    break;
                                }

                            // any other status codes are treated as errors
                            default:
                                {
                                    // message = $"{AppConstants.DefaultErrorMessage} : {response.StatusCode}";
                                    break;
                                }
                        }

                        if (!string.IsNullOrEmpty(message) && showError)
                        {
                            // await UserDialogs.Instance.AlertAsync(message, okText: AppConstants.TextOk);
                        }
                    }
                }

                return response;
            }
            catch (OperationCanceledException)
            {
                //UserDialogs.Instance.HideLoading();
                //UserDialogs.Instance.Alert($"{ErrorMessageResponseTimedOut}\nMore Information : {action}", okText: AppConstants.TextOk);
                return null;
            }
            catch (Exception ex)
            {
                var t = ex.Message;
                //UserDialogs.Instance.HideLoading();
                if (ex.InnerException != null && string.IsNullOrWhiteSpace(ex.InnerException.Message) == false)
                {
#if DEBUG
                    // UserDialogs.Instance.Alert($"{AppConstants.ErrorMessageErrorOccurred}\nInner Exception : {ex.InnerException.Message}", okText: AppConstants.TextOk);
#else
                    //UserDialogs.Instance.Alert($"{AppConstant.ErrorMessageErrorOccurred}", okText: AppConstant.TextOk);
#endif
                }
                else if (string.IsNullOrWhiteSpace(ex.Message) == false)
                {
#if DEBUG
                    // UserDialogs.Instance.Alert($"{AppConstants.ErrorMessageErrorOccurred}\nException Message : {ex.Message}", okText: AppConstants.TextOk);
#else
                    //UserDialogs.Instance.Alert($"{AppConstant.ErrorMessageErrorOccurred}", okText: AppConstant.TextOk);
#endif
                }
                else
                {
                    // UserDialogs.Instance.Alert(AppConstants.ErrorMessageErrorOccurred, okText: AppConstants.TextOk);
                }

                return null;
            }
        }

        public static async Task<IRestResponse<HttpResponseMessage>> ValidateUser(LoginRequest loginRequest)
        {
            var request = new RestRequest("Site/login", Method.POST);
            //request.AddJsonBody(loginRequest);
            //request.AddBody(loginRequest);
            request.AddParameter("email", loginRequest.Email);
            request.AddParameter("password", loginRequest.Password);
            return await BaseRequest<HttpResponseMessage>(request, "login", true);
        }

        //public static async Task<IRestResponse<HttpResponseMessage>> GetAllPosts()
        //{
        //    var request = new RestRequest("Record/getRecord", Method.POST);
        //    return await BaseRequest<HttpResponseMessage>(request, "getRecord", true);
        //}

        public static async Task<IRestResponse<ResponseObject<Post>>> GetAllPosts()
        {
            var request = new RestRequest("Record/getRecord", Method.GET);
            return await BaseRequest<ResponseObject<Post>> (request, "getRecord", true);
        }

        public static async Task<IRestResponse<HttpResponseMessage>> UploadPost(Post Post)
        {
            var request = new RestRequest("Record/uploadRecord", Method.POST);
            request.AddParameter("user_id", Post.id);
            request.AddParameter("file", Post.file);
            request.AddParameter("longitude", Post.longitude);
            request.AddParameter("latitude", Post.latitude);
            return await BaseRequest<HttpResponseMessage>(request, "uploadRecord", true);
        }

        //public static async Task<IRestResponse<HttpResponseMessage>> UserRegistration(RegistrationRequest registrationRequest)
        //{
        //    var request = new RestRequest("user/signup", Method.POST);
        //    request.AddJsonBody(registrationRequest);
        //    return await BaseRequest<HttpResponseMessage>(request, "signup", true);
        //}

        //public static async Task<IRestResponse<HttpResponseMessage>> SocialLoginUserRegistration(RegistrationRequest socialLoginRequest)
        //{
        //    var request = new RestRequest("user/social_login", Method.POST);
        //    request.AddJsonBody(socialLoginRequest);
        //    return await BaseRequest<HttpResponseMessage>(request, "social_login", true);
        //}

        //public static async Task<IRestResponse<List<CategoryItem>>> GetCategoryList()
        //{
        //    var request = new RestRequest("user/categories", Method.GET);
        //    return await BaseRequest<List<CategoryItem>>(request, "categories", true);
        //}

        //public static async Task<IRestResponse<List<QuoteDetail>>> GetCategoryListData(int CategoryId, RecordRequest recordRequest)
        //{
        //    var request = new RestRequest("user/categories_data", Method.GET);
        //    request.AddParameter("Categoryid", CategoryId);
        //    request.AddParameter("Records", recordRequest.Records);
        //    request.AddParameter("Pageno", recordRequest.Pageno);
        //    request.AddParameter("Userid", AppConstant.UserId);
        //    return await BaseRequest<List<QuoteDetail>>(request, "categories_data", true);
        //}

        //public static async Task<IRestResponse<List<QuoteDetail>>> GetCategoryListAllData(RecordRequest recordRequest, bool isFavoriteTapped)
        //{
        //    var apiCall = (isFavoriteTapped ? "user/my_favorite" : "user/all_categories_data");
        //    var request = new RestRequest(apiCall, Method.GET);
        //    request.AddParameter("Records", recordRequest.Records);
        //    request.AddParameter("Pageno", recordRequest.Pageno);
        //    request.AddParameter("Userid", AppConstant.UserId);
        //    return await BaseRequest<List<QuoteDetail>>(request, "all_categories_data", true);
        //}

        //public static async Task<IRestResponse<HttpResponseMessage>> EditUserData(User userdata)
        //{
        //    var request = new RestRequest("user/update_profile", Method.POST);
        //    request.AddJsonBody(userdata);
        //    return await BaseRequest<HttpResponseMessage>(request, "update_profile", true);
        //}

        //public static async Task<IRestResponse<HttpResponseMessage>> GetUserProfile(int Userid)
        //{
        //    var request = new RestRequest("user/get_profile", Method.POST);
        //    request.AddParameter("Userid", Userid);
        //    return await BaseRequest<HttpResponseMessage>(request, "get_profile", false);
        //}

        ///// <summary>
        ///// check internet connection availabilty
        ///// </summary>
        //public static async Task<bool> CheckInternetConnection()
        //{
        //    TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
        //    try
        //    {
        //        var IsInternetAvailable = await CrossConnectivity.Current.IsRemoteReachable("http://www.google.co.in", msTimeout: 10000);
        //        if (IsInternetAvailable)
        //        {
        //            tcs.SetResult(true);
        //        }
        //        else
        //        {
        //            tcs.SetResult(false);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        tcs.SetException(new Exception { Source = "Internet connection not available" });
        //    }
        //    return tcs.Task.Result;
        //}

        //public static async Task<IRestResponse<HttpResponseMessage>> GetPostLiked(int UserId, int PostId)
        //{
        //    var request = new RestRequest("user/like_dislike", Method.POST);
        //    request.AddParameter("Userid", UserId);
        //    request.AddParameter("Postid", PostId);
        //    return await BaseRequest<HttpResponseMessage>(request, "like_dislike", true);
        //}

        //public static async Task<IRestResponse<List<CommentDetail>>> GetPostComment(int PostId, RecordRequest recordRequest)
        //{
        //    var request = new RestRequest("user/get_comments", Method.POST);
        //    request.AddParameter("Postid", PostId);
        //    request.AddParameter("Records", recordRequest.Records);
        //    request.AddParameter("Pageno", recordRequest.Pageno);
        //    return await BaseRequest<List<CommentDetail>>(request, "get_comments", true);
        //}

        //public static async Task<IRestResponse<HttpResponseMessage>> FavoritePost(int UserId, int PostId)
        //{
        //    var request = new RestRequest("user/favorite_unfavorite", Method.POST);
        //    request.AddParameter("Userid", UserId);
        //    request.AddParameter("Postid", PostId);
        //    return await BaseRequest<HttpResponseMessage>(request, "favorite_unfavorite", true);
        //}



        //public static async Task<IRestResponse<HttpResponseMessage>> PostComment(int UserId, int PostId, string Comment)
        //{
        //    try
        //    {
        //        var request = new RestRequest("user/post_comments", Method.POST);
        //        request.AddParameter("Userid", UserId);
        //        request.AddParameter("Postid", PostId);
        //        request.AddParameter("Comment", Comment);
        //        request.AddParameter("Datetime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        //        return await BaseRequest<HttpResponseMessage>(request, "post_comments", true);
        //    }
        //    catch (Exception ex)
        //    {
        //        var t = ex.Message;
        //        throw;
        //    }
        //}

    }
}
