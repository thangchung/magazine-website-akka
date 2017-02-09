using System.Threading.Tasks;
using Cik.Magazine.Core.Views;

namespace Cik.Magazine.Core.Services
{
    public interface ICategoryService
    {
        Task<object> Create(CategoryDto cat);
    }
}