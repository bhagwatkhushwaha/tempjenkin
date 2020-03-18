using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Autumn.Editions.Dto;

namespace Autumn.MultiTenancy.Dto
{
    public class GetTenantFeaturesEditOutput
    {
        public List<NameValueDto> FeatureValues { get; set; }

        public List<FlatFeatureDto> Features { get; set; }
    }
}