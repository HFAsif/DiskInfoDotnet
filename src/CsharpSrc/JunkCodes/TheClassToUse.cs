

using Microsoft.Build.Framework;
using Microsoft.Build.Framework.XamlTypes;
using Microsoft.Build.Utilities;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace UsingTaskTestLib
{
    public class TheClassToUse : Task
    {
#nullable disable
        public string SolutionDirectory { get; set; }
        public string TargetOutPutPath { get; set; }
#nullable enable
        public override bool Execute()
        {
            //var TextCreate = Path.Combine(Environment.CurrentDirectory, "atext.txt");
            //var directories = Directory.GetDirectories(AllOutPaths);

            //if (!File.Exists(TextCreate))
            //{
            //    File.Delete(TextCreate);
            //}

            //File.WriteAllText(TextCreate, string.Empty);
            //using StreamWriter writer = new StreamWriter(TextCreate, true);

            //writer.WriteLine($"{SolutionDirectory} {Environment.NewLine}", AllOutPaths);
            ////writer.Close();
            ///

            //"C:\Program Files (x86)\Windows Kits\10\bin\10.0.19041.0\x64\signtool.exe" sign /fd SHA256 /tr http://timestamp.digicert.com /td SHA256 /sha1 "YOUR_CERTIFICATE_THUMBPRINT" "$(TargetPath)"


            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = @"C:\Windows\SysWOW64\WindowsPowerShell\v1.0\powershell.exe",
                UseShellExecute = false,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                CreateNoWindow = true,
                Arguments = @"-noe -c ""&{Import-Module """"""C:\Program Files\Microsoft Visual Studio\2022\Enterprise\Common7\Tools\Microsoft.VisualStudio.DevShell.dll""""""; Enter-VsDevShell 46b89eab}"""
            };
            //startInfo.Arguments += " && signtool";

            Process devCmd = new Process();
            devCmd.StartInfo = startInfo;
            devCmd.Start();

            //devCmd.StartInfo.Arguments += " && signtool";

            var cPfxPath = "C:\\Users\\Admin\\Documents\\MyDrive\\MyStorecert.pfx";
            var cPfxPass = "";

            devCmd.StandardInput.WriteLine($"signtool.exe sign /f {cPfxPath} /p {cPfxPass} /fd SHA256 {dirFile.FullName}");
            devCmd.StandardInput.Flush();
            devCmd.StandardInput.Close();

            while (!devCmd.StandardOutput.EndOfStream)
            {
                string line = devCmd.StandardOutput.ReadLine();
                //Console.WriteLine(line);
                // do something with line
                Log.LogMessage(MessageImportance.High, line);
            }

            //Console.WriteLine(devCmd.StandardOutput.ReadToEnd());
            devCmd.WaitForExit();


            #region MyRegion
            //var shellPath = "C:\\Windows\\system32\\WindowsPowerShell\\v1.0\\powershell.exe";
            //var arguments = "-NoExit -Command &quot;& { Import-Module &quot;&quot;&quot;$env:VSAPPIDDIR\\..\\Tools\\Microsoft.VisualStudio.DevShell.dll&quot;&quot;&quot;; Enter-VsDevShell -SkipAutomaticLocation -SetDefaultWindowTitle -InstallPath $env:VSAPPIDDIR\\..\\..\\}&quot";
            //var arguments = @"-NoExit -Command ""&{Import-Module """"""C:\Program Files (x86)\Microsoft Visual Studio\2022\BuildTools\Common7\Tools\Microsoft.VisualStudio.DevShell.dll""""""; Enter-VsDevShell 0d0637f3 -SkipAutomaticLocation -DevCmdArguments """"""-arch=x64 -host_arch=x64""""""}"",
            //""overrideName"": true";

            //arguments += " signtool";
            //var arguments = @"/k """"%VSAPPIDDIR%\..\Tools\VsDevCmd.bat""";

            //-NoExit -Command &quot;& { Import-Module &quot;&quot;&quot;$env:VSAPPIDDIR\..\Tools\Microsoft.VisualStudio.DevShell.dll&quot;&quot;&quot;; Enter-VsDevShell -SkipAutomaticLocation -SetDefaultWindowTitle -InstallPath $env:VSAPPIDDIR\..\..\}&quot;


            //-NoExit -Command "& { Import-Module """$env:VSAPPIDDIR\..\Tools\Microsoft.VisualStudio.DevShell.dll"""; Enter-VsDevShell -SkipAutomaticLocation -SetDefaultWindowTitle -InstallPath $env:VSAPPIDDIR\..\..\}"

            //var arguments = $"-NoProfile -ExecutionPolicy Bypass -Command \"Get-Child";
            //foreach (var dir in directories)
            //{
            //    arguments += $" -Path '{dir}\\**\\*.dll' -Recurse";
            //}
            //arguments += " | Select-Object -ExpandProperty FullName | Out-File -FilePath '" + TextCreate + "' -Append\"";

            //Log.LogMessage(MessageImportance.High, $"Executing: {shellPath} {arguments}");

            //var p = new Process();
            //p.StartInfo.UseShellExecute = false;
            //p.StartInfo.RedirectStandardOutput = true;
            //p.StartInfo.Arguments = arguments;
            //p.StartInfo.FileName = shellPath;
            //p.Start();

            //ProcessStartInfo psi = new ProcessStartInfo
            //{
            //    FileName = "C:\\Windows\\system32\\WindowsPowerShell\\v1.0\\powershell.exe", // Or the full path to your Developer PowerShell script
            //    Arguments = "-NoExit -Command &quot;& { Import-Module Microsoft.PowerShell.Utility; Write-Host 'Developer PowerShell is ready!'; }&quot;", // Example: execute a command
            //    UseShellExecute = false, // Set to false to redirect standard output/error
            //    RedirectStandardOutput = true,
            //    RedirectStandardError = true,
            //    CreateNoWindow = true, // Set to true to hide the PowerShell window

            //    RedirectStandardInput = true
            //};

            //using (Process process = Process.Start(psi))
            //{
            //    // Read output and errors
            //    string output = process.StandardOutput.ReadToEnd();
            //    string error = process.StandardError.ReadToEnd();

            //    process.WaitForExit(); // Wait for the process to complete

            //    ////// Process the output or error messages
            //    ////Console.WriteLine("Output: " + output);
            //    ////Console.WriteLine("Error: " + error);

            //    //writer.WriteLine("Output: " + output);
            //    //writer.WriteLine("Error: " + error);
            //    //writer.Close();
            //}

            //p.StartInfo = psi;
            //p.Start();
            //// To avoid deadlocks, always read the output stream first and then wait.  
            //string output = p.StandardOutput.ReadToEnd();
            //p.WaitForExit();
            ////p.Dispose();

            //writer.WriteLine(output);
            //writer.Close();

            //var p = new Process();
            //p.StartInfo.Arguments = arguments;
            //p.StartInfo.FileName = shellPath;
            //p.StartInfo.RedirectStandardOutput = true;
            //p.Start();

            //// To avoid deadlocks, always read the output stream first and then wait.  
            //string output = p.StandardOutput.ReadToEnd();
            //p.WaitForExit();
            //p.StartInfo.UseShellExecute = true;

            //var startInfo = new ProcessStartInfo(shellPath, arguments);

            //var process = System.Diagnostics.Process.Start(shellPath, arguments);
            //Process.Start(startInfo).WaitForExit();
            //startInfo.UseShellExecute = true;


            //var proc = new Process
            //{
            //    StartInfo = new ProcessStartInfo
            //    {
            //        FileName = shellPath,
            //        Arguments = arguments
            //    }

            //};
            //proc.Start();

            ////// Synchronously read the standard output of the spawned process.
            ////StreamReader reader = proc.StandardOutput;
            ////string output = reader.ReadToEnd();

            //writer.WriteLine(proc.StandardOutput.ReadLine());
            //writer.Close();

            //proc.WaitForExit();
            //proc.StartInfo.UseShellExecute = true;

            //while (!proc.StandardOutput.EndOfStream)
            //{
            //    string line = proc.StandardOutput.ReadLine();
            //    writer.WriteLine(line);
            //    // do something with line
            //}
            //* Read the output (or the error)

            //string output = proc.StandardOutput.ReadToEnd(); 
            #endregion



            var TextCreate = Path.Combine(Environment.CurrentDirectory, "atext.txt");
            var directories = Directory.GetDirectories(AllOutPaths);

            if (!File.Exists(TextCreate))
            {
                File.Delete(TextCreate);
            }

            File.WriteAllText(TextCreate, string.Empty);
            //file.Create();
            //file.cl

            //File.WriteAllText(TextCreate, String.Empty);

            using StreamWriter writer = new StreamWriter(TextCreate, true);

            var netFourDirs = directories.ToList();
            foreach (var dir in netFourDirs)
            {
                var dirInfo = new DirectoryInfo(dir);
                if (!dirInfo.Name.StartsWith("net4"))
                    continue;

                var CreatingDepDir = Path.Combine(dir, "Mydependencies");
                if (!Directory.Exists(CreatingDepDir))
                {
                    Directory.CreateDirectory(CreatingDepDir);
                    writer.WriteLine("Success full to create folder {0}", CreatingDepDir);
                }

                var netFrFiles = dirInfo.GetFiles();

                foreach (var dirFile in netFrFiles)
                {
                    var asmName = dirFile.Name;

                    if (!asmName.StartsWith("DiskInfoDotnet.Demo"))
                    {
                        var MovingFile = Path.Combine(CreatingDepDir, dirFile.Name);
                        //writer.WriteLine(MovingFile);
                        dirFile.MoveTo(MovingFile);

                        //File.Move(dirFile, MovingFile);
                        writer.WriteLine("Successfull to move to {0}", dirFile.FullName);

                    }
                }

            }

            writer.Close();


            #region MyRegion
            //var GetAllNetfrDirs = directories.Any(a => a.StartsWith("net4"));
            //foreach ( var dir in GetAllNetfrDirs)
            //{
            //    writer.WriteLine(dir);
            //}

            //writer.WriteLine("Successfull to move to {0}", AllOutPaths);
            ////using (StreamWriter writer = new StreamWriter(TextCreate, true)) // false for overwrite
            ////{
            ////    writer.WriteLine(directories.Length);
            ////    writer.Close();
            ////}

            ////GC.Collect();
            ////GC.WaitForPendingFinalizers();
            ////GC.SuppressFinalize(this);

            //for ( int i = 0; i < directories.Length; i++ )
            //{
            //    var FoldLocation = directories[i];
            //    //var DirName = Path.Combine(directories[i], "Mydependencies");
            //    //Directory.CreateDirectory(DirName);
            //    //writer.WriteLine(DirName);

            //    var DirFiles = Directory.GetFiles(FoldLocation);
            //    var createDep = Path.Combine(FoldLocation, "Mydependencies");
            //    writer.WriteLine(Environment.NewLine);

            //    if (!Directory.Exists(createDep))
            //    {
            //        Directory.CreateDirectory(createDep);
            //    }

            //    foreach (var dirFile in DirFiles)
            //    {

            //        var asmName = Path.GetFileNameWithoutExtension(dirFile);
            //        if (!asmName.Contains("DiskInfoDotnet.Demo.NetAll"))
            //        {
            //            //var MovingFile = Path.Combine(createDep, Path.GetFileName(dirFile));
            //            //File.Move(dirFile, MovingFile);
            //            //writer.WriteLine("Successfull to move to {0}", MovingFile);
            //        }

            //    }

            //}





            //File.AppendAllText(TextCreate, directories.Count().ToString());

            //foreach (var dir in directories)
            //{
            //    var DirName = Path.Combine(dir, "Mydependencies");
            //    //var createDep = Directory.CreateDirectory(DirName);
            //    File.AppendAllText(TextCreate, Environment.NewLine + DirName);
            //}

            //File.WriteAllText(Path.Combine(Environment.CurrentDirectory, "atext.txt"), string.Join(Environment.NewLine, directories));

            //Debug.WriteLine($"asembly info {Assembly} config Infos {Config}"); 
            #endregion


            return true;
        }
    }
}
