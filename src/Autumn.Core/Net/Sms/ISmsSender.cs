using System.Threading.Tasks;

namespace Autumn.Net.Sms
{
    public interface ISmsSender
    {
        Task SendAsync(string number, string message);
    }
}