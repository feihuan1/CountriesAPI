using System.Diagnostics.Metrics;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();


app.UseRouting();

Dictionary<int, string> countries = new Dictionary<int, string>()
{
    { 1, "United States" },
    { 2, "Canada" },
    { 3, "United Kingdom" },
    { 4, "India" },
    { 5, "Japan" },
};

app.UseEndpoints(endpoints =>
{
    endpoints.MapGet("countries", async (HttpContext context) =>
    {
        foreach (KeyValuePair<int, string> country in countries)
        {
        await context.Response.WriteAsync($"{country.Key} - {country.Value}\n");
        }
    });

    endpoints.MapGet("country/{id:int:range(1,100)}", async (HttpContext context) =>
    {
        if(context.Request.RouteValues.ContainsKey("id") == false)
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync("Please provide id of the country");
        }

        int id = Convert.ToInt32(context.Request.RouteValues["id"]);
        if (countries.ContainsKey(id))
        {
            await context.Response.WriteAsync($"{id} - {countries[id]}");
        }
        else
        {
            context.Response.StatusCode = 404;
            await context.Response.WriteAsync("[No Country]");
        }
    });
   
    
    endpoints.MapGet("country/{id:int:min(101)}",async (HttpContext context) =>
    {
        await context.Response.WriteAsync("The CountryID should be between 1 and 100");
    });
    
    endpoints.MapGet("country/{id:int:max(0)}",async (HttpContext context) =>
    {
        await context.Response.WriteAsync("The CountryID should be between 1 and 100");
    });

});

app.Run(async (HttpContext context) => 
{
    context.Response.WriteAsync("Invalid Path");
});

app.Run();
