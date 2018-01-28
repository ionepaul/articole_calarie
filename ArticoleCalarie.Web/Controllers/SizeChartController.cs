using System.Web.Mvc;
using ArticoleCalarie.Logic.ILogic;

namespace ArticoleCalarie.Web.Controllers
{
    public class SizeChartController : Controller
    {
        private ISizeChartLogic _iSizeChartLogic;
        
        public SizeChartController(ISizeChartLogic iSizeChartLogic)
        {
            _iSizeChartLogic = iSizeChartLogic;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public JsonResult GetSizeCharts()
        {
            return Json(_iSizeChartLogic.GetAllSizeCharts(), JsonRequestBehavior.AllowGet);
        }
    }
}