var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

//app.MapGet("/employees", async (HttpContext context) =>
//{
//   await context.Response.WriteAsync("Get Employees");
//});

app.Use(async (context, next) =>
{
    await next(context);
});

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
});

app.Run();





/*NOTES
 * An Endpoint handles a request and returns a response.
 * 
 * 
 */
