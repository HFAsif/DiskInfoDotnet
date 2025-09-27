using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameWorkEngine;
internal enum CompatibilityFlag
{
    SwallowUnhandledExceptions,
    NullReferenceExceptionOnAV,
    EagerlyGenerateRandomAsymmKeys,
    FullTrustListAssembliesInGac,
    DateTimeParseIgnorePunctuation,
    OnlyGACDomainNeutral,
    DisableReplacementCustomCulture
}
#if false // Decompilation log
'10' items in cache
#endif
