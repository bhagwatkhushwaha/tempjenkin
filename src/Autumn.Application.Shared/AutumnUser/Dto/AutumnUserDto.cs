using Abp.Domain.Entities.Auditing;
using Autumn.Authorization.Users.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Autumn.AutumnUser.Dto
{
    public class AutumnUserDto : FullAuditedEntity
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public string UserName { get; set; }

        public string EmailAddress { get; set; }

        public string Password { get; set; }

        public int Age { get; set; }

        public GenderEnumDto? Gender { get; set; }

        public int CountryId { get; set; }

        public RetirementGoalsEnumDto RetirementGoalOptions { get; set; }

        public double? DesiredRetirementSum { get; set; }

        public double? DesiredRetirementIncome { get; set; }

        public double? DesiredLegacyAmount { get; set; }

        public int? DesiredRetirementAge { get; set; }

        public float? ReturnRate { get; set; }

        public double? InitialSaved { get; set; }
                       
        public double? InitialOwed { get; set; }
                       
        public double? InitialNet { get; set; }
                       
        public double? RequiredSavings { get; set; }
                       
        public double? TotalMonthlyIncome { get; set; }
                       
        public double? TotalMonthlyExpences { get; set; }
                       
        public double? TotalMonthlySavings { get; set; }
                       
        public double? TotalSavings { get; set; }
                       
        public double? TotalLiabilities { get; set; }
                       
        public double? LikelyRetirementSum { get; set; }
                       
        public double? LikelyRetirementIncome { get; set; }
                       
        public double? LikelyRetirementLegacy { get; set; }
                       
        public double? LikelyRetirementAge { get; set; }

        public string DesiredRetirementProgress { get; set; }

        public int RegistrationStep { get; set; }
    }
}
