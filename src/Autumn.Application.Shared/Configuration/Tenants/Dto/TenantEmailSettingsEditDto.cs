using Abp.Auditing;
using Autumn.Configuration.Dto;

namespace Autumn.Configuration.Tenants.Dto
{
    public class TenantEmailSettingsEditDto : EmailSettingsEditDto
    {
        public bool UseHostDefaultEmailSettings { get; set; }
    }
}