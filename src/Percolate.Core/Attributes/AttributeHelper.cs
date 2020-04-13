using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using System;
using System.Linq;
using System.Reflection;

namespace Percolate.Core.Attributes
{
    public static class AttributeHelper
    {
        public static TAttribute GetAttributeFromAction<TAttribute>(ActionDescriptor actionDescriptor) where TAttribute : Attribute
        {
            TAttribute attribute = null;

            if (actionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
            {
                attribute = controllerActionDescriptor
                    .MethodInfo
                    .GetCustomAttributes(typeof(TAttribute))
                    .SingleOrDefault() as TAttribute;
            }

            return attribute;
        }

        public static TAttribute GetAttributeFromController<TAttribute>(ActionDescriptor actionDescriptor) where TAttribute : Attribute
        {
            TAttribute attribute = null;

            if (actionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
            {
                attribute = controllerActionDescriptor
                    .ControllerTypeInfo
                    .GetCustomAttributes(typeof(TAttribute))
                    .SingleOrDefault() as TAttribute;
            }

            return attribute;
        }
    }
}
