
namespace HelperClass
{
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using System;
    using static ImportantElements_Enums;
    using static ImportantElements_Properties;
    using HelperClass;

    sealed class CAtaSmartArtificial : CAtaSmartArtificialBase
    {
        private static IntPtr CreateSD(ref IntPtr pDacl, string name)
        {
            IntPtr pSid = default;
            IntPtr pAcl = default;
            IntPtr psdb = default;
            IntPtr sabAlloc = default;

            SECURITY_ATTRIBUTES[] sab = new SECURITY_ATTRIBUTES[1];
            SECURITY_DESCRIPTOR[] sdb = new SECURITY_DESCRIPTOR[1];
            ACL[] acl = new ACL[32];

            SID_IDENTIFIER_AUTHORITY[] swa = new SID_IDENTIFIER_AUTHORITY[1];

            swa[0].Value = [0x0, 0x0, 0x0, 0x0, 0x0, 0x1];
            InitializeSecurityDescriptor(sdb, SECURITY_DESCRIPTOR_REVISION);  // setup Security Descriptor

            //sid = default;
            try
            {
                var _AllocateAndInitializeSid = AllocateAndInitializeSid(swa,       // SID Identifier Authority
                1,                                  // Sub Authority count
                (uint)SECURITY_WORLD_RID,                 // Sub Authority 0
                0,                                  // Sub Authority 1
                0,                                  // Sub Authority 2
                0,                                  // Sub Authority 3
                0,                                  // Sub Authority 4
                0,                                  // Sub Authority 5
                0,                                  // Sub Authority 6
                0,                                  // Sub Authority 7
                out pSid);

                var aclsize = Marshal.SizeOf(typeof(ACL)) * acl.Length;
                pAcl = Marshal.AllocHGlobal(aclsize);

                var _initializeAcl = InitializeAcl(pAcl, (uint)aclsize, ACL_REVISION);

                if (!_initializeAcl && _AllocateAndInitializeSid)
                    goto gettingError;

                var _AddAccessDeniedAce = AddAccessAllowedAce(pAcl, ACL_REVISION, MutantAccess.MUTANT_ALL_ACCESS, pSid);

                if (_AddAccessDeniedAce)                          //
                {
                    //Marshal.PtrToStructure(pAcl, acl);
                    if (!SetSecurityDescriptorDacl(sdb, true, pAcl, false))  // yes, setup world access
                    {
                        Debugger.Break();
                        goto gettingError;
                    }
                }
                else
                {
                    //var nullPacl = IntPtr.Zero;

                    SetSecurityDescriptorDacl(sdb, true, IntPtr.Zero, false); // no, setup with default

                }


                var sdbSize = Marshal.SizeOf(typeof(SECURITY_DESCRIPTOR)) * sdb.Length;
                psdb = Marshal.AllocHGlobal(sdbSize);
                Marshal.StructureToPtr(sdb[0], psdb, true);//error

                sab[0].nLength = (uint)(Marshal.SizeOf(typeof(SECURITY_DESCRIPTOR)) * sdb.Length);                            // setup Security Attributes Block 
                sab[0].bInheritHandle = false;                           //
                sab[0].lpSecurityDescriptor = psdb;                       //

                string sprint_f = string.Format("Global\\{0}", name);

                sabAlloc = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(SECURITY_ATTRIBUTES)));
                Marshal.StructureToPtr(sab[0], sabAlloc, false);//error

                var _CreateMutexsprint_f = CreateMutex(sabAlloc, false, sprint_f);
                var _OpenMutex = OpenMutex((uint)(READ_CONTROL | MUTANT_QUERY_STATE | SYNCHRONIZE), false, sprint_f);
                var _CreateMutexName = CreateMutex(sabAlloc, false, name);

                if (_CreateMutexsprint_f.IsInvalid || _OpenMutex.IsInvalid || _CreateMutexName.IsInvalid)
                {
                    Debugger.Break();
                    goto gettingError;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                if (pSid != IntPtr.Zero)
                    pSid.FreePtr();
                if (pAcl != IntPtr.Zero)
                    pAcl.FreePtr();
                if (psdb != IntPtr.Zero)
                    psdb.FreePtr();
                //if (sab[0].lpSecurityDescriptor != IntPtr.Zero)
                //    Marshal.FreeHGlobal(sab[0].lpSecurityDescriptor);
                if (sabAlloc != IntPtr.Zero)
                    sabAlloc.FreePtr();
            }

            return IntPtr.Zero;
        gettingError:
            {
                int error = Marshal.GetLastWin32Error();
                throw new System.ComponentModel.Win32Exception(error, "Failed to initialize ACL.");
            }
        }



        public static IntPtr CreateWorldMutex(string name)
        {
            IntPtr pDacl = IntPtr.Zero;
            var pSecurityDescriptor = CreateSD(ref pDacl, name);
            return pSecurityDescriptor;
        }
    }
}
