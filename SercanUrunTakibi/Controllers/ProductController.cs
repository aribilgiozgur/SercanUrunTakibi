using SercanUrunTakibi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SercanUrunTakibi.Controllers
{
    public class ProductController : Controller
    {
        //
        // GET: /Product/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List(int? offset)
        {
            if (offset == null) offset = 0;
            Product p = new Product();
            List<Product> products = p.getAllProducts(50, offset);
            return View(products);
        }

        public ActionResult Insert() {
            return View();
        }

        [HttpPost]
        public ActionResult Insert(FormCollection f)
        {
            Object[] paramArray = new Object[11];
            for (int i = 0; i < f.Count; i++)
            {
                paramArray[i] = f[i];
            }
            Product p = new Product(paramArray, 1);
            return View("Index");
        }

    }
}
