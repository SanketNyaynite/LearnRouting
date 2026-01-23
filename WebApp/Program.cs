var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

//app.MapGet("/employees", async (HttpContext context) =>
//{
//   await context.Response.WriteAsync("Get Employees");
//});

app.UseEndpoints(endpoints =>
{
    endpoints.MapGet("/employee", (HttpContext context) =>
    {
        await context.Response.WriteAsync("Get Employees");
    });

    endpoints.MapPost("/employees", (HttpContext context) =>
    {
        await context.Response.WriteAsync("Get Employees");
    });

    endpoints.MapPut("/employees", (HttpContext context) =>
    {
        await context.Response.WriteAsync("Update an Employee");
    });

    endpoints.MapDelete("/employees", (HttpContext context) =>
    {
        await context.Response.WriteAsync("Delete an Employee");
    });
});

app.Run();





/*NOTES
 * An Endpoint handles a request and returns a response.
 * 
 * 
 */
