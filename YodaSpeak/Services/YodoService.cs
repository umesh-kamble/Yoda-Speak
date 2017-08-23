using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using unirest_net.http;
namespace YodaSpeak.Services
{
    public class YodoService
    {
        private static YodoService authInstance;
        public static YodoService Instance
        {
            get
            {
                if (authInstance == null)
                    authInstance = new YodoService();

                return authInstance;
            }
        }
        public string GetStringFromApi(string sentence)
        {
            string errorMsg = "";
            try
            {
                var response = Unirest.get("https://yoda.p.mashape.com/yoda?sentence=" + sentence)
                    .header("X-Mashape-Key", "GED6XhwlBhmshzR1JzaQqkQpqhBtp13gS5qjsnkCIIZHIXrNa7")
                    .header("Accept", "text/plain")
                    .asString();

                if (response.Code == 200)
                {
                    return response.Body;
                }
                errorMsg = "Error";
                return errorMsg;
            }
            catch (Exception ex)
            {
                return errorMsg = "Error";
            }
        }
    }
}
