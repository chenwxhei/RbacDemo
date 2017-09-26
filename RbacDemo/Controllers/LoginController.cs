using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RbacDemo.Models;
using RbacDemo.Filters;

namespace RbacDemo.Controllers
{
    [CustomAuthrozi(AuthorizationType=AuthorizationType.None)]
    public class LoginController : Controller
    {
        Rbac db = new Rbac();
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(User LoginUser)
        {
            //模型绑定验证
            if (!ModelState.IsValid)
            {
                return Json(new { code = 400 }); 
            }
            //查找用户
            var user = db.Users.FirstOrDefault(
                u => u.Username == LoginUser.Username && u.Password == LoginUser.Password);
            //如果没有找到用户,就返回404
            if (user == null) return Json(new { code = 404 });
            //设置Session，作为身份验证的标识
            Session["user"] = user;

            /*第一种方式，支持多角色选择
            //获取所有角色的所有模块,转换成字典类，Key是角色ID，value是角色所有用的模块集合
            var roleModules = user.Roles.ToDictionary(r => r.Id, r => r.Modules);
            //存入到Session，之后在复用，不同再去查数据库
            Session["roleModules"] = roleModules;

            //获取用户角色表
            var roles = user.Roles.ToList();
            //存入到Sess，以便以后复用
            Session["roles"] = roles;
           
            //设置默认角色为用户角色表的第一个
            Session["role"] = roles[0];
            */

            //泛型委托
            //Func<T,T....TResult>

            //第二种形式，内联委托
            //Func<Role,bool> funcl = delegate(Role rollel)
            //{
            //    if (rollel.Id == 3) return true;
            //    return false; 
            //};

            //第三种形式，lamdba表达式代替了内联委托
            //Func<Role, bool> funcl = rolel =>
            // {
            //     return true;
            // };

            //第四种形式，简化lamdba表达式的方法体，如果只有一句返回值的语句，就可以省略{}和return
            //Func<Role, bool> funcl = rolel => true;

            //r=>true，就是只拿出一条数据
            var role = user.Roles.FirstOrDefault(rolel => true);

            Session["role"] = role;

            return Json(new { code = 200 });
        }
        /// <summary>
        /// 第一种委托形式，创建一个方法，传给委托
        /// </summary>
        /// <param name="rolel"></param>
        /// <returns></returns>
        public bool Funcl(Role rolel)
        {
            if (rolel.Id == 1) return true;
            return false;
        }
    }
}