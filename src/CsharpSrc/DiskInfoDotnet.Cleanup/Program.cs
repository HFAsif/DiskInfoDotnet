// See https://aka.ms/new-console-template for more information
using DiskInfoDotnet.Cleanup;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

internal class Program
{
    private static void Main(string[] args)
    {

        if (Debugger.IsAttached)
        {
            Console.WriteLine("Dont run it directly , you have to build this project , when the build get success , you will get the File in solution root dir just run it ");
            Console.ReadLine();
            Environment.Exit(0);
        }

        CleanSystem1();
    }

    public static void CleanSystem1()
    {
        string arguments = "Get-ChildItem -path " + InternalExtensions.SolutionDirectory + @" .\ -include TaskUsingFolder,bin,obj,net_4_0_Debug,net_3_5_Debug,Debug -Recurse | ForEach-Object ($_) { Remove-Item $_.FullName -Force -Recurse }";
        ProcessStartInfo startInfo = new ProcessStartInfo(@"C:\Windows\system32\WindowsPowerShell\v1.0\powershell.exe", arguments);
        Process.Start(startInfo).WaitForExit();
    }

    public static void CleanSystem2()
    {
        string arguments = "Get-ChildItem -path " + InternalExtensions.SolutionDirectory + @" .\ -include TaskUsingFolder,bin,obj -Recurse | ForEach-Object ($_) { Remove-Item $_.FullName -Force -Recurse }";
        ProcessStartInfo info = new ProcessStartInfo(@"C:\\Windows\\system32\\WindowsPowerShell\\v1.0\\powershell.exe", arguments)
        {
            RedirectStandardOutput = true,
            RedirectStandardError = true
        };
        using (Process process = new Process())
        {
            process.StartInfo = info;
            process.Start();
            string str2 = process.StandardOutput.ReadToEnd();
            string str3 = process.StandardError.ReadToEnd();
            Console.WriteLine(str2);
        }
    }

    
}