namespace DiskInfoDotnet.Demo.NetAll;

using CrystalDiskInfoDotnet.CheckDiskInfos;
using HelperClass;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

[SomeElementsInfos($"Last class name {nameof(FinishingClass)}")]
internal class FinishingClass : CrystalDiskInfoDotnetBase
{
    [SomeElementsInfos("Coded internal")]
    [FinishingClassAttr]
    public override void ExtracInformation()
    {
        Console.Title = GetTitle();
        var prgAttr = MethodBase.GetCurrentMethod().GetCustomAttribute<FinishingClassAttr>();
        prgAttr.PrimaryWorker(out var logs);

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
                Console.WriteLine(Environment.NewLine + Environment.NewLine);
                if (item is not null)
                {
                    foreach (var item2 in item)
                    {
                        //Console.WriteLine(Environment.NewLine + Environment.NewLine);
                        Console.WriteLine(item2.Key + "  :  " + item2.Value);
                    }
                }

            }

            Task.Delay(1000);
        }
        else if (infoForCasts is not null)
        {

            var _infoForCasts = infoForCasts;
            foreach (var item in _infoForCasts)
            {
                Console.WriteLine(Environment.NewLine + Environment.NewLine);
                var json = JsonConvert.SerializeObject(item, Formatting.Indented);
                Console.WriteLine(json);
            }
            Task.Delay(1000);
        }

        var currentAsm = AppDomain.CurrentDomain.GetAssemblies().GetEnumerator();


        List<object> smTpList = new List<object>();
        List<MethodInfo> smMtList = new List<MethodInfo>();

        if (currentAsm.MoveNext())
        {
            do
            {
                var asm = (Assembly)currentAsm.Current;
                foreach (var type in asm.GetTypes())
                {
                    if (Attribute.IsDefined(type, typeof(SomeElementsInfos)))
                    {
                        smTpList.Add(type);
                    }
                    foreach (var method in type.GetRuntimeMethods())
                    {
                        var smm = method.GetCustomAttribute<SomeElementsInfos>();
                        if (smm != null)
                        {
                            smMtList.Add(method);
                        }
                    }
                }
            }
            while (currentAsm.MoveNext());
        }

        foreach (var smUnion in smTpList.Union(smMtList))
        {

            if (smUnion is Type type)
            {
                attributeWorker(type);
            }
            else if (smUnion is MethodInfo method)
            {

                var smmattrs = method.GetCustomAttributes(typeof(SomeElementsInfos));
                if (smmattrs is not null)
                {
                    foreach (var smttr in smmattrs)
                    {
                        if (smttr is SomeElementsInfos smE)
                            Console.WriteLine("method name {0}, " + smE.Details, Environment.NewLine + method.Name);
                    }
                }
                //logger.LogInformation("method name {0}, " + smmattr.Details, Environment.NewLine + method.Name);
            }

        }
    }

    void attributeWorker(Type smUnion)
    {
        //Unsafe.SkipInit(out someElementsInfos);

        if (smUnion is Type type)
        {
            var customattrs = type.GetCustomAttributes(typeof(SomeElementsInfos));
            if (customattrs is not null)
            {

                foreach (var smtpattr in customattrs)
                {
                    if (smtpattr is SomeElementsInfos smE)
                    {
                        Console.WriteLine("class name {0}, " + smE.Details, Environment.NewLine + type.FullName);
                    }
                }
            }
        }
    }
}
