using System.IO;
using Microsoft.AspNetCore.Mvc;

namespace FileStreamResultDisposeBug
{
    [Route("test")]
    public class TestController
    {
        [Route("test")]
        public IActionResult GetFileContent(string path)
        {
            return new FileStreamResult(
                File.Open(path, FileMode.Open),
                "text/plain");
        }
    }
}
