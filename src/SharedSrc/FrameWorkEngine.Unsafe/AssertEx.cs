using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace FrameWorkEngine;
internal class AssertEx
{
}

internal static class Assert
{
    internal const int COR_E_FAILFAST = -2146232797;

    private static AssertFilter Filter;

    //static Assert()
    //{
    //    Filter = new DefaultFilter();
    //}

    internal static void Check(bool condition, string conditionString, string message)
    {
        if (!condition)
        {
            Fail(conditionString, message, null, -2146232797);
        }
    }

    internal static void Check(bool condition, string conditionString, string message, int exitCode)
    {
        if (!condition)
        {
            Fail(conditionString, message, null, exitCode);
        }
    }

    internal static void Fail(string conditionString, string message)
    {
        Fail(conditionString, message, null, -2146232797);
    }

    internal static void Fail(string conditionString, string message, string windowTitle, int exitCode)
    {
        Fail(conditionString, message, windowTitle, exitCode, TraceFormat.Normal, 0);
    }

    internal static void Fail(string conditionString, string message, int exitCode, TraceFormat stackTraceFormat)
    {
        Fail(conditionString, message, null, exitCode, stackTraceFormat, 0);
    }

    [SecuritySafeCritical]
    internal static void Fail(string conditionString, string message, string windowTitle, int exitCode, TraceFormat stackTraceFormat, int numStackFramesToSkip)
    {
        StackTrace location = new StackTrace(numStackFramesToSkip, fNeedFileInfo: true);
        switch (Filter.AssertFailure(conditionString, message, location, stackTraceFormat, windowTitle))
        {
            case AssertFilters.FailDebug:
                if (Debugger.IsAttached)
                {
                    Debugger.Break();
                }
                else if (!Debugger.Launch())
                {
                    throw new InvalidOperationException(EnvironmentEx.GetResourceString("InvalidOperation_DebuggerLaunchFailed"));
                }
                break;
            case AssertFilters.FailTerminate:
                if (Debugger.IsAttached)
                {
                    EnvironmentEx._Exit(exitCode);
                }
                else
                {
                    EnvironmentEx.FailFast(message, (uint)exitCode);
                }
                break;
        }
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    [SecurityCritical]
    internal static extern int ShowDefaultAssertDialog(string conditionString, string message, string stackTrace, string windowTitle);
}
