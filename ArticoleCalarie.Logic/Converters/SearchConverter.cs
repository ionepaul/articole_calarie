using System;
using System.Collections.Generic;
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
                ItemsToSkip = (searchViewModel.PageNumber - 1) * itemsPerPage,
                MinPrice = searchViewModel.MinPrice,
                MaxPrice = searchViewModel.MaxPrice
            };

            if (!string.IsNullOrEmpty(searchViewModel.Sizes))
            {
                searchModel.Sizes = new List<string>();

                var sizes = searchViewModel.Sizes.Split(',');

                foreach (var size in sizes)
                {
                    searchModel.Sizes.Add(size);
                }
            }

            if (!string.IsNullOrEmpty(searchViewModel.ColorIds))
            {
                searchModel.ColorIds = new List<int>();

                var colorIds = searchViewModel.ColorIds.Split(',');

                foreach(var color in colorIds)
                {
                    var colorId = Convert.ToInt32(color);

                    searchModel.ColorIds.Add(colorId);
                }
            }

            return searchModel;
        }
    }
}
