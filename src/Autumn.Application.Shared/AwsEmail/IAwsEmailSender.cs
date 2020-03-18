using Abp.Application.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Autumn.AwsEmail
{
    public interface IAwsEmailSender : IApplicationService
    {
        Task AmazonEmailSendAsync(string userToaddress, string subject, string body);

        Task OutlookEmailSendAsync(string userToaddress, string subject, string body, IConfigurationRoot configuration);
    }
}
