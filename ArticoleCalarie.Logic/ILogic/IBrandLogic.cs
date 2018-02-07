using System.Collections.Generic;
using ArticoleCalarie.Models;

namespace ArticoleCalarie.Logic.ILogic
{
    public interface IBrandLogic
    {
        IEnumerable<BrandViewModel> GetAllBrands(string searchTerm = "");
    }
}
