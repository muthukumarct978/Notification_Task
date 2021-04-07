using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Iid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InterviewFalaina.Droid.Service
{
    /// <summary>
    /// Refresh Token
    /// </summary>
    [Service]
    [IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
    [Obsolete]
    public class FirebaseInstanceService : FirebaseInstanceIdService
    {
        [Obsolete]
        public override void OnTokenRefresh()
        {
            // Get updated InstanceID token.
            var refreshToken = FirebaseInstanceId.Instance.Token;
            Xamarin.Forms.Application.Current.Properties["RefreshToken"] = FirebaseInstanceId.Instance.Token ?? "";
            Xamarin.Forms.Application.Current.SavePropertiesAsync();
        }
    }
}