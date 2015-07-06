using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BaseProject.WebAPI.Classes
{
    public static class Extentions
    {
        public static bool HasFilter(this AuthorizationContext context, Type attributeType)
        {
            return (context.ActionDescriptor.GetCustomAttributes(attributeType, false).Any() ||
                context.ActionDescriptor.ControllerDescriptor.GetCustomAttributes(attributeType, false).Any());
        }

        public static bool HasFilter(this ActionExecutingContext context, Type attributeType)
        {
            return (context.ActionDescriptor.GetCustomAttributes(attributeType, false).Any() ||
                context.ActionDescriptor.ControllerDescriptor.GetCustomAttributes(attributeType, false).Any());
        }
    }
}
