var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient();

var app = builder.Build();

app.MapGet("/", () => "Project 1!");

app.MapGet("/get-project-2", async (IHttpClientFactory httpClientFactory) =>
{
    var httpClient = httpClientFactory.CreateClient();
    return await httpClient.GetStringAsync("http://project-2");
});


app.Run();
