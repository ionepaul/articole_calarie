using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ArticoleCalarie.Logic.ILogic;
using ArticoleCalarie.Models;
using ArticoleCalarie.Web.Utils;

namespace ArticoleCalarie.Web.Controllers
{
    public class SitemapController : Controller
    {
        private ISubcategoryLogic _iSubcategoryLogic;
        private IProductLogic _iProductLogic;

        public SitemapController(ISubcategoryLogic iSubcategoryLogic, IProductLogic iProductLogic)
        {
            _iSubcategoryLogic = iSubcategoryLogic;
            _iProductLogic = iProductLogic;
        }
        
        [OutputCache(Duration = 120, VaryByParam = "none")]
        public SitemapActionResult Index()
        {
            var website = "https://www.articolecalarie.ro";
            var dateAdded = new DateTime(2018, 6, 17);
            var sitemapItems = new List<SitemapItem>
            {
                new SitemapItem
                {
                    URL = "",
                    Priority = "1",
                    DateAdded = dateAdded
                },
                new SitemapItem
                {
                    URL = "/account/login",
                    Priority = ".8",
                    DateAdded = dateAdded
                },
                new SitemapItem
                {
                    URL = "/account/register",
                    Priority = ".8",
                    DateAdded = dateAdded
                },
                new SitemapItem
                {
                    URL = "/contact",
                    Priority = ".8",
                    DateAdded = dateAdded
                },
                new SitemapItem
                {
                    URL = "/produse/noutati",
                    Priority = ".9",
                    DateAdded = dateAdded
                },
                new SitemapItem
                {
                    URL = "/produse/oferte",
                    Priority = ".9",
                    DateAdded = dateAdded
                },
                new SitemapItem
                {
                    URL = "/termeni-si-conditii",
                    Priority = ".7",
                    DateAdded = dateAdded
                },
                new SitemapItem
                {
                    URL = "/politica-de-confidentialitate",
                    Priority = ".7",
                    DateAdded = dateAdded
                }
                ,
                new SitemapItem
                {
                    URL = "/calaret/subcategorii",
                    Priority = ".8",
                    DateAdded = dateAdded
                },
                new SitemapItem
                {
                    URL = "/cal/subcategorii",
                    Priority = ".8",
                    DateAdded = dateAdded
                }
            };

            var subcategories = _iSubcategoryLogic.GetAllSubcategories();


            foreach (var subcategory in subcategories)
            {
                sitemapItems.Add(new SitemapItem
                {
                    URL = $"/produse/{subcategory.CategoryName.ToLower().Trim()}/{subcategory.Id}/{subcategory.Name.ToUrlSubcategoryName()}",
                    Priority = ".9",
                    DateAdded = dateAdded
                });

                if (subcategory.Products != null && subcategory.Products.Count() > 0)
                {
                    foreach(var product in subcategory.Products)
                    {
                        sitemapItems.Add(new SitemapItem
                        {
                            URL = $"/produse/{subcategory.CategoryName}/{subcategory.Id}/{subcategory.Name.ToUrlSubcategoryName()}/{product.ProductCode.ToLower().Trim()}/{product.ProductName.ToUrlProductName()}",
                            Priority = ".9",
                            DateAdded = product.DateAdded > dateAdded ? product.DateAdded : dateAdded
                        });
                    }
                }
            }

            return new SitemapActionResult(sitemapItems, website);
        }
    }
}