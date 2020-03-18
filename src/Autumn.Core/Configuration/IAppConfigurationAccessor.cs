using Microsoft.Extensions.Configuration;

namespace Autumn.Configuration
{
    public interface IAppConfigurationAccessor
    {
        IConfigurationRoot Configuration { get; }
    }
}
