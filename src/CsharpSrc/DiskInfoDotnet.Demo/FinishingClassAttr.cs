namespace DiskInfoDotnet.Demo;

using Microsoft.Extensions.Logging;
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

    public void PrimaryWorker(ILogger logger)
    {
        if (targetFrameworkAttribute != null)
        {
            logger.LogInformation($"Target Framework: {targetFrameworkAttribute.FrameworkName}");
        }
        else
        {
            logger.LogInformation("TargetFrameworkAttribute not found on this assembly.");
        }
    }
}
