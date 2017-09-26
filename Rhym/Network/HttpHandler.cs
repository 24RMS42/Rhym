using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using Xamarin.Forms;

namespace Rhym
{
    public class HttpHandler
    {
        private HttpClient httpClient;

        public HttpHandler()
        {
            httpClient = new HttpClient();
        }

        #region LoginAsync
        public async Task<bool> LoginAsync(string email, string password)
        {
            try
            {
                UserDialogs.Instance.ShowLoading("Logging in...");
                var client = new RestClient(Constants.BASE);
                var request = new RestRequest(Constants.LOGIN_API, Method.POST);
                var cancellationTokenSource = new CancellationTokenSource();

                request.AddHeader("content-type", "application/x-www-form-urlencoded");
                request.AddParameter("grant_type", "custom");
                request.AddParameter("emailaddress", email);
                request.AddParameter("password", password);
                IRestResponse response = await client.ExecuteTaskAsync(request, cancellationTokenSource.Token);

                UserDialogs.Instance.HideLoading();

                var result = response.Content;
                Console.WriteLine("login result:" + result);
                if (response != null && response.StatusCode == HttpStatusCode.OK)
                {
                    var jsonArray = JToken.Parse(result);
                    var token = jsonArray["access_token"].ToString();
                    Application.Current.Properties[Constants.TOKEN_KEY] = token;
                    App.Token = token;
                    Console.WriteLine("token: {0}", token);

                    return true;
                }
                else
                {
                    var jsonArray = JToken.Parse(result);
                    var exceptionMsg = jsonArray["error_description"].ToString();
                    await ParseError(exceptionMsg);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                UserDialogs.Instance.HideLoading();
                await ParseError("Something went wrong. Please try again");
                return false;
            }
        }
        #endregion

        #region SignupAsync
        public async Task<bool> SignupAsync(string firstName, string lastName, string email, string password)
        {
            try
            {
                UserDialogs.Instance.ShowLoading("Registering...");
                httpClient.BaseAddress = new Uri(Constants.BASE);
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Add("X-ApiKey", "cd9e1d2f-c933-406e-878b-d30fcf9a2db4");

                var request = new HttpRequestMessage(HttpMethod.Post, Constants.API_PATH + Constants.SIGNUP_API);

                var oJsonObject = new JObject();
                oJsonObject.Add("EmailAddress", email);
                oJsonObject.Add("Password", password);
                oJsonObject.Add("FirstName", firstName);
                oJsonObject.Add("LastName", lastName);
                request.Content = new StringContent(oJsonObject.ToString(),
                                                    Encoding.UTF8,
                                                    "application/json");

                var response = await httpClient.SendAsync(request);
                UserDialogs.Instance.HideLoading();

                var result = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine("signup result:" + result);
                if (response != null && response.StatusCode == HttpStatusCode.OK)
                {
                    var jsonArray = JToken.Parse(result);
                    var isSuccess = jsonArray["IsSuccessful"].Value<bool>();
                    Console.WriteLine("isSuccess: {0}", isSuccess);
                    var statusMsg = jsonArray["StatusMessage"].ToString();
                    if (isSuccess)
                        return true;
                    else
                    {
                        await ParseError(statusMsg);
                        return false;
                    }
                }
                else
                {
                    var jsonArray = JToken.Parse(result);
                    var exceptionMsg = jsonArray["ExceptionMessage"].ToString();
                    await ParseError(exceptionMsg);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                UserDialogs.Instance.HideLoading();
                await ParseError("Something went wrong. Please try again");
                return false;
            }
        }
        #endregion

        #region ActivateAsync
        public async Task<bool> ActivateAsync(string email, string activationcode)
        {
            try
            {
                UserDialogs.Instance.ShowLoading("Activating...");
                httpClient.BaseAddress = new Uri(Constants.BASE);
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Add("X-ApiKey", "axknghe234nh6h_=22mnna+25n1ajs$@6na");

                var request = new HttpRequestMessage(HttpMethod.Post, Constants.API_PATH + Constants.ACTIVATE_API);

                var oJsonObject = new JObject();
                oJsonObject.Add("EmailAddress", email);
                oJsonObject.Add("ActivationCode", activationcode);
                request.Content = new StringContent(oJsonObject.ToString(),
                                                    Encoding.UTF8,
                                                    "application/json");

                var response = await httpClient.SendAsync(request);
                UserDialogs.Instance.HideLoading();

                var result = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine("activation result:" + result);
                if (response != null && response.StatusCode == HttpStatusCode.OK)
                {
                    var jsonArray = JToken.Parse(result);
                    var isSuccess = jsonArray["IsSuccessful"].Value<bool>();
                    Console.WriteLine("isSuccess: {0}", isSuccess);
                    var statusMsg = jsonArray["StatusMessage"].ToString();
                    if (isSuccess)
                        return true;
                    else
                    {
                        await ParseError(statusMsg);
                        return false;
                    }
                }
                else
                {
                    var jsonArray = JToken.Parse(result);
                    var exceptionMsg = jsonArray["ExceptionMessage"].ToString();
                    await ParseError(exceptionMsg);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                UserDialogs.Instance.HideLoading();
                await ParseError("Something went wrong. Please try again");
                return false;
            }
        }
        #endregion

        #region GetPlayListAsync
        public async Task<ObservableCollection<SongModel>> GetPlayListAsync(string guid)
        {
            try
            {
                UserDialogs.Instance.ShowLoading("Getting playlist...");
                httpClient.BaseAddress = new Uri(Constants.BASE);
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.Token);

                var request = new HttpRequestMessage(HttpMethod.Get, Constants.API_PATH + Constants.PARTY_API + guid + "/playlist");
                var response = await httpClient.SendAsync(request);

                UserDialogs.Instance.HideLoading();

                var result = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine("get playlist result:" + result);

                var songList = new ObservableCollection<SongModel>();

                if (response != null && response.StatusCode == HttpStatusCode.OK)
                {
                    var jsonArray = JToken.Parse(result);
                    foreach (var item in jsonArray)
                    {
                        var SongModel = JsonConvert.DeserializeObject<SongModel>(item.ToString());
                        songList.Add(SongModel);   
                    }

                    return songList;
                }
                else
                {
                    var jsonArray = JToken.Parse(result);
                    var exceptionMsg = jsonArray["ExceptionMessage"].ToString();
                    await ParseError(exceptionMsg);
                    return null;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                UserDialogs.Instance.HideLoading();
                await ParseError("Something went wrong. Please try again");
                return null;
            }
        }
        #endregion

        #region GetMusicListAsync
        public async Task<ObservableCollection<SongModel>> GetMusicListAsync()
        {
            try
            {
                UserDialogs.Instance.ShowLoading("Getting music list...");
                httpClient.BaseAddress = new Uri(Constants.BASE);
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.Token);

                var request = new HttpRequestMessage(HttpMethod.Get, Constants.API_PATH + Constants.MUSICLIST_API);
                var response = await httpClient.SendAsync(request);

                UserDialogs.Instance.HideLoading();

                var result = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine("get musiclist result:" + result);

                var musicList = new ObservableCollection<SongModel>();

                if (response != null && response.StatusCode == HttpStatusCode.OK)
                {
                    var jsonArray = JToken.Parse(result);
                    foreach (var item in jsonArray)
                    {
                        var SongModel = JsonConvert.DeserializeObject<SongModel>(item.ToString());
                        musicList.Add(SongModel);   
                    }

                    return musicList;
                }
                else
                {
                    var jsonArray = JToken.Parse(result);
                    var exceptionMsg = jsonArray["ExceptionMessage"].ToString();
                    await ParseError(exceptionMsg);
                    return null;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                UserDialogs.Instance.HideLoading();
                await ParseError("Something went wrong. Please try again");
                return null;
            }
        }
        #endregion

        #region CreatePlayListAsync
        public async Task<bool> CreatePlayListAsync(string guid, ObservableCollection<SongModel> songList)
        {
            ObservableCollection<SongModel> _songList = new ObservableCollection<SongModel>();
            _songList = songList;
            try
            {
                UserDialogs.Instance.ShowLoading("Updating playlist...");
                httpClient.BaseAddress = new Uri(Constants.BASE);
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.Token);

                var request = new HttpRequestMessage(HttpMethod.Post, Constants.API_PATH + Constants.PARTY_API + guid + "/playlist");

                request.Content = new StringContent(JToken.FromObject(_songList).ToString(),
                                                    Encoding.UTF8,
                                                    "application/json");
                
                var response = await httpClient.SendAsync(request);

                UserDialogs.Instance.HideLoading();

                var result = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine("create playlist result:" + result);

                if (response != null && response.StatusCode == HttpStatusCode.OK)
                {
                    var jsonArray = JToken.Parse(result);
                    var isSuccess = jsonArray["IsSuccessful"].Value<bool>();
                    Console.WriteLine("isSuccess: {0}", isSuccess);
                    var statusMsg = jsonArray["StatusMessage"].ToString();
                    if (isSuccess)
                        return true;
                    else
                    {
                        await ParseError(statusMsg);
                        return false;
                    }
                }
                else
                {
                    var jsonArray = JToken.Parse(result);
                    var exceptionMsg = jsonArray["ExceptionMessage"].ToString();
                    await ParseError(exceptionMsg);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                UserDialogs.Instance.HideLoading();
                await ParseError("Something went wrong. Please try again");
                return false;
            }
        }
        #endregion

        #region UploadImage
        /*public async Task<string> UploadImage(byte[] image)
        {
            string resultURL = "";
            try
            {
                UserDialogs.Instance.ShowLoading("Uploading photo...");

                var cancellationTokenSource = new CancellationTokenSource();
                RestClient restClient = new RestClient(Constants.BASE);
                RestRequest restRequest = new RestRequest(Constants.API_PATH + Constants.AVATAR_API, Method.POST);

                restRequest.AddHeader("Authorization", "Bearer " + App.Token);
                restRequest.AddHeader("Content-Type", "multipart/form-data");
                restRequest.AddFile("test", image, "test", null);
                IRestResponse response = await restClient.ExecuteTaskAsync(restRequest, cancellationTokenSource.Token);

                string resContent = response.Content;

                UserDialogs.Instance.HideLoading();

                Console.WriteLine("upload image result:" + resContent);
                Application.Current.Properties[Constants.AVATAR_URL_KEY] = resContent;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                UserDialogs.Instance.HideLoading();
                resultURL = null;
            }

            return resultURL;
        }*/
        public async Task<string> UploadImage(byte[] image, string filename = "")
        {
            Console.WriteLine("image size:" + image.Length);
            if (image.Length > 3780000)
            {
                await UserDialogs.Instance.AlertAsync("Too large image", "Oh, sorry!", "OK");
                return "";
            }

            string resultURL = "";
            try
            {
                UserDialogs.Instance.ShowLoading("Uploading photo...");
                var byteContent = new ByteArrayContent(image);
                byteContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                    FileName = "test.png",
                    Name = "test.png"
                };
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("multipart/form-data");

                var multi = new MultipartFormDataContent();
                multi.Add(byteContent);
                httpClient.BaseAddress = new Uri(Constants.BASE);
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.Token);

                HttpResponseMessage result = await httpClient.PostAsync(Constants.API_PATH + Constants.AVATAR_API, multi);
                string resContent = await result.Content.ReadAsStringAsync();

                UserDialogs.Instance.HideLoading();

                Console.WriteLine("upload image result:" + resContent);
                Application.Current.Properties[Constants.AVATAR_URL_KEY] = resContent;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                UserDialogs.Instance.HideLoading();
                resultURL = null;
            }

            return resultURL;
        }
        #endregion

        #region GetAvatarAsync
        public async Task<string> GetAvatarAsync()
        {
            try
            {
                httpClient.BaseAddress = new Uri(Constants.BASE);
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.Token);

                var request = new HttpRequestMessage(HttpMethod.Get, Constants.API_PATH + Constants.AVATAR_API + "/" + Constants.GETURL);
                var response = await httpClient.SendAsync(request);

                UserDialogs.Instance.HideLoading();

                var result = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine("get avatar result:" + result);

                result = result.Replace("\"", "");
                Application.Current.Properties[Constants.AVATAR_URL_KEY] = result;
                return result;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                UserDialogs.Instance.HideLoading();
                await ParseError("Something went wrong. Please try again");
                return null;
            }
        }
        #endregion

        async Task ParseError(string error_message)
        {
            await UserDialogs.Instance.AlertAsync(error_message, "Oh, sorry!", "OK");
        }
    }
}
