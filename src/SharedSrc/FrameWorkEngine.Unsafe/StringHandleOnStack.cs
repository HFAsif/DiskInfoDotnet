using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameWorkEngine;
internal struct StringHandleOnStack
{
    private IntPtr m_ptr;

    internal StringHandleOnStack(IntPtr pString)
    {
        m_ptr = pString;
    }
}
