using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RbacDemo.Models;

namespace RbacDemo.Controllers
{
    public class UserController : Controller
    {
        Rbac db = new Rbac();
        // GET: User
        public ActionResult Index()
        {
            return View(db.Users);
        }
        public ActionResult Create()
        {
            return PartialView();
        }

        public ActionResult Edit(int id)
        {
            var module = db.Users.FirstOrDefault(m => m.Id == id);
            if (module == null) return Content("未找到要编辑的模块");

            return View(module);
        }

        public ActionResult Delete(int id)
        {
            //实例化一个module，并把要删除的id，初始化
            User module = new User();
            //将要删除的实体，先附加到数据上下文，就像从数据库查出来一样
            module.Id = id;
            db.Users.Attach(module);
            //删除实体
            db.Users.Remove(module);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Save(User module)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { code = 400 });
            }
            //新增或更新实体
            db.Users.AddOrUpdate(module);
            //持久化输出
            db.SaveChanges();
            return Json(new { code = 200 });
        }
    }
}