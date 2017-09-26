using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RbacDemo.Filters;

namespace RbacDemo
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilter(GlobalFilterCollection filter)
        {
            filter.Add(new CustomAuthroziAttribute());
        }
    }
}