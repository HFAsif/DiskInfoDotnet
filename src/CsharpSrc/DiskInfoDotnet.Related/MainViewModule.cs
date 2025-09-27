namespace DiskInfoDotnet.Related;

using HelperClass;
using Microsoft.Build.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;

[SomeElementsInfos($"class name {nameof(MainViewModule)} namespace {nameof(DiskInfoDotnet.Related)} learned from yck1509 class")]
public class MainViewModule : Task
{

#nullable disable
    public string SolutionDirectory { get; set; }
    public string AllOutPaths { get; set; }
#nullable enable

    public override bool Execute()
    {
        //if (Attribute.IsDefined(typeof(MainViewModule), typeof(SomeElementsInfos)) && typeof(MainViewModule).GetCustomAttribute<SomeElementsInfos>() is not null and SomeElementsInfos someElementsAttr)
        //{
        //    File.WriteAllText(Path.Combine(Environment.CurrentDirectory, "atext.txt"), $"{someElementsAttr.Details}");
        //}
        //else
        //{


        //}
        
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
        foreach ( var dir in netFourDirs)
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
                
                if (!asmName.StartsWith("DiskInfoDotnet.Demo.NetAll"))
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

    //private static void Main( string[] args)
    //{
    //    new MainViewModule().Execute();
    //}
}
