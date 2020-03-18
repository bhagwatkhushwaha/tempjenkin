using System.Collections.Generic;
using Autumn.Authorization.Users.Importing.Dto;
using Abp.Dependency;

namespace Autumn.Authorization.Users.Importing
{
    public interface IUserListExcelDataReader: ITransientDependency
    {
        List<ImportUserDto> GetUsersFromExcel(byte[] fileBytes);
    }
}
