using System.Collections.Generic;
using System.Linq;
using ArticoleCalarie.Logic.Converters;
using ArticoleCalarie.Logic.ILogic;
using ArticoleCalarie.Models;
using ArticoleCalarie.Repository.IRepository;

namespace ArticoleCalarie.Logic.Logic
{
    public class ColorLogic : IColorLogic
    {
        private IColorRepository _iColorRepository;

        public ColorLogic(IColorRepository iColorRepository)
        {
            _iColorRepository = iColorRepository;
        }

        public IEnumerable<ColorViewModel> GetAllColors()
        {
            var colors = _iColorRepository.GetAll();

            return colors.Select(x => x.ToViewModel());
        }
    }
}
