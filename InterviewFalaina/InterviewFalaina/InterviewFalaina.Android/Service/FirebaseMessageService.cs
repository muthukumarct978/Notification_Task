using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidX.Core.App;
using Firebase.Messaging;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace InterviewFalaina.Droid.Service
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class FirebaseMessageService : FirebaseMessagingService
    {
        const string TAG = "MyFirebaseMsgService";
        public const string PRIMARY_CHANNEL = "default";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        [Obsolete]
        public override void OnMessageReceived(RemoteMessage message)
        {
            try
            {
                SendNotifications(message);
            }
            catch (Exception ex)
            {
                Log.Debug(TAG, "Ex Message" + ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        [Obsolete]
        public void SendNotifications(RemoteMessage message)
        {
            try
            {
                NotificationManager notificationManager = (NotificationManager)GetSystemService(NotificationService);
                int notificationid = getId();
                var push = new Intent();
                var pendingIntent = PendingIntent.GetActivity(this, 0,push, PendingIntentFlags.CancelCurrent);
                NotificationCompat.Builder notification;
                if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
                {
                    var chan1 = new NotificationChannel(PRIMARY_CHANNEL,
                     new Java.Lang.String("Primary"), NotificationImportance.High);
                    //chan1.LightColor = (int)Color.Green;
                    notificationManager.CreateNotificationChannel(chan1);
                    notification = new NotificationCompat.Builder(this, PRIMARY_CHANNEL);
                }
                else
                {
                    notification = new NotificationCompat.Builder(this);
                }
                notification.SetContentIntent(pendingIntent)
                         .SetContentTitle(message.GetNotification().Title)
                         .SetContentText(message.GetNotification().Body)
                         .SetLargeIcon(BitmapFactory.DecodeResource(Resources, Resource.Drawable.notification_bg_normal))
                         .SetSmallIcon(Resource.Drawable.notification_bg_normal)
                         .SetStyle(new NotificationCompat.BigTextStyle())
                         .SetPriority(NotificationCompat.PriorityHigh)
                         .SetAutoCancel(true);
                notificationManager.Notify(notificationid, notification.Build());
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
        int getId()
        {
            var random = new Random();
            int randomnumber = random.Next(1, 101);
            return randomnumber;
        }
    }
}