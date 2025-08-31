using System.Net.Sockets;

namespace DiscountCodeServer
{
    public interface IHandler
    {
        public Task ExecuteAction();
    }
}
