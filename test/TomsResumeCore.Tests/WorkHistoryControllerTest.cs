using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TomsResumeCore.DomainModels;
using TomsResumeCore.Data;
using TomsResumeCore.Web.API;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using TomsResumeCore.Service;
using Microsoft.Extensions.Hosting.Internal;

namespace TomsResumeCore.Tests
{
    public class WorkHistoryControllerTest
    {
        private readonly WorkHistoryController _controller;
        private readonly IWorkHistoryRepo _repo;
        private readonly IVisitService _visitService;
        private readonly IHostEnvironment _hostEnv;

        public WorkHistoryControllerTest()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettingsTest.json")
                .Build();

            _repo = new WorkHistoryRepo(configuration);
            _visitService = new VisitService(configuration);
            _hostEnv = new HostingEnvironment { EnvironmentName = Environments.Development };

            _controller = new WorkHistoryController(_repo, _visitService, _hostEnv);
        }

        [Fact]
        public async Task GetReturnsItemsAsync()
        {
            var result = await _controller.Get();

            Assert.IsAssignableFrom<ActionResult<IEnumerable<JobHeld>>>(result);
        }
    }
}
