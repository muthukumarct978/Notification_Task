using System;
using System.Diagnostics;
using Firebase.CloudMessaging;
using Foundation;
using InterviewFalaina.iOS.Service;
using UIKit;
using UserNotifications;

namespace InterviewFalaina.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate, IUNUserNotificationCenterDelegate, IMessagingDelegate
    {
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new App());
            RegisterForRemoteNotifications();
            Messaging.SharedInstance.Delegate = this;
            if (UNUserNotificationCenter.Current != null)
            {
                UNUserNotificationCenter.Current.Delegate = new UserNotificationCenterDelegate();
            }
            Firebase.Core.App.Configure();
            return base.FinishedLaunching(app, options);
        }
        /// <summary>
        /// To set Notification for Different version
        /// 
        /// </summary>
        private void RegisterForRemoteNotifications()
        {
            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
            {
                UNUserNotificationCenter.Current.Delegate = this;
                var authOptions = UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound;
                UNUserNotificationCenter.Current.RequestAuthorization(authOptions, async (granted, error) =>
                {
                    await System.Threading.Tasks.Task.Delay(500);
                });
            }
            else
            {
                // iOS version 9 or before
                var allNotificationTypes = UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound;
                var settings = UIUserNotificationSettings.GetSettingsForTypes(allNotificationTypes, null);
                UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
            }
            UIApplication.SharedApplication.RegisterForRemoteNotifications();
            Messaging.SharedInstance.ShouldEstablishDirectChannel = true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        /// <param name="token"></param>
        public override void RegisteredForRemoteNotifications(UIApplication application, NSData token)
        {
            var deviceToken = token.Description.Replace("<", "").Replace(">", "").Replace(" ", "");
            if (!string.IsNullOrEmpty(deviceToken))
            {
                Debug.WriteLine(token.Description);
                Debug.WriteLine(deviceToken);
                Messaging.SharedInstance.ApnsToken = deviceToken;
            }
        }
        /// <summary>
        ///  Indicates that a call to UIKit.UIApplication.RegisterForRemoteNotifications failed.
        /// </summary>
        /// <param name="application"></param>
        /// <param name="error"></param>
        public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
        {
            base.FailedToRegisterForRemoteNotifications(application, error);
            Console.WriteLine(@"Failed to register for remote notification {0}", error.Description);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        /// <param name="userInfo"></param>
        /// <param name="completionHandler"></param>
        public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
        {
            //Foreground
            completionHandler(UIBackgroundFetchResult.NewData);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="messaging"></param>
        /// <param name="fcmToken"></param>
        [Export("messaging: didReceiveRegistrationToken:")]
        public void DidReceiveRegistrationToken(Messaging messaging, string fcmToken)
        {
            Xamarin.Forms.Application.Current.Properties["RefreshToken"] = Messaging.SharedInstance.FcmToken ?? "";
            Xamarin.Forms.Application.Current.SavePropertiesAsync();
        }

    }
}