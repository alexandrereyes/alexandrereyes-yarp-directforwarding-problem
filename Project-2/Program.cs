using System.Diagnostics;
using System.Net;
using Yarp.ReverseProxy.Forwarder;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpForwarder();

var app = builder.Build();

app.UseRouting()
    .UseEndpoints(endpoints =>
    {
        endpoints.Map("{**catch-all}", async httpContext =>
        {
            await app.Services.GetRequiredService<IHttpForwarder>().SendAsync(
                httpContext,
                httpContext.Request.Host.ToString(),
                new HttpMessageInvoker(new SocketsHttpHandler()
                {
                    UseProxy = false,
                    AllowAutoRedirect = false,
                    AutomaticDecompression = DecompressionMethods.All,
                    UseCookies = false,
                    ActivityHeadersPropagator = new ReverseProxyPropagator(DistributedContextPropagator.Current)
                })
            );
        });
    });

app.MapGet("/", () => "Project 2!");

app.Run();
