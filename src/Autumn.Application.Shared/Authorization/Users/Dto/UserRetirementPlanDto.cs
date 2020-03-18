using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Autumn.Authorization.Users.Dto
{
    public class UserRetirementPlanDto: FullAuditedEntity, IMayHaveTenant
    {
        [Required]
        public long UserId { get; set; }

        public RetirementGoalsEnumDto? RetirementGoalOptions { get; set; }

        public double? DesiredRetirementSum { get; set; }

        public double? DesiredRetirementIncome { get; set; }

        public double? DesiredLegacyAmount { get; set; }

        [MaxLength(2)]
        public int? DesiredRetirementAge { get; set; }

        public float? ReturnRate { get; set; }

        [MaxLength(15)]
        public double? InitialSaved { get; set; }

        [MaxLength(15)]
        public double? InitialOwed { get; set; }

        public double? InitialNet { get; set; }

        public double? RequiredSavings { get; set; }

        [Required]
        public double? TotalMonthlyIncome { get; set; }

        [Required]
        public double? TotalMonthlyExpences { get; set; }

        [MaxLength(15)]
        public double? TotalMonthlySavings { get; set; }

        [MaxLength(15)]
        public double? TotalLiabilities { get; set; }

        public double? LikelyRetirementSum { get; set; }

        [MaxLength(12)]
        public double? LikelyRetirementIncome { get; set; }

        public double? LikelyRetirementLegacy { get; set; }

        [MaxLength(2)]
        public double? LikelyRetirementAge { get; set; }

        public string DesiredRetirementProgress { get; set; }

        public bool FamilyView { get; set; }

        public int? TenantId { get; set; }
    }
}
