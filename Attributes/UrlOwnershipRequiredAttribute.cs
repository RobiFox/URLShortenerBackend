using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace url_shorten_backend;

public class UrlOwnershipRequiredAttribute(UrlDbContext urlDbContext) : ActionFilterAttribute {
    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next) {
        string? owner = context.ActionArguments["owner"] as string;
        string? id = context.ActionArguments["id"] as string;
        if (owner == null) {
            context.Result = new BadRequestObjectResult("Missing owner");
            return;
        }
        if (id == null) {
            context.Result = new BadRequestObjectResult("Missing shortened url id");
            return;
        }
        IdUrlPair? pair = urlDbContext.UrlPairs.FirstOrDefault(e => e.Id == id);
        if (pair == null) {
            context.Result = new BadRequestObjectResult($"No URL with {id}");
            return;
        }
        if (pair.UploaderId != owner) {
            context.Result = new BadRequestObjectResult("Owner doesn't own url");
            return;
        }
        await next();
    }
}