using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GoogleRecaptchaImplement.HTMLHelpers
{
    public static class GoogleCatchaValidationMessageHelper
    {

        public static IHtmlString GoogleCatchaValidationMessage(this HtmlHelper helper, string errorMessage) {
            var invalidCaptchaObj = helper.ViewContext.Controller.TempData["InvalidCaptcha"];
            var invalidCaptcha = invalidCaptchaObj?.ToString();
            if (string.IsNullOrWhiteSpace(invalidCaptcha))
                return MvcHtmlString.Create("");
            var mvcHtmlString = new TagBuilder("span")
            {
                Attributes =
                {
                new KeyValuePair<string, string>("class", "text text-danger")
                },
                InnerHtml = errorMessage ?? invalidCaptcha
            };

            return MvcHtmlString.Create(mvcHtmlString.ToString(TagRenderMode.Normal));
        }
    }
}