using System.Collections.Generic;
using ArticoleCalarie.Repository.Entities;

namespace ArticoleCalarie.Repository.IRepository
{
    public interface ISubcategoryRepository : IAbstractRepository<Subcategory>
    {
        IEnumerable<Subcategory> GetAllByCategoryId(int categoryId);
        IEnumerable<Subcategory> GetAllByCategoryIdAndSearchTerm(int categoryId, string searchTerm = "");
    }
}
