using System.Collections.Generic;
using System.Web.Mvc;
using System.Xml;
using ArticoleCalarie.Models;

namespace ArticoleCalarie.Web.Utils
{
    public class SitemapActionResult : ActionResult
    {
        private List<SitemapItem> _sitemapItems;
        private string _website;

        public SitemapActionResult(List<SitemapItem> sitemapItems, string website)
        {
            _sitemapItems = sitemapItems;
            _website = website;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.ContentType = "text/xml";

            using (XmlWriter writer = XmlWriter.Create(context.HttpContext.Response.Output))
            {
                writer.WriteStartElement("urlset", "http://www.sitemaps.org/schemas/sitemap/0.9");

                foreach(var sitemapItem in _sitemapItems)
                {
                    writer.WriteStartElement("url");
                    writer.WriteElementString("loc", string.Format(_website + "{0}", sitemapItem.URL));

                    if (sitemapItem.DateAdded != null)
                    {
                        writer.WriteElementString("lastmod", string.Format("{0:yyyy-MM-dd}", sitemapItem.DateAdded));
                    }

                    writer.WriteElementString("changefreq", "daily");
                    writer.WriteElementString("priority", sitemapItem.Priority);
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                writer.Flush();
                writer.Close();
            }
        }
    }
}