using System;
using System.Configuration;
using ArticoleCalarie.Models;
using ArticoleCalarie.Repository.Models;

namespace ArticoleCalarie.Logic.Converters
{
    public static class SearchConverter
    {
        public static SearchModel ToDbSearchModel(this SearchViewModel searchViewModel)
        {
            var itemsPerPage = Convert.ToInt32(ConfigurationManager.AppSettings["ProductsPerPage"]);

            var searchModel = new SearchModel
            {
                SubcategoryId = searchViewModel.SubcategoryId,
                ItemsPerPage = itemsPerPage,
                ItemsToSkip = (searchViewModel.PageNumber - 1) * itemsPerPage
            };

            return searchModel;
        }
    }
}
