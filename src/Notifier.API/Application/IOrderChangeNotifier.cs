using System.Threading.Tasks;

namespace Notifier.API.Application
{
    public interface IOrderChangeNotifier
    {
        Task NotifyOrderChanged(int locationId, int orderId);
    }
}