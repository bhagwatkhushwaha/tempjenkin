using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Autumn.EmailServices
{
    public class SendEmailDto
    {
        public string userToaddress;
        public string subject;
        public string body;
        public IConfigurationRoot configuration;
    }
} 