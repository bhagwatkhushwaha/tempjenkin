using Abp.BackgroundJobs;
using Abp.Dependency;
using Autumn.AwsEmail;
using Autumn.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Autumn.EmailServices
{
    public class sendEmail : BackgroundJob<SendEmailDto>, ITransientDependency
    {
        private readonly IAwsEmailSender _awsEmailSender; 

        public sendEmail(IAwsEmailSender awsEmailSender)
        {
            _awsEmailSender = awsEmailSender; 
        }

        public async override void Execute(SendEmailDto args)
        {
            await _awsEmailSender.OutlookEmailSendAsync(args.userToaddress, args.subject, args.body, args.configuration);
        }
    }
}
