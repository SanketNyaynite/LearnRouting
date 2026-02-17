using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using WebApp.Models;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.UseRouting();

app.UseEndpoints(endpoints =>
{

    endpoints.MapGet("/", async (HttpContext context) =>
    {
        await context.Response.WriteAsync("Welcome to the Homepage");
    });

    endpoints.MapGet("/people", (Person? p) =>
    {
        return $"Id is {p?.Id}; Name is {p?.Name}";
    });

//endpoints.MapGet("/employees", async (HttpContext context) =>
//{
//    // Get all of the employees' information
//    var employees = EmployeesRepository.GetEmployees();

//    context.Response.ContentType = "text/html";
//    await context.Response.WriteAsync("<h2>Employees</h><br/>");
//    await context.Response.WriteAsync("<ul>");
//    foreach (var employee in employees)
//    {
//        await context.Response.WriteAsync($"<li><b>{employee.Name}</b>: {employee.Position}</li>");
//    }
//    await context.Response.WriteAsync("</ul>");
//});

endpoints.MapGet("/people", (int id) =>
    {
        
    });

    endpoints.MapGet("/employees", (int[] ids) =>
    {
        var employees = EmployeesRepository.GetEmployees(); 
        var emps = employees.Where(x => ids.Contains(x.Id)).ToList();


        return emps;
    });

    //endpoints.MapGet("/employees/{id:int}", async (HttpContext context) =>
    //{
    //        var id = context.Request.RouteValues["id"];
    //        var employeeId = int.Parse(id.ToString());
        
    //        // Get a particular employee's information
    //        var employee = EmployeesRepository.GetEmployeeById(employeeId);

    //        context.Response.ContentType = "text/html";

    //    await context.Response.WriteAsync("<h2>Employee</h>");
    //    if (employee is not null)
    //        {
    //            await context.Response.WriteAsync($"Name: {employee.Name}<br/>");
    //            await context.Response.WriteAsync($"Position: {employee.Position}<br/>");
    //            await context.Response.WriteAsync($"Salary: {employee.Salary}<br/>");
    //        }
    //        else
    //        {
    //            context.Response.StatusCode = 404;
    //            await context.Response.WriteAsync("Employee not found.");
    //        }
    //});

    endpoints.MapPost("/employees",  (Employee employee) =>
    {
            if (employee is null || employee.Id <= 0)
            {
                return "Employee is not provided or is not valid";
            }

            EmployeesRepository.AddEmployee(employee);

        return "Employee added successfully.";
        
    });

    endpoints.MapPut("/employees", async (HttpContext context) =>
    {
        using var reader = new StreamReader(context.Request.Body);
        var body = await reader.ReadToEndAsync();
        var employee = JsonSerializer.Deserialize<Employee>(body);

        var result = EmployeesRepository.UpdateEmployee(employee);
        if (result)
        {
            context.Response.StatusCode = 204;
            return;
        }
        else
        {
            await context.Response.WriteAsync("Employee not found.");
        }
    });

    endpoints.MapDelete("/employees/{id}", async (HttpContext context) =>
    {
            var id = context.Request.RouteValues["id"];
            var employeeId = int.Parse(id.ToString());

        
            
                if (context.Request.Headers["Authorization"] == "frank")
                {
                    var result = EmployeesRepository.DeleteEmployee(employeeId);

                    if (result)
                    {
                        await context.Response.WriteAsync("Employee is deleted successfully.");
                    }
                    else
                    {
                        context.Response.StatusCode = 404;
                        await context.Response.WriteAsync("Employee not found.");
                    }
                }
                else
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("You are not authorized to delete.");
                }
    });
});

app.Run();



/*NOTES
 * An Endpoint handles a request and returns a response.
 * Model binding is extracting data from Http request to .Net objects as parameters in endpoint handlers.
 * 
 */


struct GetEmployeeParamereter
{
    [FromRoute]
    public int Id { get; set; }
    [FromQuery]
    public string Name { get; set; }
    [FromHeader]
    public string Position { get; set; }
}

class Person
{
    public int Id { get; set; }
    public string? Name { get; set; }

    public static ValueTask<Person?> BindAsync(HttpContext context)
    {
        var idStr = context.Request.Query["id"];
        var nameStr = context.Request.Headers["name"];
        if (int.TryParse(idStr, out var id))
        {
            return new ValueTask<Person?>(new Person {Id = id, Name = nameStr });
        }
        return new ValueTask<Person?>(Task.FromResult<Person?>(null));
    }
}