using InterviewFalaina.Configuration;
using InterviewFalaina.Model;
using Newtonsoft.Json;
using Plugin.DeviceInfo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace InterviewFalaina
{
    public partial class MainPage : ContentPage
    {
        #region Construtor
        public MainPage()
        {
            InitializeComponent();
        }
        #endregion Construtor

        #region Events
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSend_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(entryTitle.Text) && !string.IsNullOrEmpty(entryBody.Text) )
            {
                var result = SendNotification();
                DisplayAlert("Success", "Notification Sent, Please Check your Mobile device!", "Done");

            }
            else
                DisplayAlert("Warning", "Please fill all the field!", "OK");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSend2_Clicked(object sender, EventArgs e)
        {
            SendviaWebAPINotification();
        }
        #endregion Events

        #region Methods
        /// <summary>
        /// 
        /// </summary>
        private async void SendviaWebAPINotification()
        {
            // Please add your local ip Address
            var uri = new Uri(GlobalConfig.WebApiURl);
            try
            {
                NotificationModel notificationModel = new NotificationModel()
                {
                    to = (string)Application.Current.Properties["RefreshToken"],
                    notification = new Notification()
                    {
                        body = entryBody.Text,
                        title = entryTitle.Text
                    }
                };
                var data = JsonConvert.SerializeObject(notificationModel);
                var content = new StringContent(data, Encoding.UTF8, "application/json");
                using (var client = new HttpClient())
                {
                    var response = await client.PostAsync(uri, content).ConfigureAwait(false);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var details = await response.Content.ReadAsStringAsync();
                        //var result = JsonConvert.DeserializeObject<ResponseModel>(details);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private async Task<string> SendNotification()
        {
            NotificationModel notificationModel = new NotificationModel()
            {
                to = (string)Application.Current.Properties["RefreshToken"],
                notification = new Notification()
                {
                    body = entryBody.Text,
                    title = entryTitle.Text
                }
            };
            var httpContent = JsonConvert.SerializeObject(notificationModel);
            var client = new HttpClient();
            var authorization = string.Format("key={0}", GlobalConfig.Key);
            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", authorization);
            var stringContent = new StringContent(httpContent);
            stringContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            string uri = GlobalConfig.FirebaseURL;
            var response = await client.PostAsync(uri, stringContent).ConfigureAwait(false);
            var result =await response.Content.ReadAsStringAsync();
            var reult = JsonConvert.DeserializeObject<string>(result);
            if (response.IsSuccessStatusCode)
            {
                return "Notification Sent, Please Check your Mobile device!";
            }
            else
            {
                return "Something went wrong in API Servie!";
            }
           
        }
        #endregion Methods

    }
}
