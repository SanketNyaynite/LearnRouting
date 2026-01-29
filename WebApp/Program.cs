var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRouting(options =>
{
    options.ConstraintMap.Add("pos", typeof(PositionConstraint));
});

var app = builder.Build();

//app.MapGet("/employees", async (HttpContext context) =>
//{
//   await context.Response.WriteAsync("Get Employees");
//});

app.Use(async (context, next) =>
{
    await next(context);
});

app.UseStaticFiles();

app.UseRouting();

app.Use(async (context, next) =>
{
    await next(context);
});

app.UseEndpoints(endpoints =>
{
    endpoints.MapGet("/employees", async (HttpContext context) =>
    {
        await context.Response.WriteAsync("Get Employees");
    });

    endpoints.MapPost("/employees", async (HttpContext context) =>
    {
        await context.Response.WriteAsync("Created an Employee");
    });

    endpoints.MapPut("/employees", async (HttpContext context) =>
    {
        await context.Response.WriteAsync("Updated an Employee");
    });

    endpoints.MapDelete("/employees/{id}", async (HttpContext context) =>
    {
        await context.Response.WriteAsync($"Deleted an Employee: {context.Request.RouteValues["id"]}");
    });

    //endpoints.MapGet("/{category=shirts}/{size=medium}/{id?}", async (HttpContext context) =>
    //{
    //    await context.Response.WriteAsync($"Get category: {context.Request.RouteValues["category"]} in size: {context.Request.RouteValues["size"]} and ID is: {context.Request.RouteValues["id"]}");
    //});

    endpoints.MapGet("/employees/{id:int}", async (HttpContext context) =>
    {
        await context.Response.WriteAsync($"Get employee: {context.Request.RouteValues["id"]}");
    });

    endpoints.MapGet("/employees/positions/{position:pos}", async (HttpContext context) =>
    {
        await context.Response.WriteAsync($"Get employees under position: {context.Request.RouteValues["position"]}");
    });
});

app.Run();

class PositionConstraint : IRouteConstraint
{
    public bool Match(HttpContext? httpContext, IRouter? route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
    {
        if (!values.ContainsKey(routeKey)) return false;
        if (values[routeKey] is null) return false;
        
        if (values[routeKey].ToString().Equals("manager", StringComparison.OrdinalIgnoreCase) ||
            values[routeKey].ToString().Equals("developer", StringComparison.OrdinalIgnoreCase))        
            return true;
        return false;
    }
}



/*NOTES
 * An Endpoint handles a request and returns a response.
 * 
 * 
 */
