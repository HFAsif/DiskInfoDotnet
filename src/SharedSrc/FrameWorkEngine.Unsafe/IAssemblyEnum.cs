using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FrameWorkEngine;
[ComImport]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[Guid("21b8916c-f28e-11d2-a473-00c04f8ef448")]
internal interface IAssemblyEnum
{
    [PreserveSig]
    int GetNextAssembly(out IApplicationContext ppAppCtx, out IAssemblyName ppName, uint dwFlags);

    [PreserveSig]
    int Reset();

    [PreserveSig]
    int Clone(out IAssemblyEnum ppEnum);
}

