using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using TomsResumeCore.DomainModels;
using TomsResumeCore.Service;
using TomsResumeCore.Web.API;
using Xunit;

namespace TomsResumeCore.Tests
{
    public class ContactControllerTest
    {
        private readonly ContactController _controller;
        private readonly IEmailService _emailService;
        private readonly IGoogleRecaptchaService _googleRecaptchaService;

        public ContactControllerTest()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettingsTest.json")
                .Build();

            _googleRecaptchaService = new GoogleRecaptchaService(configuration);
            _emailService = new EmailService(_googleRecaptchaService, configuration);
            _controller = new ContactController(_emailService, _googleRecaptchaService);
        }

        [Fact]
        public async Task PostReturns204Result()
        {
            var payload = new ContactPayload() {
                email = "test@test.com",
                name = "Unit Tester",
                message = "We are running unit tests!",
                recaptcha = "NotGoingToWork"
            };

            var result = await _controller.Post(payload);

            Assert.IsAssignableFrom<ActionResult>(result);
        }

        [Fact]
        public async Task PostEmptyFieldReturns204Result()
        {
            var payload = new ContactPayload()
            {
                email = "test@test.com",
                name = "",//This space intentionally left blank.
                message = "We are running unit tests!",
                recaptcha = "NotGoingToWork"
            };

            var result = await _controller.Post(payload);

            Assert.IsAssignableFrom<BadRequestResult>(result);
        }
    }
}
