namespace HelperClass
{
    using Microsoft.Win32.SafeHandles;
    using System;
    using System.Runtime.InteropServices;
    using static ImportantElements_Enums;

    class CAtaSmartArtificialBase : CAtaSmartArtificialBaseBase
    {
        [DllImport("advapi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        protected static extern bool InitializeSecurityDescriptor([Out] SECURITY_DESCRIPTOR[] pSecurityDescriptor, [In] uint dwRevision);

        [DllImport("advapi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        protected static extern bool InitializeAcl(IntPtr pAcl, [In] uint nAclLength, [In] uint dwAclRevision);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        protected static extern bool AddAccessAllowedAce([In, Out] IntPtr pAcl, [In] uint dwAceRevision, [In] MutantAccess AccessMask, [In] IntPtr pSid);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        protected static extern bool SetSecurityDescriptorDacl([In, Out] SECURITY_DESCRIPTOR[] pSecurityDescriptor, [In] bool bDaclPresent, [In, Optional] IntPtr pDacl, [In] bool bDaclDefaulted);

        [DllImport("kernel32.dll", EntryPoint = "CreateMutexW", CharSet = CharSet.Unicode)]
        protected static extern SafeWaitHandle CreateMutex([In, Optional] IntPtr lpMutexAttributes, [In][MarshalAs(UnmanagedType.Bool)] bool bInitialOwner, [In, Optional][MarshalAs(UnmanagedType.LPWStr)] string lpName);

        [System.Runtime.InteropServices.DllImportAttribute("advapi32.dll")]
        [return: System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)]
        protected static extern bool AllocateAndInitializeSid( [System.Runtime.InteropServices.InAttribute()]
        SID_IDENTIFIER_AUTHORITY[] pIdentifierAuthority,
          byte nSubAuthorityCount,
          uint nSubAuthority0,
          uint nSubAuthority1,
          uint nSubAuthority2,
          uint nSubAuthority3,
          uint nSubAuthority4,
          uint nSubAuthority5,
          uint nSubAuthority6,
          uint nSubAuthority7,
          out IntPtr pSid);

        [DllImport("kernel32.dll", EntryPoint = "OpenMutexW", CharSet = CharSet.Unicode)]
        protected static extern SafeWaitHandle OpenMutex(
       [In] uint dwDesiredAccess,
       [In] bool bInheritHandle,
       [In][MarshalAs(UnmanagedType.LPWStr)] string lpName
       );
    }

    internal class CAtaSmartArtificialBaseBase
    {
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        protected struct SECURITY_ATTRIBUTES
        {
            public uint nLength;
            public IntPtr lpSecurityDescriptor;
            public bool bInheritHandle;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        protected struct ACL
        {
            public byte AclRevision;
            public byte Sbz1;
            public uint AclSize;
            public uint AceCount;
            public uint Sbz2;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        protected struct SECURITY_DESCRIPTOR
        {
            public byte Revision;
            public byte Sbz1;
            public ushort Control;
            public IntPtr Owner;
            public IntPtr Group;
            public IntPtr Sacl;
            public IntPtr Dacl;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        protected struct SID_IDENTIFIER_AUTHORITY
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)] // Placeholder for actual size
            public byte[] Value;
        }

        
    }
}
