﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TomsResumeCore.Service
{
    public interface IGoogleRecaptchaService
    {
        Task<bool> IsReCaptchaPassed(string recaptchaResponse);        
    }
}
