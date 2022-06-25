using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GpmLoginApiV2Sample.Libs
{
    public class GPMLoginAPI
    {
        private string _apiUrl;

        private const string API_START_PATH = "/v2/start";
        private const string API_CREATE_PATH = "/v2/create";
        private const string API_PROFILE_LIST_PATH = "/v2/profiles";
        private const string API_DELETE_PATH = "/v2/delete";

        /// <summary>
        /// Init with API_URL (get on GPM-LOGIN app)
        /// </summary>
        /// <param name="apiUrl">Eg: 127.0.0.1:57172</param>
        public GPMLoginAPI(string apiUrl)
        {
            _apiUrl = apiUrl;
        }

        /// <summary>
        /// Start a profile by ID
        /// </summary>
        /// <param name="profileId"></param>
        /// <param name="remoteDebugPort"></param>
        /// <param name="addinationArgs"></param>
        /// <returns>View API document</returns>
        public JObject Start(string profileId, int remoteDebugPort = 0, string addinationArgs = "")
        {
            // Make api url
            string url = _apiUrl + API_START_PATH + $"?profile_id={profileId}";
            if (remoteDebugPort > 0) url += $"&remote_debug_port={remoteDebugPort}";
            if (!string.IsNullOrEmpty(addinationArgs)) url += $"&addination_args={addinationArgs}";

            // Call api
            string resp = httpRequest(url);
            return resp != null ? JObject.Parse(resp) : null;
        }

        /// <summary>
        /// Create profile on GPMLogin
        /// </summary>
        /// <param name="name"></param>
        /// <param name="proxy"></param>
        /// <param name="isNoiseCanvas"></param>
        /// <returns>View API document</returns>
        public JObject Create(string name, string proxy = "", bool isNoiseCanvas = false)
        {
            // Make api url
            string url = _apiUrl + API_CREATE_PATH + $"?name={name}&proxy={proxy}";
            if (isNoiseCanvas) url += $"&canvas=on";

            // Call api
            string resp = httpRequest(url);
            return resp != null ? JObject.Parse(resp) : null;
        }

        public void Delete(string profileId, int mode = 2)
        {
            // Make api url
            string url = _apiUrl + API_DELETE_PATH + $"?profile_id={profileId}&mode={mode}";

            // Call api
            httpRequest(url);
        }

        public List<JObject> GetProfiles()
        {
            // Make api url
            string url = _apiUrl + API_PROFILE_LIST_PATH;

            // Call api
            string resp = httpRequest(url);
            return resp != null ? JsonConvert.DeserializeObject<List<JObject>>(resp) : null;
        }

        #region Helpers
        private string httpRequest(string url)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            return reader.ReadToEnd();
                        }
                    }
                }
            }
            catch
            {
                return null;
            }
        }
        #endregion
    }
}
