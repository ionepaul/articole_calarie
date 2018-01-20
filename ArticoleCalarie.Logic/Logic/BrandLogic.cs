using System.Collections.Generic;
using System.Linq;
using ArticoleCalarie.Logic.Converters;
using ArticoleCalarie.Logic.ILogic;
using ArticoleCalarie.Models;
using ArticoleCalarie.Repository.IRepository;

namespace ArticoleCalarie.Logic.Logic
{
    public class BrandLogic : IBrandLogic
    {
        private IBrandRepository _iBrandRepository;

        public BrandLogic(IBrandRepository iBrandRepository)
        {
            _iBrandRepository = iBrandRepository;
        }

        public IEnumerable<BrandViewModel> GetAllBrands(string searchTerm = "")
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                var brands = _iBrandRepository.GetAll().Select(x => x.ToViewModel());

                return brands;
            }

            var brandsByTerm = _iBrandRepository.GetBrandsBySearchTerm(searchTerm).Select(x => x.ToViewModel());

            return brandsByTerm;
        }
    }
}
