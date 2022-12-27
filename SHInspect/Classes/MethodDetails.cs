using System.Reflection;

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
