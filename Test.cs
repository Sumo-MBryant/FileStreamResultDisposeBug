using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace FileStreamResultDisposeBug
{
    public class Test
    {
        [Fact]
        public async Task FileStreamResultShouldDisposeFile()
        {
            // Write some content to a file.
            var content = "Hello";
            var fileName = Path.GetRandomFileName();
            File.WriteAllText(fileName, content);

            var builder = new WebHostBuilder()
                .ConfigureServices(services => services.AddMvc())
                .Configure(app => app.UseMvc());

            using (var server = new TestServer(builder))
            {
                using (var client = server.CreateClient())
                {
                    // Fetch the header for the file.
                    using (var request = new HttpRequestMessage(
                        HttpMethod.Head,
                        $"test/test?path={WebUtility.UrlEncode(fileName)}"))
                    {
                        using (var response = await client.SendAsync(request))
                        {
                            Assert.True(response.IsSuccessStatusCode);
                        }
                    }

                    // Delete the file.
                    File.Delete(fileName);
                }
            }
        }
    }
}
