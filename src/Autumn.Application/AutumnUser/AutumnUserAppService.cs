using System.Collections.Generic;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Notifications;
using Abp.Organizations;
using Abp.Zero.Configuration;
using Microsoft.AspNetCore.Identity;
using Autumn.Authorization.Roles;
using Autumn.Authorization.Users.Exporting;
using Autumn.Notifications;
using Autumn.Url;
using Autumn.Authorization.Users;
using Autumn.AutumnUser.Dto;
using System.Threading.Tasks;
using System;
using Abp.UI;
using System.Collections.ObjectModel;
using Autumn.Authorization.Users.Dto;
using Autumn.Countries;
using Autumn.FinancialCalc;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Autumn.AutumnUser
{
    public class AutumnUserAppService : AutumnAppServiceBase, IAutumnUserAppService
    {
        public IAppUrlService AppUrlService { get; set; }

        private readonly RoleManager _roleManager;
        private readonly IUserEmailer _userEmailer;
        private readonly IUserListExcelExporter _userListExcelExporter;
        private readonly INotificationSubscriptionManager _notificationSubscriptionManager;
        private readonly IAppNotifier _appNotifier;
        private readonly IRepository<RolePermissionSetting, long> _rolePermissionRepository;
        private readonly IRepository<UserPermissionSetting, long> _userPermissionRepository;
        private readonly IRepository<UserRole, long> _userRoleRepository;
        private readonly IRepository<Role> _roleRepository;
        private readonly IUserPolicy _userPolicy;
        private readonly IEnumerable<IPasswordValidator<User>> _passwordValidators;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IRepository<OrganizationUnit, long> _organizationUnitRepository;
        private readonly IRoleManagementConfig _roleManagementConfig;
        private readonly UserManager _userManager;
        private readonly IRepository<UserRetirementPlan> _userRetirementPlanRepository;
        private readonly IRepository<Country> _countryRepository;

        public AutumnUserAppService(
            RoleManager roleManager,
            IUserEmailer userEmailer,
            IUserListExcelExporter userListExcelExporter,
            INotificationSubscriptionManager notificationSubscriptionManager,
            IAppNotifier appNotifier,
            IRepository<RolePermissionSetting, long> rolePermissionRepository,
            IRepository<UserPermissionSetting, long> userPermissionRepository,
            IRepository<UserRole, long> userRoleRepository,
            IRepository<Role> roleRepository,
            IUserPolicy userPolicy,
            IEnumerable<IPasswordValidator<User>> passwordValidators,
            IPasswordHasher<User> passwordHasher,
            IRepository<OrganizationUnit, long> organizationUnitRepository,
            IRoleManagementConfig roleManagementConfig,
            UserManager userManager,
            IRepository<UserRetirementPlan> userRetirementPlanRepository,
            IRepository<Country> countryRepository)
        {
            _roleManager = roleManager;
            _userEmailer = userEmailer;
            _userListExcelExporter = userListExcelExporter;
            _notificationSubscriptionManager = notificationSubscriptionManager;
            _appNotifier = appNotifier;
            _rolePermissionRepository = rolePermissionRepository;
            _userPermissionRepository = userPermissionRepository;
            _userRoleRepository = userRoleRepository;
            _userPolicy = userPolicy;
            _passwordValidators = passwordValidators;
            _passwordHasher = passwordHasher;
            _organizationUnitRepository = organizationUnitRepository;
            _roleManagementConfig = roleManagementConfig;
            _userManager = userManager;
            _roleRepository = roleRepository;
            _userRetirementPlanRepository = userRetirementPlanRepository;
            _countryRepository = countryRepository;

            AppUrlService = NullAppUrlService.Instance;
        }

        public async Task CreateUser(AutumnUserDto input)
        {
            try
            {
                var user = ObjectMapper.Map<User>(input); //Passwords is not mapped (see mapping configuration)
                user.TenantId = null;

                await UserManager.InitializeOptionsAsync(AbpSession.TenantId);
                foreach (var validator in _passwordValidators)
                {
                    CheckErrors(await validator.ValidateAsync(UserManager, user, input.Password));
                }

                user.Password = _passwordHasher.HashPassword(user, input.Password);
                user.Surname = "xyz";
                //Assign roles
                var role = await _roleManager.GetRoleByNameAsync(StaticRoleNames.Host.User);
                user.Roles = new Collection<UserRole>();
                user.CreatorUserId = null;
                user.DeleterUserId = null;
                user.LastModifierUserId = null;
                user.Roles.Add(new UserRole(null, user.Id, role.Id));

                CheckErrors(await UserManager.CreateAsync(user));
                await CurrentUnitOfWork.SaveChangesAsync(); //To get new user's Id.

                //Notifications
                await _notificationSubscriptionManager.SubscribeToAllAvailableNotificationsAsync(user.ToUserIdentifier());
                await _appNotifier.WelcomeToTheApplicationAsync(user);

                //Organization Units
                //await UserManager.SetOrganizationUnitsAsync(user, input.OrganizationUnits.ToArray());

                //Send activation email
                //if (input.SendActivationEmail)
                //{
                //    user.SetNewEmailConfirmationCode();
                //    await _userEmailer.SendEmailActivationLinkAsync(
                //        user,
                //        AppUrlService.CreateEmailActivationUrlFormat(AbpSession.TenantId),
                //        input.User.Password
                //    );
                //}

                var userRetirementPlan = ObjectMapper.Map<UserRetirementPlanDto>(input);
                userRetirementPlan.UserId = user.Id;
                userRetirementPlan.ReturnRate = 0;
                var countryData = _countryRepository.FirstOrDefault((int)user.CountryId);
                switch (userRetirementPlan.RetirementGoalOptions)
                {
                    //Variable legacy
                    case RetirementGoalsEnumDto.DesiredRetirementSum:
                        if (userRetirementPlan.DesiredLegacyAmount == null)
                            userRetirementPlan.DesiredLegacyAmount = 0;

                        userRetirementPlan.DesiredRetirementIncome = FinancialCalculations.PMT((double)userRetirementPlan.ReturnRate, (double)userRetirementPlan.DesiredRetirementAge - (double)user.Age, -(double)userRetirementPlan.DesiredRetirementSum, (double)userRetirementPlan.DesiredLegacyAmount) / 12;
                        userRetirementPlan.InitialNet = userRetirementPlan.InitialSaved - userRetirementPlan.InitialOwed;
                        userRetirementPlan.RequiredSavings = FinancialCalculations.PMT((double)userRetirementPlan.ReturnRate, (double)userRetirementPlan.DesiredRetirementAge - (double)user.Age, (double)userRetirementPlan.InitialNet, -(double)userRetirementPlan.DesiredRetirementSum) / 12;
                        userRetirementPlan.TotalMonthlySavings = userRetirementPlan.TotalMonthlyIncome - userRetirementPlan.TotalMonthlyExpences;
                        userRetirementPlan.LikelyRetirementSum = FinancialCalculations.FV((double)userRetirementPlan.ReturnRate, (double)userRetirementPlan.DesiredRetirementAge - (double)user.Age, (double)userRetirementPlan.TotalMonthlySavings * -12, (double)-userRetirementPlan.InitialNet) * (Math.Pow(1 + (double)userRetirementPlan.ReturnRate, (double)userRetirementPlan.DesiredRetirementAge - (double)user.Age));
                        userRetirementPlan.LikelyRetirementLegacy = FinancialCalculations.FV((double)userRetirementPlan.ReturnRate, countryData.LifeExpectancy - (double)userRetirementPlan.DesiredRetirementAge, (double)userRetirementPlan.DesiredRetirementIncome * 12, -(double)userRetirementPlan.LikelyRetirementSum);

                        break;

                    //Variable legacy
                    case RetirementGoalsEnumDto.DesiredRetirementIncome:
                        if (userRetirementPlan.DesiredLegacyAmount == null)
                            userRetirementPlan.DesiredLegacyAmount = 0;

                        userRetirementPlan.DesiredRetirementSum = FinancialCalculations.PV((double)userRetirementPlan.ReturnRate, countryData.LifeExpectancy - (double)userRetirementPlan.DesiredRetirementAge, (double)userRetirementPlan.DesiredRetirementIncome * -12, (double)userRetirementPlan.DesiredLegacyAmount) * (Math.Pow(1 + (double)userRetirementPlan.ReturnRate, (double)userRetirementPlan.DesiredRetirementAge - (double)user.Age));
                        userRetirementPlan.InitialNet = userRetirementPlan.InitialSaved - userRetirementPlan.InitialOwed;
                        userRetirementPlan.RequiredSavings = FinancialCalculations.PMT((double)userRetirementPlan.ReturnRate, (double)userRetirementPlan.DesiredRetirementAge - (double)user.Age, (double)userRetirementPlan.InitialNet, -(double)userRetirementPlan.DesiredRetirementSum) / 12;
                        userRetirementPlan.TotalMonthlySavings = userRetirementPlan.TotalMonthlyIncome - userRetirementPlan.TotalMonthlyExpences;
                        userRetirementPlan.LikelyRetirementSum = FinancialCalculations.FV((double)userRetirementPlan.ReturnRate, (double)userRetirementPlan.DesiredRetirementAge - (double)user.Age, (double)userRetirementPlan.TotalMonthlySavings * -12, (double)-userRetirementPlan.InitialNet) * (Math.Pow(1 + (double)userRetirementPlan.ReturnRate, (double)userRetirementPlan.DesiredRetirementAge - (double)user.Age));
                        userRetirementPlan.LikelyRetirementLegacy = FinancialCalculations.FV((double)userRetirementPlan.ReturnRate, countryData.LifeExpectancy - (double)userRetirementPlan.DesiredRetirementAge, (double)userRetirementPlan.DesiredRetirementIncome * 12, -(double)userRetirementPlan.LikelyRetirementSum);

                        break;

                    //Variable age
                    case RetirementGoalsEnumDto.DesiredLegacyAmount:

                        userRetirementPlan.DesiredRetirementAge = countryData.RetirementAge;
                        userRetirementPlan.DesiredRetirementSum = FinancialCalculations.PV((double)userRetirementPlan.ReturnRate, countryData.LifeExpectancy - (double)userRetirementPlan.DesiredRetirementAge, (double)userRetirementPlan.DesiredRetirementIncome * -12, (double)userRetirementPlan.DesiredLegacyAmount) * (Math.Pow(1 + (double)userRetirementPlan.ReturnRate, (double)userRetirementPlan.DesiredRetirementAge - (double)user.Age));
                        userRetirementPlan.InitialNet = userRetirementPlan.InitialSaved - userRetirementPlan.InitialOwed;
                        userRetirementPlan.TotalMonthlySavings = userRetirementPlan.TotalMonthlyIncome - userRetirementPlan.TotalMonthlyExpences;
                        userRetirementPlan.RequiredSavings = FinancialCalculations.PMT((double)userRetirementPlan.ReturnRate, (double)userRetirementPlan.DesiredRetirementAge - (double)user.Age, (double)userRetirementPlan.InitialNet, -(double)userRetirementPlan.DesiredRetirementSum) / 12;
                        userRetirementPlan.LikelyRetirementSum = FinancialCalculations.FV((double)userRetirementPlan.ReturnRate, (double)userRetirementPlan.DesiredRetirementAge - (double)user.Age, (double)userRetirementPlan.InitialNet * -12, (double)-userRetirementPlan.InitialNet) * (Math.Pow(1 + (double)userRetirementPlan.ReturnRate, (double)userRetirementPlan.DesiredRetirementAge - (double)user.Age));
                        userRetirementPlan.LikelyRetirementAge = Math.Round((((double)userRetirementPlan.DesiredRetirementSum - (double)userRetirementPlan.LikelyRetirementSum) / (((double)userRetirementPlan.TotalMonthlySavings + (double)userRetirementPlan.DesiredRetirementIncome) * 12)) + (double)userRetirementPlan.DesiredRetirementAge, MidpointRounding.AwayFromZero);

                        break;

                    //Variable legacy
                    case RetirementGoalsEnumDto.DesiredRetirementAge:
                        if (userRetirementPlan.DesiredLegacyAmount == null)
                            userRetirementPlan.DesiredLegacyAmount = 0;

                        userRetirementPlan.DesiredRetirementSum = FinancialCalculations.PV((double)userRetirementPlan.ReturnRate, countryData.LifeExpectancy - (double)userRetirementPlan.DesiredRetirementAge, (double)userRetirementPlan.DesiredRetirementIncome * -12, (double)userRetirementPlan.DesiredLegacyAmount) * (Math.Pow(1 + (double)userRetirementPlan.ReturnRate, (double)userRetirementPlan.DesiredRetirementAge - (double)user.Age));
                        userRetirementPlan.InitialNet = userRetirementPlan.InitialSaved - userRetirementPlan.InitialOwed;
                        userRetirementPlan.RequiredSavings = FinancialCalculations.PMT((double)userRetirementPlan.ReturnRate, (double)userRetirementPlan.DesiredRetirementAge - (double)user.Age, (double)userRetirementPlan.InitialNet, -(double)userRetirementPlan.DesiredRetirementSum) / 12;
                        userRetirementPlan.TotalMonthlySavings = userRetirementPlan.TotalMonthlyIncome - userRetirementPlan.TotalMonthlyExpences;
                        userRetirementPlan.LikelyRetirementSum = FinancialCalculations.FV((double)userRetirementPlan.ReturnRate, (double)userRetirementPlan.DesiredRetirementAge - (double)user.Age, (double)userRetirementPlan.TotalMonthlySavings * -12, (double)-userRetirementPlan.InitialNet) * (Math.Pow(1 + (double)userRetirementPlan.ReturnRate, (double)userRetirementPlan.DesiredRetirementAge - (double)user.Age));
                        userRetirementPlan.LikelyRetirementLegacy = FinancialCalculations.FV((double)userRetirementPlan.ReturnRate, countryData.LifeExpectancy - (double)userRetirementPlan.DesiredRetirementAge, (double)userRetirementPlan.DesiredRetirementIncome * 12, -(double)userRetirementPlan.LikelyRetirementSum);

                        break;

                    case RetirementGoalsEnumDto.PlaceToLiveAfterRetirement:

                        break;

                    default:

                        userRetirementPlan.DesiredRetirementAge = countryData.RetirementAge;
                        userRetirementPlan.DesiredLegacyAmount = 0;

                        break;
                }

                await _userRetirementPlanRepository.InsertAsync(ObjectMapper.Map<UserRetirementPlan>(userRetirementPlan));
            }
            catch (UserFriendlyException e)
            {
                throw new UserFriendlyException(e.Message);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<AutumnUserDto> CheckUserProgress()
        {
            try
            {
                var getuser = await UserManager.FindByIdAsync(AbpSession.UserId.Value.ToString());

                var user = ObjectMapper.Map<AutumnUserDto>(getuser);

                var userRetiretmentData = await _userRetirementPlanRepository.GetAll().FirstOrDefaultAsync(r => r.UserId == user.Id);

                user.RetirementGoalOptions = (RetirementGoalsEnumDto)userRetiretmentData.RetirementGoalOptions;

                user.DesiredRetirementSum = userRetiretmentData.DesiredRetirementSum;

                user.DesiredRetirementIncome = userRetiretmentData.DesiredRetirementIncome;

                user.DesiredLegacyAmount = userRetiretmentData.DesiredLegacyAmount;

                user.DesiredRetirementAge = userRetiretmentData.DesiredRetirementAge;

                user.ReturnRate = userRetiretmentData.ReturnRate;

                user.InitialSaved = userRetiretmentData.InitialSaved;

                user.InitialOwed = userRetiretmentData.InitialOwed;

                user.InitialNet = userRetiretmentData.InitialNet;

                user.RequiredSavings = userRetiretmentData.RequiredSavings;

                user.TotalMonthlyIncome = userRetiretmentData.TotalMonthlyIncome;

                user.TotalMonthlyExpences = userRetiretmentData.TotalMonthlyExpences;

                user.TotalLiabilities = userRetiretmentData.TotalLiabilities;

                user.LikelyRetirementSum = userRetiretmentData.LikelyRetirementSum;

                user.LikelyRetirementIncome = userRetiretmentData.LikelyRetirementIncome;

                user.LikelyRetirementLegacy = userRetiretmentData.LikelyRetirementLegacy;


                user.LikelyRetirementAge = userRetiretmentData.LikelyRetirementAge;

                user.DesiredRetirementProgress = userRetiretmentData.DesiredRetirementProgress;

                return user;

            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task UpdateUser(AutumnUserDto user)
        {
            try
            {
                var autumnUser = await UserManager.FindByIdAsync(AbpSession.UserId.ToString());
                autumnUser.Age = user.Age;
                autumnUser.CountryId = user.CountryId;
                autumnUser.EmailAddress = user.EmailAddress;
                autumnUser.Name = user.Name;
                autumnUser.Gender = (Gender)user.Gender;
                autumnUser.Surname = "xyz";
                CheckErrors(await UserManager.UpdateAsync(autumnUser));
                var userRetiretmentData = await _userRetirementPlanRepository.GetAll().FirstOrDefaultAsync(r => r.UserId == AbpSession.UserId);
                userRetiretmentData.RetirementGoalOptions = (RetirementGoals)user.RetirementGoalOptions;
                userRetiretmentData.TotalMonthlyIncome = user.TotalMonthlyIncome;
                userRetiretmentData.TotalMonthlyExpences = user.TotalMonthlyExpences;
                userRetiretmentData.InitialSaved = user.InitialSaved;
                userRetiretmentData.InitialOwed = user.InitialOwed;

                userRetiretmentData.DesiredRetirementSum = user.DesiredRetirementSum;
                userRetiretmentData.DesiredRetirementIncome = user.DesiredRetirementIncome;
                userRetiretmentData.DesiredLegacyAmount = user.DesiredLegacyAmount;
                userRetiretmentData.DesiredRetirementAge = user.DesiredRetirementAge;






                var userRetirementPlan = ObjectMapper.Map<UserRetirementPlanDto>(user);
                userRetirementPlan.UserId = AbpSession.UserId.Value;
                userRetirementPlan.ReturnRate = 0;
                var countryData = _countryRepository.FirstOrDefault((int)user.CountryId);
                switch (userRetirementPlan.RetirementGoalOptions)
                {
                    //Variable legacy
                    case RetirementGoalsEnumDto.DesiredRetirementSum:
                        if (userRetirementPlan.DesiredLegacyAmount == null)
                            userRetirementPlan.DesiredLegacyAmount = 0;

                        userRetiretmentData.DesiredRetirementIncome = FinancialCalculations.PMT((double)userRetirementPlan.ReturnRate, (double)userRetirementPlan.DesiredRetirementAge - (double)user.Age, -(double)userRetirementPlan.DesiredRetirementSum, (double)userRetirementPlan.DesiredLegacyAmount) / 12;
                        userRetiretmentData.InitialNet = userRetirementPlan.InitialSaved + userRetirementPlan.InitialOwed;
                        userRetiretmentData.RequiredSavings = FinancialCalculations.PMT((double)userRetirementPlan.ReturnRate, (double)userRetirementPlan.DesiredRetirementAge - (double)user.Age, (double)userRetiretmentData.InitialNet, -(double)userRetirementPlan.DesiredRetirementSum) / 12;
                        userRetiretmentData.TotalMonthlySavings = userRetirementPlan.TotalMonthlyIncome - userRetirementPlan.TotalMonthlyExpences;
                        userRetiretmentData.LikelyRetirementSum = FinancialCalculations.FV((double)userRetirementPlan.ReturnRate, (double)userRetirementPlan.DesiredRetirementAge - (double)user.Age, (double)userRetiretmentData.TotalMonthlySavings * -12, (double)-userRetiretmentData.InitialNet) * (Math.Pow(1 + (double)userRetirementPlan.ReturnRate, (double)userRetirementPlan.DesiredRetirementAge - (double)user.Age));
                        userRetiretmentData.LikelyRetirementLegacy = FinancialCalculations.FV((double)userRetirementPlan.ReturnRate, countryData.LifeExpectancy - (double)userRetirementPlan.DesiredRetirementAge, (double)userRetiretmentData.DesiredRetirementIncome * 12, -(double)userRetiretmentData.LikelyRetirementSum);

                        break;

                    //Variable legacy
                    case RetirementGoalsEnumDto.DesiredRetirementIncome:
                        if (userRetirementPlan.DesiredLegacyAmount == null)
                            userRetirementPlan.DesiredLegacyAmount = 0;

                        userRetiretmentData.DesiredRetirementSum = FinancialCalculations.PV((double)userRetirementPlan.ReturnRate, countryData.LifeExpectancy - (double)userRetirementPlan.DesiredRetirementAge, (double)userRetirementPlan.DesiredRetirementIncome * -12, (double)userRetirementPlan.DesiredLegacyAmount) * (Math.Pow(1 + (double)userRetirementPlan.ReturnRate, (double)userRetirementPlan.DesiredRetirementAge - (double)user.Age));
                        userRetiretmentData.InitialNet = userRetirementPlan.InitialSaved + userRetirementPlan.InitialOwed;
                        userRetiretmentData.RequiredSavings = FinancialCalculations.PMT((double)userRetirementPlan.ReturnRate, (double)userRetirementPlan.DesiredRetirementAge - (double)user.Age, (double)userRetiretmentData.InitialNet, -(double)userRetiretmentData.DesiredRetirementSum) / 12;
                        userRetiretmentData.TotalMonthlySavings = userRetirementPlan.TotalMonthlyIncome - userRetirementPlan.TotalMonthlyExpences;
                        userRetiretmentData.LikelyRetirementSum = FinancialCalculations.FV((double)userRetirementPlan.ReturnRate, (double)userRetirementPlan.DesiredRetirementAge - (double)user.Age, (double)userRetiretmentData.TotalMonthlySavings * -12, (double)-userRetiretmentData.InitialNet) * (Math.Pow(1 + (double)userRetirementPlan.ReturnRate, (double)userRetirementPlan.DesiredRetirementAge - (double)user.Age));
                        userRetiretmentData.LikelyRetirementLegacy = FinancialCalculations.FV((double)userRetirementPlan.ReturnRate, countryData.LifeExpectancy - (double)userRetirementPlan.DesiredRetirementAge, (double)userRetirementPlan.DesiredRetirementIncome * 12, -(double)userRetiretmentData.LikelyRetirementSum);

                        break;

                    //Variable age
                    case RetirementGoalsEnumDto.DesiredLegacyAmount:

                        userRetiretmentData.DesiredRetirementAge = countryData.RetirementAge;
                        userRetiretmentData.DesiredRetirementSum = FinancialCalculations.PV((double)userRetirementPlan.ReturnRate, countryData.LifeExpectancy - (double)userRetirementPlan.DesiredRetirementAge, (double)userRetirementPlan.DesiredRetirementIncome * -12, (double)userRetirementPlan.DesiredLegacyAmount) * (Math.Pow(1 + (double)userRetirementPlan.ReturnRate, (double)userRetirementPlan.DesiredRetirementAge - (double)user.Age));
                        userRetiretmentData.InitialNet = userRetirementPlan.InitialSaved + userRetirementPlan.InitialOwed;
                        userRetiretmentData.TotalMonthlySavings = userRetirementPlan.TotalMonthlyIncome - userRetirementPlan.TotalMonthlyExpences;
                        userRetiretmentData.RequiredSavings = FinancialCalculations.PMT((double)userRetirementPlan.ReturnRate, (double)userRetirementPlan.DesiredRetirementAge - (double)user.Age, (double)userRetiretmentData.InitialNet, -(double)userRetiretmentData.DesiredRetirementSum) / 12;
                        userRetiretmentData.LikelyRetirementSum = FinancialCalculations.FV((double)userRetirementPlan.ReturnRate, (double)userRetirementPlan.DesiredRetirementAge - (double)user.Age, (double)userRetiretmentData.InitialNet * -12, (double)-userRetiretmentData.InitialNet) * (Math.Pow(1 + (double)userRetirementPlan.ReturnRate, (double)userRetirementPlan.DesiredRetirementAge - (double)user.Age));
                        userRetiretmentData.LikelyRetirementAge = Math.Round((((double)userRetirementPlan.DesiredRetirementSum - (double)userRetiretmentData.LikelyRetirementSum) / (((double)userRetiretmentData.TotalMonthlySavings + (double)userRetirementPlan.DesiredRetirementIncome) * 12)) + (double)userRetirementPlan.DesiredRetirementAge, MidpointRounding.AwayFromZero);

                        break;

                    //Variable legacy
                    case RetirementGoalsEnumDto.DesiredRetirementAge:
                        if (userRetirementPlan.DesiredLegacyAmount == null)
                            userRetirementPlan.DesiredLegacyAmount = 0;

                        userRetiretmentData.DesiredRetirementSum = FinancialCalculations.PV((double)userRetirementPlan.ReturnRate, countryData.LifeExpectancy - (double)userRetirementPlan.DesiredRetirementAge, (double)userRetirementPlan.DesiredRetirementIncome * -12, (double)userRetirementPlan.DesiredLegacyAmount) * (Math.Pow(1 + (double)userRetirementPlan.ReturnRate, (double)userRetirementPlan.DesiredRetirementAge - (double)user.Age));
                        userRetiretmentData.InitialNet = userRetirementPlan.InitialSaved + userRetirementPlan.InitialOwed;
                        userRetiretmentData.RequiredSavings = FinancialCalculations.PMT((double)userRetirementPlan.ReturnRate, (double)userRetirementPlan.DesiredRetirementAge - (double)user.Age, (double)userRetiretmentData.InitialNet, -(double)userRetiretmentData.DesiredRetirementSum) / 12;
                        userRetiretmentData.TotalMonthlySavings = userRetirementPlan.TotalMonthlyIncome - userRetirementPlan.TotalMonthlyExpences;
                        userRetiretmentData.LikelyRetirementSum = FinancialCalculations.FV((double)userRetirementPlan.ReturnRate, (double)userRetirementPlan.DesiredRetirementAge - (double)user.Age, (double)userRetiretmentData.TotalMonthlySavings * -12, (double)-userRetiretmentData.InitialNet) * (Math.Pow(1 + (double)userRetirementPlan.ReturnRate, (double)userRetirementPlan.DesiredRetirementAge - (double)user.Age));
                        userRetiretmentData.LikelyRetirementLegacy = FinancialCalculations.FV((double)userRetirementPlan.ReturnRate, countryData.LifeExpectancy - (double)userRetirementPlan.DesiredRetirementAge, (double)userRetirementPlan.DesiredRetirementIncome * 12, -(double)userRetiretmentData.LikelyRetirementSum);

                        break;

                    case RetirementGoalsEnumDto.PlaceToLiveAfterRetirement:

                        break;

                    default:

                        userRetiretmentData.DesiredRetirementAge = countryData.RetirementAge;
                        userRetiretmentData.DesiredLegacyAmount = 0;

                        break;
                }

                await _userRetirementPlanRepository.UpdateAsync(userRetiretmentData);
            }
            catch (UserFriendlyException e)
            {
                throw new UserFriendlyException(e.Message);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<bool> CheckEmailExists(string email)
        {
            try
            {
                var status = await UserManager.CheckDuplicateUsernameOrEmailAddressAsync(0, email, email);
                return status.Succeeded;
            }
            catch(UserFriendlyException ue)
            {
                if (ue.Message.Contains("is already taken"))
                    return false;
                throw new UserFriendlyException(ue.Message);
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<GraphDataDto> GenGraphData(AutumnUserDto userInfo)
        {
            try
            {
                userInfo.ReturnRate = 0;
                var countryData = _countryRepository.FirstOrDefault(userInfo.CountryId);
                GraphDataDto data = new GraphDataDto()
                {
                    CurrentAge = userInfo.Age,
                    DesRetSum = userInfo.DesiredRetirementSum == null ? 0 : (double)userInfo.DesiredRetirementSum,
                    LifeExpectancy = countryData.LifeExpectancy,
                    LikRetLegacies = new List<double>(),
                    LikRetSums = new List<double>(),
                    RetirementAges = new List<int>()
                };
                if(userInfo.RetirementGoalOptions == RetirementGoalsEnumDto.DesiredRetirementSum || 
                    userInfo.RetirementGoalOptions == RetirementGoalsEnumDto.DesiredRetirementIncome || 
                    userInfo.RetirementGoalOptions == RetirementGoalsEnumDto.DesiredRetirementAge)
                {
                    userInfo.DesiredLegacyAmount = 0;
                }
                for (int i = userInfo.Age + 1, j = 0; i < countryData.LifeExpectancy; i++)
                {
                    data.RetirementAges.Add(i);
                    switch (userInfo.RetirementGoalOptions)
                    {
                        //Variable legacy
                        case RetirementGoalsEnumDto.DesiredRetirementSum:

                            userInfo.DesiredRetirementIncome = FinancialCalculations.PMT((double)userInfo.ReturnRate, i - userInfo.Age, -(double)userInfo.DesiredRetirementSum, (double)userInfo.DesiredLegacyAmount) / 12;
                            userInfo.InitialNet = userInfo.InitialSaved - userInfo.InitialOwed;
                            //userInfo.RequiredSavings = FinancialCalculations.PMT((double)userInfo.ReturnRate, i - userInfo.Age, (double)userInfo.InitialNet, -(double)userInfo.DesiredRetirementSum) / 12;
                            userInfo.TotalMonthlySavings = userInfo.TotalMonthlyIncome - userInfo.TotalMonthlyExpences;
                            data.LikRetSums.Add(FinancialCalculations.FV((double)userInfo.ReturnRate, i - userInfo.Age, (double)userInfo.TotalMonthlySavings * -12, (double)-userInfo.InitialNet) * (Math.Pow(1 + (double)userInfo.ReturnRate, i - userInfo.Age)));
                            data.LikRetLegacies.Add(FinancialCalculations.FV((double)userInfo.ReturnRate, countryData.LifeExpectancy - i, (double)userInfo.DesiredRetirementIncome * 12, -(double)data.LikRetSums[j]));

                            break;

                        //Variable legacy
                        case RetirementGoalsEnumDto.DesiredRetirementIncome:

                            userInfo.DesiredRetirementSum = FinancialCalculations.PV((double)userInfo.ReturnRate, countryData.LifeExpectancy - i, (double)userInfo.DesiredRetirementIncome * -12, (double)userInfo.DesiredLegacyAmount) * (Math.Pow(1 + (double)userInfo.ReturnRate, i - userInfo.Age));
                            userInfo.InitialNet = userInfo.InitialSaved - userInfo.InitialOwed;
                            //userInfo.RequiredSavings = FinancialCalculations.PMT((double)userInfo.ReturnRate, i - userInfo.Age, (double)userInfo.InitialNet, -(double)userInfo.DesiredRetirementSum) / 12;
                            userInfo.TotalMonthlySavings = userInfo.TotalMonthlyIncome - userInfo.TotalMonthlyExpences;
                            data.LikRetSums.Add(FinancialCalculations.FV((double)userInfo.ReturnRate, i - userInfo.Age, (double)userInfo.TotalMonthlySavings * -12, (double)-userInfo.InitialNet) * Math.Pow(1 + (double)userInfo.ReturnRate, i - userInfo.Age));
                            data.LikRetLegacies.Add(FinancialCalculations.FV((double)userInfo.ReturnRate, countryData.LifeExpectancy - i, (double)userInfo.DesiredRetirementIncome * 12, -(double)data.LikRetSums[j]));

                            break;

                        //Variable age
                        case RetirementGoalsEnumDto.DesiredLegacyAmount:

                            userInfo.DesiredRetirementAge = i;
                            userInfo.DesiredRetirementSum = FinancialCalculations.PV((double)userInfo.ReturnRate, countryData.LifeExpectancy - i, (double)userInfo.DesiredRetirementIncome * -12, (double)userInfo.DesiredLegacyAmount) * (Math.Pow(1 + (double)userInfo.ReturnRate, i - userInfo.Age));
                            userInfo.InitialNet = userInfo.InitialSaved - userInfo.InitialOwed;
                            userInfo.TotalMonthlySavings = userInfo.TotalMonthlyIncome - userInfo.TotalMonthlyExpences;
                            //userInfo.RequiredSavings = FinancialCalculations.PMT((double)userInfo.ReturnRate, i - userInfo.Age, (double)userInfo.InitialNet, -(double)userInfo.DesiredRetirementSum) / 12;
                            data.LikRetSums.Add(FinancialCalculations.FV((double)userInfo.ReturnRate, i - userInfo.Age, (double)userInfo.InitialNet * -12, (double)-userInfo.InitialNet) * Math.Pow(1 + (double)userInfo.ReturnRate, i - userInfo.Age));
                            //userInfo.LikelyRetirementAge = Math.Round((((double)userInfo.DesiredRetirementSum - (double)data.LikRetSums[j]) / (((double)userInfo.TotalMonthlySavings + (double)userInfo.DesiredRetirementIncome) * 12)) + i, MidpointRounding.AwayFromZero);

                            break;

                        //Variable legacy
                        case RetirementGoalsEnumDto.DesiredRetirementAge:

                            userInfo.DesiredRetirementSum = FinancialCalculations.PV((double)userInfo.ReturnRate, countryData.LifeExpectancy - i, (double)userInfo.DesiredRetirementIncome * -12, (double)userInfo.DesiredLegacyAmount) * (Math.Pow(1 + (double)userInfo.ReturnRate, i - userInfo.Age));
                            userInfo.InitialNet = userInfo.InitialSaved - userInfo.InitialOwed;
                            //userInfo.RequiredSavings = FinancialCalculations.PMT((double)userInfo.ReturnRate, i - userInfo.Age, (double)userInfo.InitialNet, -(double)userInfo.DesiredRetirementSum) / 12;
                            userInfo.TotalMonthlySavings = userInfo.TotalMonthlyIncome - userInfo.TotalMonthlyExpences;
                            data.LikRetSums.Add(FinancialCalculations.FV((double)userInfo.ReturnRate, i - userInfo.Age, (double)userInfo.TotalMonthlySavings * -12, (double)-userInfo.InitialNet) * Math.Pow(1 + (double)userInfo.ReturnRate, i - userInfo.Age));
                            data.LikRetLegacies.Add(FinancialCalculations.FV((double)userInfo.ReturnRate, countryData.LifeExpectancy - i, (double)userInfo.DesiredRetirementIncome * 12, -(double)data.LikRetSums[j]));

                            break;

                        case RetirementGoalsEnumDto.PlaceToLiveAfterRetirement:

                            break;

                        default:

                            userInfo.DesiredRetirementAge = i;
                            userInfo.DesiredLegacyAmount = 0;

                            break;
                    }
                }
                return data;
            }
            catch (UserFriendlyException e)
            {
                throw new UserFriendlyException(e.Message);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
