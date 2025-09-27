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
        return true;
    }

    
}
