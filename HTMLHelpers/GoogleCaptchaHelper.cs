using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GoogleRecaptchaImplement.HTMLHelpers
{
    public static class GoogleCaptchaHelper
    {
        public static IHtmlString GoogleCaptcha(this HtmlHelper helper) {

             string publicKey = System.Web.Configuration.WebConfigurationManager.AppSettings.Get("GoogleRecaptchaSiteKey");
            var mvcHtmlString = new TagBuilder("div")
            {
                Attributes ={
                     new KeyValuePair<string, string>("class", "g-recaptcha"),
                     new KeyValuePair<string, string>("data-sitekey", publicKey)
                 }
            };
            const string googleCaptchaScript = "<script src='https://www.google.com/recaptcha/api.js'></script>";
            var renderedCaptcha = mvcHtmlString.ToString(TagRenderMode.Normal);

            // Use of interpolated string C#6 to add the google captch api script in the html helper in itself
            return MvcHtmlString.Create($"{googleCaptchaScript}{renderedCaptcha}");
        }
    }
}