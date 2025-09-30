namespace DiskInfoDotnet.Demo;

using Microsoft.Extensions.Logging;
using System.Reflection;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using CrystalDiskInfoDotnet.CheckDiskInfos;
using CrystalDiskInfoDotnet;
using HelperClass;
using System.Collections;

internal class FinishingClass : CrystalDiskInfoDotnetBase
{
    public required ILogger logger { get; set; }

    [SomeElementsInfos("Coded internal")]
    [FinishingClassAttr]
    public override void ExtracInformation()
    {
        var list = new List<object>();
        var keyValuePairs = new List<KeyValuePair<string, string>>();

        Console.Title = GetTitle();
        var prgAttr = MethodBase.GetCurrentMethod().GetCustomAttribute<FinishingClassAttr>();
        prgAttr.PrimaryWorker(logger);

        #region MyRegion
        //ICrystalDiskInfoDotnet diskInfoArtificialManager = new CrystalDiskInfoDotnetLoad()
        //{
        //    ataLists = ataLists,
        //    DiskInfoArtificialEndedAt = 
        //};

        //diskInfoArtificialManager.ExternalRun(out var infoForCasts, out var optimizedListBuilder); 
        #endregion

        //crystalDiskInfoDotnet.LoadInformation(out var infoForCasts, out var OptimizedListBuilder);
        base.ExtracInformation(out var infoForCasts, out var optimizedListBuilder);

        //EnvDTE.DTE dte = (EnvDTE.DTE)GetActiveObject("VisualStudio.DTE");
        if (optimizedListBuilder is not null)
        {
            var build = optimizedListBuilder.ToReadOnlyCollection();
            foreach (var item in build)
            {
                //Console.WriteLine(Environment.NewLine + Environment.NewLine);
                if (item is not null)
                {
                    foreach (var item2 in item)
                    {
                        keyValuePairs.Add(item2);
                        //Console.WriteLine(Environment.NewLine + Environment.NewLine);
                        //Console.WriteLine(item2.Key + "  :  " + item2.Value);
                    }
                }

            }
        }
        else if (infoForCasts is not null)
        {

            var _infoForCasts = infoForCasts;
            if (infoForCasts is ICollection infoCasts)
            {
                foreach (var item in infoCasts)
                {
                    list.Add(item);
                    //logger.LogInformation(Environment.NewLine + Environment.NewLine);
                    //var json = JsonConvert.SerializeObject(item, Formatting.Indented);
                    //logger.LogInformation(json);
                }
            }

        }



        //Task.Delay(1000);
    }


    //public override void FinallyWorker()
    //{


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
    //            attributeWorker(type);
    //            #region MyRegion
    //            //logger.LogInformation("class name {0}, " + someElementsInfos.Details, Environment.NewLine + type.FullName);

    //            //var customattrs = type.GetCustomAttributes(typeof(SomeElementsInfos));
    //            //if (customattrs is not null)
    //            //{

    //            //    foreach (var smtpattr in customattrs)
    //            //    {
    //            //        if (smtpattr is SomeElementsInfos someElementsInfos)
    //            //        {
    //            //            logger.LogInformation("class name {0}, " + someElementsInfos.Details, Environment.NewLine + type.FullName);
    //            //        }
    //            //    }
    //            //} 
    //            #endregion
    //        }
    //        else if (smUnion is MethodInfo method)
    //        {

    //            var smmattrs = method.GetCustomAttributes(typeof(SomeElementsInfos));
    //            if (smmattrs is not null)
    //            {
    //                foreach (var smttr in smmattrs)
    //                {
    //                    if (smttr is SomeElementsInfos smE)
    //                        logger.LogInformation("method name {0}, " + smE.Details, Environment.NewLine + method.Name);
    //                }
    //            }
    //            //logger.LogInformation("method name {0}, " + smmattr.Details, Environment.NewLine + method.Name);
    //        }

    //    }
    //}

    //void attributeWorker(Type smUnion)
    //{
    //    //Unsafe.SkipInit(out someElementsInfos);

    //    if (smUnion is Type type)
    //    {
    //        var customattrs = type.GetCustomAttributes(typeof(SomeElementsInfos));
    //        if (customattrs is not null)
    //        {

    //            foreach (var smtpattr in customattrs)
    //            {
    //                if (smtpattr is SomeElementsInfos smE)
    //                {
    //                    logger.LogInformation("class name {0}, " + smE.Details, Environment.NewLine + type.FullName);
    //                }
    //            }
    //        }
    //    }
    //}
}


