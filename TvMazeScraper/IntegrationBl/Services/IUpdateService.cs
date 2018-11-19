using System.Threading;
using System.Threading.Tasks;

namespace IntegrationBl.Services
{
    public interface IUpdateService
    {
        Task StartUpdateProcessAsync(CancellationToken cancellationToken);
    }
}