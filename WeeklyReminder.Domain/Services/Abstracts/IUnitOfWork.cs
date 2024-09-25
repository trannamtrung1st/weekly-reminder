using System.Threading.Tasks;

namespace WeeklyReminder.Domain.Services.Abstracts;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync();
}