using System.Collections.Generic;
using ArticoleCalarie.Models;

namespace ArticoleCalarie.Logic.ILogic
{
    public interface IColorLogic
    {
        IEnumerable<ColorViewModel> GetAllColors();
    }
}
