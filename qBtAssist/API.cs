using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace qBtAssist
{
    class API
    {
        private static HttpClient client = new HttpClient();

        /// <summary>
        /// Initializes the API.
        /// </summary>
        /// <param name="baseAddress">Host address.</param>
        /// <param name="timeout">Time before timeout.</param>
        public static void Initialize(string baseAddress, int timeout = 100)
        {
            baseAddress = baseAddress.Replace("\\", "/");
            if (baseAddress[baseAddress.Length - 1] != '/')
                baseAddress += "/";

            Uri.TryCreate(baseAddress, UriKind.Absolute, out Uri uri);

            if (uri == null)
                throw new UriFormatException("Format of base address is invalid!");

            client.BaseAddress = new Uri(baseAddress);
            client.Timeout = TimeSpan.FromSeconds(timeout);
        }

        /// <summary>
        /// Logs in to qBittorrent.
        /// </summary>
        /// <param name="username">Username.</param>
        /// <param name="password">Password.</param>
        /// <returns>True if successful, false if wrong username/password, null if unreachable.</returns>
        public static async Task<bool?> Login(string username, string password)
        {
            var content = new[]
            {
            new KeyValuePair<string, string>("username", username),
            new KeyValuePair<string, string>("password", password)
        };

            var reply = await Post(client, "/login", new FormUrlEncodedContent(content));

            if (reply == null)
                return null;

            var result = await reply.Content.ReadAsStringAsync();

            if (result == "Ok.")
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Sets global upload speed limit.
        /// </summary>
        /// <param name="limit">Global upload speed limit in bytes/second.</param>
        public static async Task SetGlobalUploadSpeedLimit(long limit)
        {
            var content = new[]
            {
            new KeyValuePair<string, string>("limit", limit.ToString())
        };

            await Post(client, "/command/setGlobalUpLimit", new FormUrlEncodedContent(content));
        }

        /// <summary>
		/// Sets global download speed limit.
		/// </summary>
		/// <param name="limit">Global download speed limit in bytes/second.</param>
		public static async Task SetGlobalDownloadSpeedLimit(long limit)
        {
            var content = new[]
            {
                new KeyValuePair<string, string>("limit", limit.ToString())
            };

            await Post(client, "/command/setGlobalDlLimit", new FormUrlEncodedContent(content));
        }






        private static async Task<HttpResponseMessage> Get(HttpClient client, string requestUri)
        {
            try
            {
                var reply = await client.GetAsync(requestUri);
                if (!reply.IsSuccessStatusCode)
                    throw new QBTException(reply.StatusCode, await reply.Content.ReadAsStringAsync());
                return reply;
            }

            catch (HttpRequestException)
            {
                return null;
            }
        }

        private static async Task<HttpResponseMessage> Post(HttpClient client, string requestUri, FormUrlEncodedContent content = null)
        {
            try
            {
                var reply = await client.PostAsync(requestUri, content);
                if (!reply.IsSuccessStatusCode)
                    throw new QBTException(reply.StatusCode, await reply.Content.ReadAsStringAsync());
                return reply;
            }

            catch (HttpRequestException)
            {
                return null;
            }
        }

        private static async Task<HttpResponseMessage> Post(HttpClient client, string requestUri, MultipartFormDataContent content)
        {
            try
            {
                var reply = await client.PostAsync(requestUri, content);
                if (!reply.IsSuccessStatusCode)
                    throw new QBTException(reply.StatusCode, await reply.Content.ReadAsStringAsync());
                return reply;
            }

            catch (HttpRequestException)
            {
                return null;
            }
        }

        private static string ListToString(List<string> stringList, char separator)
        {
            string returnString = "";
            foreach (string element in stringList)
                returnString += element + separator;
            returnString = returnString.Remove(returnString.Length - 1);

            return returnString;
        }

        private static string ListToString(List<Uri> uris, char separator)
        {
            string returnString = "";
            foreach (Uri element in uris)
                returnString += element.ToString() + separator;
            returnString = returnString.Remove(returnString.Length - 1);

            return returnString;
        }

     }
    
}
