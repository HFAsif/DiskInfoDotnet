namespace CrystalDiskInfoDotnet;

#if LoggerExist
using Microsoft.Extensions.Logging;
#else
using HelperClass;
#endif
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

    public void PrimaryWorker(
#if LoggerExist
        ILogger logger
#endif
        )
    {
        if (targetFrameworkAttribute != null)
        {
#if LoggerExist
            logger.LogInformation($"Target Framework: {targetFrameworkAttribute.FrameworkName}");
#else
            InternalLogger.MyLogs($"Target Framework: {targetFrameworkAttribute.FrameworkName}");
#endif
        }
        else
        {
#if LoggerExist
            logger.LogInformation("TargetFrameworkAttribute not found on this assembly.");
#else
            InternalLogger.MyLogs("TargetFrameworkAttribute not found on this assembly.");
#endif
        }
    }
}
