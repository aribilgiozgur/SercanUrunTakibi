using SercanUrunTakibi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SercanUrunTakibi.Controllers
{
    public class SubCategoryController : Controller
    {
        //
        // GET: /SubCategory/

        public ActionResult Index()
        {
            SubCategory sc = new SubCategory();
            return View(sc.getAllSubCategories());
        }

        public ActionResult Insert()
        {
            Category c = new Category();
            List<Category> categories = c.getAll();
            return View(categories);
        }

        [HttpPost]
        public ActionResult Insert(FormCollection f)
        {
            int categoryId = int.Parse(f["category-id"].ToString());
            String subCategoryName = f["sub-category-name"].ToString();
            // int userId = int.Parse(Session["userId"].ToString());

            SubCategory sc = new SubCategory(categoryId, subCategoryName, 1);
            return RedirectToAction("Index");
        }

    }
}
