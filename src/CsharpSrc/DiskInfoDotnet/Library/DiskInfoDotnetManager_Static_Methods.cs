
namespace DiskInfoDotnet.Library;

using HelperClass;
using System;
using System.Text;
using static Impp;

static class DiskInfoDotnetManager_Static_Methods_Internal
{
    public static int IOCTL_STORAGE_QUERY_PROPERTY = CTL_CODE(IOCTL_STORAGE_BASE, 0x0500, METHOD_BUFFERED, FILE_ANY_ACCESS);
    public static int IOCTL_SCSI_PASS_THROUGH = CTL_CODE(IOCTL_SCSI_BASE, 0x0401, METHOD_BUFFERED, FILE_READ_ACCESS | FILE_WRITE_ACCESS);
    public static int NVME_PASS_THROUGH_SRB_IO_CODE = CTL_CODE(NVME_STORPORT_DRIVER, 0x800, METHOD_BUFFERED, FILE_ANY_ACCESS);
    public static int IOCTL_SCSI_GET_ADDRESS = CTL_CODE(IOCTL_SCSI_BASE, 0x0406, METHOD_BUFFERED, FILE_ANY_ACCESS);

    public static int CTL_CODE(int deviceType, int function, int method, int access)
    {
        return (deviceType << 16) | (access << 14) | (function << 2) | method;
    }

    public static string Mid(this string vals, int start, int len) => vals.Substring(start, Math.Min(len, vals.Length));

    public static bool IsEmpty(this string vals) => string.IsNullOrEmpty(vals);

    public static ushort MAKEWORD(uint a, uint b)
    {
        return (ushort)((byte)(a & 0xff) | ((ushort)((byte)(b & 0xff)) << 8));
        //((WORD)(((BYTE)(((DWORD_PTR)(a)) & 0xff)) | ((WORD)((BYTE)(((DWORD_PTR)(b)) & 0xff))) << 8))
    }

    public static long MAKELONG(uint a, uint b)
    {
        return ((long)((ushort)(a & 0xffff)) | ((long)((ushort)(b & 0xffff)) << 16));
        //#define MAKELONG(a, b)      ((LONG)(((WORD)(((DWORD_PTR)(a)) & 0xffff)) | ((DWORD)((WORD)(((DWORD_PTR)(b)) & 0xffff))) << 16))
    }

    public static void TrimRight(this string value, ref string propertyeName)
    {
        var _stringBuilder = new StringBuilder(value);

        string str = _stringBuilder.ToString();
        int lastIndex = str.Length;

        while (lastIndex > 0 && char.IsWhiteSpace(str[lastIndex - 1]))
        {
            lastIndex--;
        }

        if (lastIndex < str.Length)
        {
            //this.Truncate(lastIndex);

            if (lastIndex < _stringBuilder.Length)
            {
                _stringBuilder.Length = lastIndex;
            }

        }

        propertyeName = _stringBuilder.ToString();
    }

    public static void TrimLeft(this string _value, ref string propertyeName)
    {
        // find first non-space character
        int firstNonSpaceIndex = 0;

        while (firstNonSpaceIndex < _value.Length && char.IsWhiteSpace(_value[firstNonSpaceIndex]))
        {
            firstNonSpaceIndex++;
        }

        if (firstNonSpaceIndex > 0)
        {
            // fix up data and length
            string trimmedValue = _value.Substring(firstNonSpaceIndex);
            _value = trimmedValue;
            //ReleaseBufferSetLength(trimmedValue.Length);

            _value = _value.Substring(0, trimmedValue.Length);
        }
        propertyeName = _value;
    }
}
