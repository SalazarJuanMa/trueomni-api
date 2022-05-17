
namespace API.Constants
{
    /// <summary>
    /// Colorado Constants
    /// </summary>
    public class APPConstants
    {
        private const string NAME = "TrueOmni";
        private const string NAME_SHORT = "Domain";

        /// <summary>
        /// The accept
        /// </summary>
        public const string ACCEPT = "Accept";
        /// <summary>
        /// The allowed origin
        /// </summary>
        public const string ALLOWED_ORIGIN = "ALLOWED_ORIGIN";
        /// <summary>
        /// The allowed original policy
        /// </summary>
        public const string ALLOWED_ORIGINAL_POLICY = "_allowedOriginPolicy";
        /// <summary>
        /// The API
        /// </summary>
        public static string API = NAME.ToLower();
        /// <summary>
        /// The API base URL
        /// </summary>
        public const string API_BASE_URL = "API_BASE_URL";
        /// <summary>
        /// The application
        /// </summary>
        public static string APP = NAME.ToLower() + "app";
        /// <summary>
        /// The content type
        /// </summary>
        public const string CONTENT_TYPE = "Content-Type";
        /// <summary>
        /// The content type json
        /// </summary>
        public const string CONTENT_TYPE_JSON = "application/json";
        /// <summary>
        /// The controller route
        /// </summary>
        public const string CONTROLLER_ROUTE = "{controller}/{action=Index}/{id?}";
        /// <summary>
        /// The cors
        /// </summary>
        public const string CORS = "Cors";
        /// <summary>
        /// The default
        /// </summary>
        public static string DEFAULT = NAME_SHORT;
        /// <summary>
        /// The health check
        /// </summary>
        public const string HEALTH_CHECK = "/health";

        /// <summary>
        /// The route
        /// </summary>
        public const string ROUTE = NAME_SHORT + "/" + VERSION + "/[controller]";

        /// <summary>
        /// The version
        /// </summary>
        public const string VERSION = "V1";

        /// <summary>
        /// Class Swagger.
        /// </summary>
        public static class Swagger
        {
            /// <summary>
            /// The contact URL
            /// </summary>
            public const string CONTACT_URL = "https://contact-us/";
            /// <summary>
            /// The description
            /// </summary>
            public static string DESCRIPTION = APPConstants.NAME + " API";
            /// <summary>
            /// The license URL
            /// </summary>
            public const string LICENSE_URL = "https://privacy/";
            /// <summary>
            /// The name
            /// </summary>
            public static string NAME = APPConstants.NAME.ToUpper() + " API V1";
            /// <summary>
            /// The policy
            /// </summary>
            public const string POLICY = "Privacy Policy";
            /// <summary>
            /// The support
            /// </summary>
            public const string SUPPORT = "Support TrueOmni";
            /// <summary>
            /// The term service URL
            /// </summary>
            public const string TERM_SERVICE_URL = "https://site-terms/";
            /// <summary>
            /// The title
            /// </summary>
            public static string TITLE = APPConstants.NAME + " Apps";
            /// <summary>
            /// The URL json
            /// </summary>
            public const string URL_JSON = "/swagger/v1/swagger.json";
            /// <summary>
            /// The version
            /// </summary>
            public const string VERSION = "v1";
            /// <summary>
            /// The email
            /// </summary>
            public const string EMAIL = "no-reply@trueomni.com";
            /// <summary>
            /// The Operations
            /// </summary>
            public const string OPERATIONS = "api";

            /// <summary>
            /// 
            /// </summary>
            public static class ResponseHeader
            {
                /// <summary>
                /// The location name
                /// </summary>
                public const string LOCATION_NAME = "Location";
                /// <summary>
                /// The location description
                /// </summary>
                public const string LOCATION_DESCRIPTION = "Location of the newly created resource";
                /// <summary>
                /// The date name
                /// </summary>
                public const string DATE_NAME = "Date";
                /// <summary>
                /// The date description
                /// </summary>
                public const string DATE_DESCRIPTION = "The UTC date/time at which the current rate limit window resets.";
                /// <summary>
                /// The content type
                /// </summary>
                public const string CONTENT_TYPE = "Content-Type";
                /// <summary>
                /// The content description
                /// </summary>
                public const string CONTENT_DESCRIPTION = "application/json";
                /// <summary>
                /// The string type
                /// </summary>
                public const string STRING_TYPE = "string";
            }
        }
    }
}
