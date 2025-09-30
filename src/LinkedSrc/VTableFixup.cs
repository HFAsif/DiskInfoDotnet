using System;
using System.Text;
using Microsoft.Build.Utilities;
using System.IO;
using System.Diagnostics;
using System.Linq;
public static class VisualStudioProvider
{
    public static DirectoryInfo TryGetSolutionDirectoryInfo(string currentPath = null)
    {
        var directory = new DirectoryInfo(
            currentPath ?? Directory.GetCurrentDirectory());
        while (directory != null && !directory.GetFiles("*.sln").Any())
        {
            directory = directory.Parent;
        }
        return directory;
    }
}
public class VTableFixupTask : Task, IDisposable
{
    private bool disposedValue;
    public string SolutionDirectory = VisualStudioProvider.TryGetSolutionDirectoryInfo().FullName ?? "SolutionDir Failed to found";
    public string Assembly { get; set; }
    public string Config { get; set; }
    public override bool Execute()
    {
        string str2 = Path.Combine(Path.GetTempPath(), "jitdVF");
        if (!Directory.Exists(str2))
        {
            Directory.CreateDirectory(str2);
        }
        str2 = Path.Combine(str2, Path.ChangeExtension(Path.GetFileName(Assembly), "il"));

        if (File.Exists(Path.ChangeExtension(str2, "pdb")))
        {
            File.Delete(Path.ChangeExtension(str2, "pdb"));
        }
        File.Copy(Path.ChangeExtension(Assembly, "pdb"), Path.ChangeExtension(str2, "pdb"));

        ProcessStartInfo startInfo = _processStartInfo(str2);
        startInfo.UseShellExecute = true;
        Process.Start(startInfo).WaitForExit();

        string contents;

        using (FileStream fileStream = new FileStream(str2, FileMode.Open, FileAccess.Read))
        {

            using (StreamReader reader = new StreamReader(fileStream))
            {
                contents = reader.ReadToEnd();
                int index = contents.IndexOf("Injection(bool list) cil managed");
                index = contents.IndexOf(".maxstack", index);
                contents = contents.Insert(index, "\r\n.vtentry 1 : 1\r\n.export [1] as Injection\r\n");

                if (File.Exists(str2))
                {
                    DisposingAll(fileStream, reader);
                    File.Delete(str2);
                }
            }
        }

        FileStream fileStreamWriteText = new FileStream(str2, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
        byte[] ContentBytes = Encoding.UTF8.GetBytes(contents);
        var _mem = new MemoryStream(ContentBytes);

        int bytesRead;

        while ((bytesRead = _mem.Read(ContentBytes, 0, ContentBytes.Length)) > 0)
        {
            fileStreamWriteText.Write(ContentBytes, 0, bytesRead);
        }

        //mem.CopyTo(fileStreamWriteText);
        DisposingAll(fileStreamWriteText, null, _mem);



        //string contents = File.ReadAllText(str2);
        //int index = contents.IndexOf("Injection(bool list) cil managed");
        //index = contents.IndexOf(".maxstack", index);
        //contents = contents.Insert(index, "\r\n.vtentry 1 : 1\r\n.export [1] as Injection\r\n");
        //File.WriteAllText(str2, contents);

        var OutPutTextTarget = Path.Combine(SolutionDirectory + "\\ConfigsFiles", "PowerShellOutPutArgs.txt");
        if (Config.StartsWith("net_3_5"))
            startInfo = new ProcessStartInfo(@"C:\WINDOWS\Microsoft.NET\Framework\v2.0.50727\ilasm.exe", "\"" + str2 + "\" /dll /debug /output=\"" + Assembly + "\"");
        else
            startInfo = new ProcessStartInfo(@"C:\WINDOWS\Microsoft.NET\Framework\v4.0.30319\ilasm.exe", "\"" + str2 + "\" /dll /debug /output=\"" + Assembly + "\"");
        Process.Start(startInfo).WaitForExit();
        startInfo.UseShellExecute = true;
        //var CreateAText = Path.Combine(Environment.CurrentDirectory, "AText.txt");
        //File.WriteAllText(CreateAText, Config.ToString());

        //Debug.WriteLine("Done");

        return true;
    }

    public void DisposingAll(FileStream fileStream = null, StreamReader streamReader = null, MemoryStream memoryStream = null)
    {
        System.GC.Collect();
        System.GC.WaitForPendingFinalizers();

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

    }

    public ProcessStartInfo _processStartInfo(string str2)
    {
        string TargetSDK = string.Empty;
        TargetSDK = @"C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.8 Tools";
        //if (Environment.Version.Major == 2)
        //{
        //    TargetSDK = @"C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\bin";
        //}
        //else if (Environment.Version.Major == 4)
        //{
        //    TargetSDK = @"C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.8 Tools";
        //}
        //else
        //{
        //    exception();
        //}

        ProcessStartInfo startInfo = default;

        if (IntPtr.Size == 4)
        {
            startInfo = new ProcessStartInfo($@"{TargetSDK}\ildasm.exe", "\"" + Assembly + "\" /out=\"" + str2 + "\" /linenum /nobar");
        }
        else if (IntPtr.Size == 8)
        {
            startInfo = new ProcessStartInfo($@"{TargetSDK}\x64\ildasm.exe", "\"" + Assembly + "\" /out=\"" + str2 + "\" /linenum /nobar");
        }
        else
        {
            exception();
        }
        return startInfo;
    }

    public Exception exception()
    {
        Debug.WriteLine("failed arc");
        return new ArgumentException("failed");
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                // TODO: dispose managed state (managed objects)
            }

            // TODO: free unmanaged resources (unmanaged objects) and override finalizer
            // TODO: set large fields to null
            disposedValue = true;
        }
    }

    // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
    // ~VTableFixupTask()
    // {
    //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
    //     Dispose(disposing: false);
    // }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}