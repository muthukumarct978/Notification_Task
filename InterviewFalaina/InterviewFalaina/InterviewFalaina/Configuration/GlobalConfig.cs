using System;
using System.Collections.Generic;
using System.Text;

namespace InterviewFalaina.Configuration
{
    public class GlobalConfig
    {
        /// <summary>
        ///  API Key ---> Dummy Api Key
		/// The key in APIKEY notepad file
        /// </summary>
        public static string Key = "SIlxmrHVN0P7u2tEoMKrsnnlA7Ed4c4tg_i0Png1SCDmANxlE-j3kCdXuWDOKb6tkyOIHUpErRZ";

        /// <summary>
        /// Firebase URL
        /// </summary>
        public static string FirebaseURL = "https://fcm.googleapis.com/fcm/send";

        /// <summary>
        /// Here add local WebAPI address i.e Localhost
        /// </summary>
        public static string WebApiURl = "http://192.168.00.00:44300/api/Notification/Post"; 
    }
}
