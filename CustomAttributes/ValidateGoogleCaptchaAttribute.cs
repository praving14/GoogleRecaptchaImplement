using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.IO;
using System.Net;

namespace GoogleRecaptchaImplement.CustomAttributes
{
    public class ValidateGoogleCaptchaAttribute :  ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string secretKey = System.Web.Configuration.WebConfigurationManager.AppSettings.Get("GoogleRecaptchaSecretKey");
            var captchaResponse = filterContext.HttpContext.Request.Form["g-recaptcha-response"];
            Console.Out.WriteLine("Captcha Response" + captchaResponse);
            if (string.IsNullOrWhiteSpace(captchaResponse)) {
                AddErrorAndRedirectToGetAction(filterContext);
            }

            var client = new WebClient();
            var result = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secretKey, captchaResponse));
            Console.Out.WriteLine("Result" + result);
            var validateResult = JsonConvert.DeserializeObject<ReCaptchaResponse>(result.ToString());
            Console.Out.WriteLine("Validate Result" + validateResult);
            if (!validateResult.Success) AddErrorAndRedirectToGetAction(filterContext, validateResult.ErrorCodes[0]);

            base.OnActionExecuting(filterContext);
        }

        private static void AddErrorAndRedirectToGetAction(ActionExecutingContext filterContext, string Error = "Invalid Captcha!")
        {

            filterContext.Controller.TempData["InvalidCaptcha"] = getCaptchaErrorMessage(Error);
            filterContext.Result = new RedirectToRouteResult(filterContext.RouteData.Values);
        }

        private static string getCaptchaErrorMessage(string errorCode) {
            string errorMessage = "";
            switch (errorCode) {
                case "missing-input-secret":
                    errorMessage = "The secret parameter is missing.";
                    break;
                case "invalid-input-secret":
                    errorMessage = "The secret parameter is invalid or malformed.";
                    break;
                case "missing-input-response":
                    errorMessage = "The response parameter is missing.";
                    break;
                case "invalid-input-response":
                    errorMessage = "The response parameter is invalid or malformed.";
                    break;
                case "bad-request":
                    errorMessage = "The request is invalid or malformed.";
                    break;
                default:
                    errorMessage = "Invalid Captcha!";
                    break;
            }
               
            return errorMessage;
        }
    }

    internal class ReCaptchaResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("challenge_ts")]
        public string ValidatedDateTime { get; set; }

        [JsonProperty("hostname")]
        public string HostName { get; set; }

        [JsonProperty("error-codes")]
        public List<string> ErrorCodes { get; set; }
    }
}