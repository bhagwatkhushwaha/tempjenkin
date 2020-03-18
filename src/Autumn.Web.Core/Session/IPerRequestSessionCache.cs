using System.Threading.Tasks;
using Autumn.Sessions.Dto;

namespace Autumn.Web.Session
{
    public interface IPerRequestSessionCache
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformationsAsync();
    }
}
