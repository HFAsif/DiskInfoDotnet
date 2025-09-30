namespace CrystalDiskInfoDotnet.CheckDiskInfos;

using System.Collections.Generic;
using System.Reflection;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using HelperClass;

#if LoggerExist
using Microsoft.Extensions.Logging;
#endif

public abstract class CrystalDiskInfoDotnetBase
{
    public required object ataInfos { get; set; }
    public required OutPutInfos outPutInfos { get; set; }

    //public required string DiskInfoArtificialEndedAt { get; set; }

    [SomeElementsInfos("The code copied from wwh00")]
    private static T GetAssemblyAttribute<T>()
    {
        return (T)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(T), false)[0];
    }

    [SomeElementsInfos("The code copied from wwh00")]
    protected virtual string GetTitle()
    {
        string productName;
        string version;
        string copyright;
        int firstBlankIndex;
        string copyrightOwnerName;
        string copyrightYear;

        productName = GetAssemblyAttribute<AssemblyProductAttribute>().Product;
        version = Assembly.GetExecutingAssembly()?.GetName()?.Version?.ToString() ?? throw new GettingExceptions($"get exception at class name {nameof(CrystalDiskInfoDotnetBase)}");
        copyright = GetAssemblyAttribute<AssemblyCopyrightAttribute>().Copyright.Substring(12);
        firstBlankIndex = copyright.IndexOf(' ');
        copyrightOwnerName = copyright.Substring(firstBlankIndex + 1);
        copyrightYear = copyright.Substring(0, firstBlankIndex);
        return $"{productName} v{version} by {copyrightOwnerName} {copyrightYear}";
    }

    public abstract void ExtractDiskInfo(DiskInfoDotnet.Sm.Management.LoadMScopModule loadMScopModule, out string diskInfoExtractedTime,
#if LoggerExist
        ILoggerFactory loggerFactory,
#endif
        string[] args);

    public virtual void ExtracInformation()
    {
        ExtracInformation(out var infoForCasts, out var extracInformation);
    }

    public virtual void ExtracInformation(
        out ObservableCollection<ExtendedInfosStruct> infoForCasts, 
        out ReadOnlyCollectionBuilder<ObservableCollection<KeyValuePair<string, string>>> OptimizedListBuilder)
    {
        CrystalDiskInfoDotnetLoadInformation crystalDiskInfoDotnet = new CrystalDiskInfoDotnetLoadInformation()
        { ataInfos = ataInfos, InfoType = outPutInfos };
        crystalDiskInfoDotnet.LoadInformation(out infoForCasts, out OptimizedListBuilder);
    }

    //public virtual void MakeInformationForUse(out ObservableCollection<ExtendedInfosStruct> infoForCasts, out ReadOnlyCollectionBuilder<ObservableCollection<KeyValuePair<string, string>>> OptimizedListBuilder)
    //{
    //    Unsafe.SkipInit(out infoForCasts);
    //    Unsafe.SkipInit(out OptimizedListBuilder);

    //    if (infoForCasts is not null)
    //    {

    //    }
    //    else if (OptimizedListBuilder is not null)
    //    {

    //    }
    //    else throw new GettingExceptions(nameof(CrystalDiskInfoDotnetBase), "no informaion exceptions");

    //}

    //public virtual void MakeInformationForUse(out ObservableCollection<ExtendedInfosStruct> infoForCasts, out ReadOnlyCollectionBuilder<ObservableCollection<KeyValuePair<string, string>>> OptimizedListBuilder)
    //{
    //    if (optimizedListBuilder is not null)
    //    {

    //        foreach (var item in optimizedListBuilder)
    //        {
    //            Console.WriteLine(Environment.NewLine + Environment.NewLine);
    //            foreach (var item2 in item)
    //            {
    //                Console.WriteLine(item2.Key + "  :  " + item2.Value);
    //            }
    //        }

    //        Task.Delay(1000);
    //    }
    //    else if (infoForCasts is not null)
    //    {
    //        Console.WriteLine(Environment.NewLine + Environment.NewLine);
    //        var _infoForCasts = infoForCasts;
    //        foreach (var item in _infoForCasts)
    //        {
    //            var json = JsonConvert.SerializeObject(item, Formatting.Indented);
    //            logger.LogInformation(json);
    //        }
    //        Task.Delay(1000);
    //    }

    //    var currentAsm = AppDomain.CurrentDomain.GetAssemblies().GetEnumerator();
    //    //var memList = currentAsm.GetType().GetMembers();

    //    #region MyRegion
    //    //var asm = (Assembly)currentAsm.Current;

    //    ////var types = from type in asm.GetTypes()
    //    ////            where Attribute.IsDefined(type, typeof(SomeElementsInfos))
    //    ////            select type;

    //    ////var tpenum = types.GetEnumerator();

    //    ////if (tpenum.MoveNext())
    //    ////{
    //    ////    do
    //    ////    {
    //    ////        var tpcr = tpenum.Current;
    //    ////        var tarGetattr = tpcr.GetCustomAttribute<SomeElementsInfos>();
    //    ////        smList.Add(tpcr);

    //    ////        foreach (var method in tpcr.GetRuntimeMethods())
    //    ////        {
    //    ////            var smm = method.GetCustomAttribute<SomeElementsInfos>();
    //    ////            if (smm != null)
    //    ////            {
    //    ////                smList.Add(method);
    //    ////            }
    //    ////        }
    //    ////    }
    //    ////    while (tpenum.MoveNext());
    //    ////}


    //    //foreach (var type in smTypeList)
    //    //{

    //    //    if (Attribute.IsDefined(type, typeof(SomeElementsInfos)))
    //    //    {
    //    //        smTpList.Add(type);
    //    //    }
    //    //    foreach (var method in type.GetRuntimeMethods())
    //    //    {
    //    //        var smm = method.GetCustomAttribute<SomeElementsInfos>();
    //    //        if (smm != null)
    //    //        {
    //    //            smMtList.Add(method);
    //    //        }
    //    //    }
    //    //} 
    //    #endregion


    //    List<object> smTpList = new List<object>();
    //    List<MethodInfo> smMtList = new List<MethodInfo>();

    //    if (currentAsm.MoveNext())
    //    {
    //        do
    //        {
    //            var asm = (Assembly)currentAsm.Current;
    //            foreach (var type in asm.GetTypes())
    //            {
    //                if (Attribute.IsDefined(type, typeof(SomeElementsInfos)))
    //                {
    //                    smTpList.Add(type);
    //                }
    //                foreach (var method in type.GetRuntimeMethods())
    //                {
    //                    var smm = method.GetCustomAttribute<SomeElementsInfos>();
    //                    if (smm != null)
    //                    {
    //                        smMtList.Add(method);
    //                    }
    //                }
    //            }

    //            #region MyRegion

    //            //var types =  (from type in asm.GetTypes()
    //            //            where Attribute.IsDefined(type, typeof(SomeElementsInfos))
    //            //            select type).ToList().Cast<object>();

    //            //if (types.Count() > 0)
    //            //{
    //            //    smTpList.Add(types);
    //            //}


    //            //var methods = from type in asm.GetTypes() from method in type.GetRuntimeMethods() where method.GetCustomAttribute<SomeElementsInfos>() is not null select method;

    //            //if(methods.Count() > 0)
    //            //{
    //            //    smMtList = methods.ToList();
    //            //}
    //            #endregion
    //        }
    //        while (currentAsm.MoveNext());
    //    }

    //    foreach (var smUnion in smTpList.Union(smMtList))
    //    {
    //        //var smDynamic = ((dynamic)smUnion).GetCustomAttribute<SomeElementsInfos>();
    //        //Console.WriteLine("class name {0}, " + smDynamic.Details, Environment.NewLine + ((dynamic)smUnion).Name);

    //        if (smUnion is Type type)
    //        {
    //            var smtpattr = type.GetCustomAttribute<SomeElementsInfos>();
    //            logger.LogInformation("class name {0}, " + smtpattr.Details, Environment.NewLine + type.FullName);
    //        }
    //        else if (smUnion is MethodInfo method)
    //        {
    //            var smmattr = method.GetCustomAttribute<SomeElementsInfos>();
    //            logger.LogInformation("method name {0}, " + smmattr.Details, Environment.NewLine + method.Name);
    //        }

    //    }
    //}

}
