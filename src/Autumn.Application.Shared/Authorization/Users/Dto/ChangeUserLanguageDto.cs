using System.ComponentModel.DataAnnotations;

namespace Autumn.Authorization.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}
