using System.Collections.Generic;
using Autumn.Authorization.Users.Importing.Dto;
using Autumn.Dto;

namespace Autumn.Authorization.Users.Importing
{
    public interface IInvalidUserExporter
    {
        FileDto ExportToFile(List<ImportUserDto> userListDtos);
    }
}
