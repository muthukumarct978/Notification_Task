using InterviewFalaina.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace NotificationWebAPI.Controllers
{
    public class NotificationController : ApiController
    {
        // GET: Notification
        [System.Web.Http.HttpPost]
        public ResponseModel Post([FromBody] NotificationModel notification)
        {
            ResponseModel responseModel = new ResponseModel();
            var httpContent = JsonConvert.SerializeObject(notification);
            var client = new HttpClient();
            var authorization = string.Format("key={0}", "AAAACZFz9Jw:APA91bG49wkm2gd3OwWw6gPykEmxaxjEWuK5js4kTxRA93QbxN8WyTGjrnBF-Zd0JSIlxmrHVN0P7u2tEoMKrsnnlA7Ed4c4tg_i0Png1SCDmANxlE-j3kCdXuWDOKb6tkyOIHUpErRZ");
            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", authorization);
            var stringContent = new StringContent(httpContent);
            stringContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            string uri = "https://fcm.googleapis.com/fcm/send";
            var response = client.PostAsync(uri, stringContent).ConfigureAwait(false).GetAwaiter().GetResult().StatusCode;
            if (response == System.Net.HttpStatusCode.OK)
            {
                responseModel.resp = "Notification Sent, Please Check your Mobile device!";
                //Console.WriteLine("Notification Sent!");
                return responseModel;
            }
            else
            {
                responseModel.resp = "Something went wrong, Have to check!";
                //Console.WriteLine("Something went wrong, Have to check!");
                return responseModel;
            }
        }
    }
}