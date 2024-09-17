using System.Threading.Tasks;

namespace WeeklyReminder.Domain.Services;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync();
}