using System.Collections.Generic;
using System.Threading.Tasks;
using Abp;
using Autumn.Dto;

namespace Autumn.Gdpr
{
    public interface IUserCollectedDataProvider
    {
        Task<List<FileDto>> GetFiles(UserIdentifier user);
    }
}
