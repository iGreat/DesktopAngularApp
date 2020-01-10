using System;

namespace DesktopApp.Common.Attribute
{
    [AttributeUsage(AttributeTargets.Class)]
    public class UnityServiceAttribute : System.Attribute
    {
        public UnityServiceAttribute(Type @interface)
        {
            Interface = @interface;
        }

        public Type Interface { get; }


    }
}
