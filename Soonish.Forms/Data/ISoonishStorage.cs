using System.Collections.ObjectModel;
using System.Threading.Tasks;
namespace Soonish.Forms.Data
{
    public interface ISoonishStorage
    {
        Task Insert(Response entity);
        Task<ReadOnlyCollection<Response>> GetResponsesForYear(int year);
        Task<ReadOnlyCollection<Response>> GetResponsesForMonth(int year, int month);
    }
}
