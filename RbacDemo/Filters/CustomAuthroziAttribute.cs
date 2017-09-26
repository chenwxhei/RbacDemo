using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RbacDemo.Models;

namespace RbacDemo.Filters
{
    public enum AuthorizationType {None,Identity, Authorization }
    //自定义的授权过滤器
    public class CustomAuthroziAttribute : FilterAttribute, IAuthorizationFilter
    {
        /// <summary>
        /// 授权认证类型属性
        /// </summary>
        public AuthorizationType AuthorizationType { get; set; } = AuthorizationType.Authorization;
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            //1.无限制
            if (AuthorizationType == AuthorizationType.None) return;
            //2.身份验证,如果不通过，则定向到登陆页面
            if (filterContext.HttpContext.Session["user"] == null)
            {
                RedirecToLogin(filterContext);
                return;
            }

            //如果请求的控制器属于dentity，就不再进行授权验证，直接返回
            if (AuthorizationType == AuthorizationType.Identity) return;
            //3.授权验证
            //3-1获取当前请求的控制器名称
            var controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            //3-2从Session里拿到用户登陆的角色
            var role = filterContext.HttpContext.Session["role"] as Role;
            //3-3如果角色为空，说明用户信息不完整，所以返回
            if (role == null)
            {
                RedirecToLogin(filterContext);
                return;
            }
            //3-4查找角色模块里的控制器是否存在 我们请求的控制器
            var module = role.Modules.FirstOrDefault(m => m.Controller == controller);
            //3-5如果不存在，就定向到登陆页
            if (module == null)
            {
                RedirecToLogin(filterContext);
            }

            //等同于3-4，3-5
            //Func<Module, bool> funcl = module =>
            //{
            //    return module.Controller != controller;
            //};

            //if (role.Modules.All(funcl))
            //{
            //    RedirecToLogin(filterContext);
            //}

        }
        /// <summary>
        /// 重新定向到登陆页面
        /// </summary>
        /// <param name="filterContext"></param>
        public void RedirecToLogin(AuthorizationContext filterContext)
        {
            var url = new UrlHelper(filterContext.RequestContext);
            filterContext.Result = new RedirectResult(url.Action("Index", "Login"));
        }
    }
}