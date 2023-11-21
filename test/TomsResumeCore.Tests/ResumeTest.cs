using Microsoft.Extensions.Configuration;
using System.IO;
using System.Threading.Tasks;
using TomsResumeCore.Data;
using TomsResumeCore.Data.Interfaces;
using Xunit;

namespace TomsResumeCore.Tests
{
    public class ResumeTest
    {
        //We are going to test the repo only here since the razor page uses a custom header and the HttpContext class is difficult to test.
        //Discussion on this found here: https://stackoverflow.com/questions/40284313/how-do-i-create-an-httpcontext-for-my-unit-test

        private readonly IResumeRepo _repo;

        public ResumeTest()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettingsTest.json")
                .Build();

            _repo = new ResumeRepo(configuration);
        }

        [Fact]
        public async Task GetResumeReturnsStream()
        {
            var result = await _repo.GetResumePDF();

            Assert.IsAssignableFrom<System.IO.Stream>(result);
        }

        [Fact]
        public void GetResumeFileNameReturnsString()
        {
            var result = _repo.GetResumeFileName();

            Assert.IsAssignableFrom<string>(result);
        }
    }
}
