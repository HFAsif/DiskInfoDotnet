

namespace DiskInfoDotnet.Library
{
    using System;
    using System.Runtime.InteropServices;

    class DiskInfoDotnetManager_NativeCaller_Internal
    {
        [DllImport("kernel32.dll", EntryPoint = "DeviceIoControl", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool DeviceIoControlSpecific(
        [In] IntPtr hDevice,
        [In] uint dwIoControlCode,
        /*[In, Optional]*/ ref Dis.SENDCMDINPARAMS lpInBuffer,
        [In] uint nInBufferSize,
        /*[Out, Optional]*/ ref Dis.SMART_READ_DATA_OUTDATA lpOutBuffer,
        [In] uint nOutBufferSize,
        /*[Out, Optional]*/ ref uint lpBytesReturned,
        [In, Out, Optional] IntPtr lpOverlapped
        );

        [DllImport("kernel32.dll", EntryPoint = "DeviceIoControl", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool DeviceIoControlSpecific(
            [In] IntPtr hDevice,
            [In] uint dwIoControlCode,
            /*[In, Optional]*/ ref Dis.CMD_IDE_PATH_THROUGH lpInBuffer,
            [In] uint nInBufferSize,
            /*[Out, Optional]*/ ref Dis.CMD_IDE_PATH_THROUGH lpOutBuffer,
            [In] uint nOutBufferSize,
            /*[Out, Optional]*/ ref uint lpBytesReturned,
            [In, Out, Optional] IntPtr lpOverlapped
            );


        [DllImport("kernel32.dll", EntryPoint = "DeviceIoControl", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool DeviceIoControlSpecific(
            [In] IntPtr hDevice,
            [In] uint dwIoControlCode,
            /*[In, Optional]*/ ref Dis.SENDCMDINPARAMS lpInBuffer,
            [In] uint nInBufferSize,
            /*[Out, Optional]*/ ref Dis.IDENTIFY_DEVICE_OUTDATA lpOutBuffer,
            [In] uint nOutBufferSize,
            /*[Out, Optional]*/ ref uint lpBytesReturned,
            [In, Out, Optional] IntPtr lpOverlapped
            );

        [DllImport("kernel32.dll", EntryPoint = "DeviceIoControl", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool DeviceIoControlSpecific(
            [In] IntPtr hDevice,
            [In] uint dwIoControlCode,
            /*[In, Optional]*/ ref Dis.TStorageQueryWithBuffer lpInBuffer,
            [In] uint nInBufferSize,
            /*[Out, Optional]*/ ref Dis.TStorageQueryWithBuffer lpOutBuffer,
            [In] uint nOutBufferSize,
            /*[Out, Optional]*/ ref uint lpBytesReturned,
            [In, Out, Optional] IntPtr lpOverlapped
            );

        [DllImport("kernel32.dll", EntryPoint = "DeviceIoControl", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool DeviceIoControlSpecific(
            [In] IntPtr hDevice,
            [In] uint dwIoControlCode,
            /*[In, Optional]*/ ref Dis.ATA_PASS_THROUGH_EX_WITH_BUFFERS lpInBuffer,
            [In] uint nInBufferSize,
            /*[Out, Optional]*/ ref Dis.ATA_PASS_THROUGH_EX_WITH_BUFFERS lpOutBuffer,
            [In] uint nOutBufferSize,
            /*[Out, Optional]*/ ref uint lpBytesReturned,
            [In, Out, Optional] IntPtr lpOverlapped
            );

        [DllImport("kernel32.dll", EntryPoint = "VirtualFree", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool safeVirtualFree(ref Dis.CMD_IDE_PATH_THROUGH lpAddress, UIntPtr dwSize, uint dwFreeType);


        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern Dis.CMD_IDE_PATH_THROUGH VirtualAlloc(IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);

    }
}
