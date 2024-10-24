using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace FormsCreator.Application.Attributes
{
    public class ViewLayoutAttribute(string layoutName) : ResultFilterAttribute
    {
        private readonly string _layoutName = layoutName;

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            base.OnResultExecuting(context);

            var viewResult = context.Result as ViewResult;
            if (viewResult is not null)
                viewResult.ViewData["Layout"] = _layoutName;
        }
    }
}
