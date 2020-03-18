using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Autumn.Goals.Dto
{
    public class PropertyTypeDto : FullAuditedEntity
    {
        public PropertyTypeEnumDto Type { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }

    public enum PropertyTypeEnumDto
    {
        Education = 1,
        Car = 2,
        Property = 3,
        Travel = 4,
        Investment = 5,
    }
}
