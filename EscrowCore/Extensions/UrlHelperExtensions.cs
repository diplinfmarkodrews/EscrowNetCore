using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EscrowCore.Controllers;

namespace Microsoft.AspNetCore.Mvc
{
    public static class UrlHelperExtensions
    {
        public static string EmailConfirmationLink(this IUrlHelper urlHelper, string userId, string code, string scheme)
        {
            return urlHelper.Action(
                action: nameof(AccountController.ConfirmEmail),
                controller: "Account",
                values: new { userId, code },
                protocol: scheme);
        }

        public static string ResetPasswordCallbackLink(this IUrlHelper urlHelper, string userId, string code, string scheme)
        {
            return urlHelper.Action(
                action: nameof(AccountController.ResetPassword),
                controller: "Account",
                values: new { userId, code },
                protocol: scheme);
        }
      
            /// <summary>
            /// Return a view path based on an action name and controller name
            /// </summary>
            /// <param name="url">Context for extension method</param>
            /// <param name="action">Action name</param>
            /// <param name="controller">Controller name</param>
            /// <returns>A string in the form "~/views/{controller}/{action}.cshtml</returns>
            public static string View(this IUrlHelper url, string action, string controller)
            {
                return string.Format("~/Views/{1}/{0}.cshtml", action, controller);
            }
        
    }
}
