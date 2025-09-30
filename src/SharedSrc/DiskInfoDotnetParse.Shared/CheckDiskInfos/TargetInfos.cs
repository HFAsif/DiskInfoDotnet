namespace CrystalDiskInfoDotnet.CheckDiskInfos;
using System.IO;
public class TargetInfos
{
    public string AgileOld = "C:\\Users\\Admin\\source\\repos\\AgileNetOld\\TestWin\\bin\\Debug\\Secured\\old\\TestWin.exe";

    public string AgileNew = "C:\\Users\\Admin\\Desktop\\hermasSrc\\src\\bin\\Debug\\net46\\Hermes Auto Plist Reader.exe";
    public bool AgileOldTest = false;
    public bool UnknownDeob = true;

    public string TatgetDLL = "C:\\Users\\Admin\\Desktop\\Desktop\\TestDeobs\\Old\\AgileDotNet.VMRuntime.dll";
    public string vmpfile = "C:\\Users\\Admin\\Desktop\\iRemoval PRO Premiun Edition\\iRemoval PRO.exe";
    public bool SingleDecrypt = false;
    public bool vmp = false;
    public bool saveBin = false;
    public bool testing = false;

    public static string binpath = Path.Combine(HelperClass.HelperViews.TryGetSolutionDirectoryInfo().FullName, "CSVMNew.bin");

    public static string comCSVM = Path.Combine(HelperClass.HelperViews.TryGetSolutionDirectoryInfo().FullName, "COMCSVM.bin");
    //C:\Users\Admin\Desktop\FinalProject\src\IDeviceSource\MacProject\curl-ca-bundle.crt

    //C:\Users\Admin\source\repos\AgileNetOld\AgileVMDecrypter\MyLib\ExtractedHasher.json


    private static string SLNDir = Path.Combine(HelperClass.HelperViews.TryGetSolutionDirectoryInfo().FullName);

    public static string certfile = Path.Combine(SLNDir, "src\\IDeviceSource\\MacProject", "curl-ca-bundle.crt");

    public static string ploads = Path.Combine(SLNDir, "MacProject\\MyLibrary", "pload.json");
    public static string postloads = Path.Combine(SLNDir, "MacProject\\MyLibrary", "postload.json");
    public static string savestate = Path.Combine(SLNDir, "MacProject\\MyLibrary", "SaveState.json");

    public static string checkingJsFile = Path.Combine(SLNDir, "MacProject//MyLibrary", "CheckingJs.json");
    public static string fixingjsfile = Path.Combine(SLNDir, "MacProject\\MyLibrary", "FixingJs.json");
    public static string fixingjsfile2 = Path.Combine(SLNDir, "MacProject\\MyLibrary", "FixingJs2.json");

    //C:\Users\Admin\Desktop\FinalProject\src\IDeviceSource\IDevicePartial\IDevicePartial.csproj
    public static string NotWorkingPayload = Path.Combine(SLNDir, "MacProject\\MyLibrary\\Test", "NotWorkingPayload.json");
    public static string TestPlist = Path.Combine(SLNDir, "src\\IDeviceSource\\IDevicePartial\\Library\\Utility", "TestPlist.plist");


    public static string NotWorkingPlistJson = Path.Combine(SLNDir, "MacProject//MyLibrary//Test", "NotWorkingPlist.plist");
    public static string WorkingPlistJson = Path.Combine(SLNDir, "MacProject//MyLibrary//Test", "WorkingPlist.plist");


    public static string OriginalJson = Path.Combine(SLNDir, "Checkm8.info-Software//MyLibrary", "OriginalJson.json");
    public static string CompositeExtracted = Path.Combine(SLNDir, "Checkm8.info-Software//MyLibrary", "ExtractedHasher.json");

    public static string CompositeExtracted2 = Path.Combine(SLNDir, "Checkm8.info-Software//MyLibrary", "ExtractedHasher2.json");

    public static string texfile1 = Path.Combine(SLNDir, "DownloadVision", "TextFile1.txt");
}
