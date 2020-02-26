using System;
using System.Net.Mime;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using Xunit;
using Xunit.Abstractions;

namespace aspnetcore_test_2_test
{
    public class UnitTest1
    {
        private ITestOutputHelper _output;

        public UnitTest1(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void Test2()
        {
            
            var contentDispositionHeader = new System.Net.Mime.ContentDisposition()
            {
                FileName = "foo bar.pdf",
                DispositionType = "attachment"
            };

            var result = contentDispositionHeader.ToString();

            _output.WriteLine(result);

            var parsed = Microsoft.Net.Http.Headers.ContentDispositionHeaderValue.Parse(result);

            Assert.Equal("foo bar.pdf", parsed.FileName);

            var parsed0 = System.Net.Http.Headers.ContentDispositionHeaderValue.Parse(result);

            Assert.Equal("foo bar.pdf", parsed0.FileName);
            
        }
        
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