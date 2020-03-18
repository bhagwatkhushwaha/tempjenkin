using System.ComponentModel.DataAnnotations;

namespace Autumn.Authorization.Accounts.Dto
{
    public class SendEmailActivationLinkInput
    {
        [Required]
        public string EmailAddress { get; set; }
    }
}