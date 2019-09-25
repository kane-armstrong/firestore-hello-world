using System.Threading.Tasks;

namespace Server.Application
{
    public interface IAdviserNotifier
    {
        Task NotifyVisitorArrived(int adviserId, VisitorDetails visitor);
    }
}