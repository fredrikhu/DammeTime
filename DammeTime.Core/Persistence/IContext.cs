using System.Threading;
using System.Threading.Tasks;

namespace DammeTime.Core.TimeReporting.Persistence
{
    public interface IContext
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}