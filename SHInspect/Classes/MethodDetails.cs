using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SHInspect.Classes
{
    public class MethodDetails
    {
        public MethodDetails(MethodInfo method, object targetObject)
        {
            Method = method;
            TargetObject = targetObject;
        }
        public MethodInfo Method;
        public object TargetObject;
    }
}
