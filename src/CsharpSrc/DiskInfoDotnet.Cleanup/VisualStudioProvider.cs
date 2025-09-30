namespace DiskInfoDotnet.Cleanup;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class VisualStudioProvider
{
    public static DirectoryInfo TryGetSolutionDirectoryInfo(string currentPath = null)
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(currentPath ?? Directory.GetCurrentDirectory());
        while (directoryInfo != null && !directoryInfo.GetFiles("*.sln").Any())
        {
            directoryInfo = directoryInfo.Parent;
        }
        return directoryInfo;
    }

}