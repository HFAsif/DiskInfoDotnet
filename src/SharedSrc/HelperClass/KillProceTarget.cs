

using System;
using System.Collections.Generic;
using System.Diagnostics;

public class KillProceTarget : IDisposable
{
    public static void KillProcess()
    {
        var allProcesses = Process.GetProcesses();

        var msbuildes = new List<Process>();
        foreach (Process theprocess in allProcesses)
        {

            if (theprocess.ProcessName == "MSBuild")
            {
                msbuildes.Add(theprocess);
            }
            //Console.WriteLine("Process: {0} ID: {1}", theprocess.ProcessName, theprocess.Id);
        }

        for (int i = 0; i < msbuildes.Count; i++)
        {
            Console.WriteLine(msbuildes.Count);
            //msbuildes[i].Dispose();
            msbuildes[i].Kill();

            Console.WriteLine(msbuildes[i].Id + " killed " + msbuildes[i].ProcessName);
        }
        //_memoryStream.Dispose();

    }

    void IDisposable.Dispose()
    {
        GC.SuppressFinalize(this);
    }
}