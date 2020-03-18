using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Autumn.Goals
{
    [Table("PropertyTypes")]
    public class PropertyType: FullAuditedEntity
    {
        public PropertyTypeEnum Type { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }

    public enum PropertyTypeEnum
    {
        Education = 1,
        Car = 2,
        Property = 3,
        Travel = 4,
        Investment = 5,
    }
}
