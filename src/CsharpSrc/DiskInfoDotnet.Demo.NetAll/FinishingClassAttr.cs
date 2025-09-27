namespace DiskInfoDotnet.Demo.NetAll;
using HelperClass;
using System;
using System.Reflection;


[AttributeUsage(AttributeTargets.Method)]
internal class FinishingClassAttr : Attribute
{
    private readonly System.Runtime.Versioning.TargetFrameworkAttribute targetFrameworkAttribute;
    public FinishingClassAttr() 
    {
        targetFrameworkAttribute
            = typeof(Program).Assembly.GetCustomAttribute<System.Runtime.Versioning.TargetFrameworkAttribute>();
    }

    public void PrimaryWorker(out string logs)
    {
        if (targetFrameworkAttribute != null)
        {
            logs = ($"Target Framework: {targetFrameworkAttribute.FrameworkName}");
        }
        else
        {
            logs = ("TargetFrameworkAttribute not found on this assembly.");
        }
    }
}
