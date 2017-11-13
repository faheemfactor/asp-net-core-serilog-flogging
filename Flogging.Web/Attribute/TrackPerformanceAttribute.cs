namespace Flogging.Web.Attribute
{
    using Flogging.Core;
    using Microsoft.AspNetCore.Mvc.Filters;

    public class TrackPerformanceAttribute : ActionFilterAttribute
    {
        public string productName { get; set; }
        public string layerName { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string userId, userName, location;
            var dict = Helpers.GetWebFloggingData(filterContext.HttpContext.Request, out userId, out userName, out location);

            var type = filterContext.HttpContext.Request.Method;
            var perfName = $"{filterContext.ActionDescriptor.DisplayName}_{type}";

            var stopwatch = new PerfTracker(perfName, userId, userName, location, productName, layerName);
            filterContext.HttpContext.Items["Stopwatch"] = stopwatch;
            base.OnActionExecuting(filterContext);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var stopwatch =  (PerfTracker)filterContext.HttpContext.Items["Stopwatch"];
            if (stopwatch != null)
                stopwatch.Stop();

            base.OnActionExecuted(filterContext);
        }
    }
}
