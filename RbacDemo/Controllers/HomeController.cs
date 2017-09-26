using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RbacDemo.Filters;
using RbacDemo.Models;

namespace RbacDemo.Controllers
{
    [CustomAuthrozi(AuthorizationType = AuthorizationType.Identity)]
    public class  HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 头部的分部视图
        /// </summary>
        /// <returns></returns>
        public ActionResult Header()
        {
            /*第一种方式  选择角色
            var user = Session["user"] as User;
            var roles = Session["roles"] as List<Role>;
            var role = Session["role"] as Role;

            List<SelectListItem> rolelist = new List<SelectListItem>();
            foreach(var itme in roles)
            {
                rolelist.Add(new SelectListItem { Text = itme.Name, Value = itme.Id.ToString(), Selected = itme.Id == role.Id });
            }
            ViewBag.roles = rolelist;
            return PartialView(user);
            */
            //二种方式
            var user = Session["user"] as User;
            var role = Session["role"] as Role;

            ViewBag.UserName = user.Username;
            ViewBag.RoleName = role.Name;
            return PartialView();


        }
        /// <summary>
        /// 导航栏的分部视图
        /// </summary>
        /// <returns></returns>
        public ActionResult Nav(int roleId=0)
        {
            /*第一种方式,  可选择用户角色
            //获取Session里的用户模块表
            var roleModules = Session["roleModules"] as Dictionary<int, ICollection<Module>>;
            //获取Session里默认的角色
            var role = Session["role"] as Role;
            var roles = Session["roles"] as List<Role>;
            //根据参数里的roleId,获取一个用户角色的模块表
            List<Module> module;
            if (roleModules.ContainsKey(roleId))
            {
                modules = roleModules[roleId].ToList();
                //切换当前角色
                Session["role"] = roles.FirstOrDefault(r=>r.Id==roleId);
            }
            else
            {
                modules = roleModules[role.Id].ToList();
            }
            */
            //第二种方式  只使用默认，不选择
            //获取Seeion里的默认角色
            var role = Session["role"] as Role;
            //从默认角色里获取模块
            var modules = role.Modules;

            return PartialView(modules);
        }
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Login");
        }
    }
}