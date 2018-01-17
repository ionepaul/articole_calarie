using System.Collections.Generic;
using ArticoleCalarie.Models;

namespace ArticoleCalarie.Logic.ILogic
{
    public interface ICategoryLogic
    {
        IEnumerable<CategoryViewModel> GetAllCategories(string term = "");
    }
}
