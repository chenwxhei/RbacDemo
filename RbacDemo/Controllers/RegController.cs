using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RbacDemo.Models;
using RbacDemo.Filters;

namespace RbacDemo.Controllers
{
    [CustomAuthrozi(AuthorizationType = AuthorizationType.None)]
    public class RegController : Controller
    {
        Rbac db = new Rbac();
        // GET: Reg
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Reg(User regUser)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { code = 400 });
            }
            try
            {
                //获取要添加的角色实体
                var role = db.Roles.FirstOrDefault(r => r.Id == 3);
                //给用户添加角色
                regUser.Roles.Add(role);
                //把注册用户添加到用户表
                db.Users.Add(regUser);
                //保存到数据库（持久化数据）
                db.SaveChanges();
            }
            catch (Exception e)
            {
                return Json(new { code = 404, msg = e.Message });
            }

            return Json(new { code = 200 });
        }
    }
}