using System.Threading.Tasks;
using ArticoleCalarie.Models;

namespace ArticoleCalarie.Logic.ILogic
{
    public interface ICategoryLogic
    {
        Task AddCategory(CategoryViewModel categoryViewModel);
    }
}
