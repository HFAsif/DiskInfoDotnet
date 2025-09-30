namespace DiskInfoDotnet.Cleanup;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class InternalExtensions
{
    public static string SolutionDirectory = VisualStudioProvider.TryGetSolutionDirectoryInfo().FullName ?? "SolutionDir Failed to found";

    public static void DisposingAll(FileStream fileStream = null, MemoryStream memoryStream = null, BinaryReader binaryReader = null, StreamReader streamReader = null)
    {
        GC.Collect();
        GC.WaitForPendingFinalizers();
        if (fileStream != null)
        {
            fileStream.Close();
            fileStream.Dispose();
            fileStream = null;
        }
        if (streamReader != null)
        {
            streamReader.Close();
            streamReader.Dispose();
            streamReader = null;
        }
        if (memoryStream != null)
        {
            memoryStream.Close();
            memoryStream.Dispose();
            memoryStream = null;
        }
        if (binaryReader != null)
        {
            binaryReader.Close();
            binaryReader = null;
        }
    }
}
