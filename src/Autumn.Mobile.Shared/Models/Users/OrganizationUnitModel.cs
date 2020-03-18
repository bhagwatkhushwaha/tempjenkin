using Abp.AutoMapper;
using Autumn.Organizations.Dto;

namespace Autumn.Models.Users
{
    [AutoMapFrom(typeof(OrganizationUnitDto))]
    public class OrganizationUnitModel : OrganizationUnitDto
    {
        public bool IsAssigned { get; set; }
    }
}