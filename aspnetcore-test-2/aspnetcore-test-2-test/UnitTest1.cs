using System.Net.Mime;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using Xunit;

namespace aspnetcore_test_2_test
{
    public class UnitTest1
    {
        [Fact]
        public async void Test1()
        {
            
            var hostBuilder = new HostBuilder()
                .ConfigureWebHost(webHost =>
                {
                    webHost.UseTestServer();
                    webHost.Configure(app => app.Run(async ctx =>
                    {

                        ctx.Response.Headers.Add("Accept-Ranges", "bytes");
                        var contentDispositionHeader = new ContentDisposition()
                        {
                            FileName = "foo bar.pdf",
                            DispositionType = "attachment"
                        };
                        ctx.Response.Headers.Add("Content-Disposition", contentDispositionHeader.ToString());
                        ctx.Response.Headers.Add("Content-Type", "application/octet-stream");
                        ctx.Response.Headers.Add("Content-Length", "42");

                    }));
                });

            
            var host = await hostBuilder.StartAsync();

            var client = host.GetTestClient();

            var response = await client.GetAsync("/");

            var fileName = response.Content.Headers.ContentDisposition.FileName;
            // fileName = "\"foo bar.pdf\""
            Assert.Equal("foo bar.pdf", fileName);


        }
    }
}