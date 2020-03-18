using System.Collections.Generic;
using Autumn.Authorization.Users.Dto;
using Autumn.Dto;

namespace Autumn.Authorization.Users.Exporting
{
    public interface IUserListExcelExporter
    {
        FileDto ExportToFile(List<UserListDto> userListDtos);
    }
}