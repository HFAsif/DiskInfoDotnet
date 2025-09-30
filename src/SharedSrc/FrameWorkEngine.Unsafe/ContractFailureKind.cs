#if NET20 || NET35 || NET40

using System;
using System.Collections.Generic;
using System.Text;

namespace FrameWorkEngine;
//
// Summary:
//     Specifies the type of contract that failed.
[__DynamicallyInvokable]
public enum ContractFailureKind
{
    //
    // Summary:
    //     A Overload:System.Diagnostics.Contracts.Contract.Requires contract failed.
    [__DynamicallyInvokable]
    Precondition,
    //
    // Summary:
    //     An Overload:System.Diagnostics.Contracts.Contract.Ensures contract failed.
    [__DynamicallyInvokable]
    Postcondition,
    //
    // Summary:
    //     An Overload:System.Diagnostics.Contracts.Contract.EnsuresOnThrow contract failed.
    [__DynamicallyInvokable]
    PostconditionOnException,
    //
    // Summary:
    //     An Overload:System.Diagnostics.Contracts.Contract.Invariant contract failed.
    [__DynamicallyInvokable]
    Invariant,
    //
    // Summary:
    //     An Overload:System.Diagnostics.Contracts.Contract.Assert contract failed.
    [__DynamicallyInvokable]
    Assert,
    //
    // Summary:
    //     An Overload:System.Diagnostics.Contracts.Contract.Assume contract failed.
    [__DynamicallyInvokable]
    Assume
}
#if false // Decompilation log
'12' items in cache
#endif

#endif