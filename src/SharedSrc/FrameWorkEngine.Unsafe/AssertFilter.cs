using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameWorkEngine;
[Serializable]
internal abstract class AssertFilter
{
    public abstract AssertFilters AssertFailure(string condition, string message, StackTrace location, TraceFormat stackTraceFormat, string windowTitle);
}
#if false // Decompilation log
'10' items in cache
#endif
