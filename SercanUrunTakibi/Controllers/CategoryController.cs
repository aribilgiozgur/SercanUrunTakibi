using SercanUrunTakibi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SercanUrunTakibi.Controllers
{
    public class CategoryController : Controller
    {
        //
        // GET: /Category/

        public ActionResult Index()
        {
            Category c = new Category();
            return View(c.getAll());
        }

        public ActionResult Insert()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Insert(FormCollection f)
        {
            // int userId = int.Parse(Session["userId"].ToString());
            String categoryName = f["category-name"].ToString();
            Category c = new Category(categoryName, 1);
            return RedirectToAction("Index");
        }



    }
}
