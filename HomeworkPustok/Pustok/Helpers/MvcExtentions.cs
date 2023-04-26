using Microsoft.AspNetCore.Mvc.Rendering;

namespace Pustok.Helpers
{
	public static class MvcExtentions
	{
		public static string ActiveClass(this IHtmlHelper htmlHelper, string controller, string action, string className = "active")
		{
			var currentController = htmlHelper.ViewContext.RouteData.Values["controller"] as string;
			var currentAction = htmlHelper.ViewContext.RouteData.Values["action"] as string;

			return (currentController == controller && currentAction == action) ? className : "";
		}
	}
}
