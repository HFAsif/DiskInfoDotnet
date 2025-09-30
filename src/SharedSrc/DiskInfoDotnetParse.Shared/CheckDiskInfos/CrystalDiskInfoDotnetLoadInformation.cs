namespace CrystalDiskInfoDotnet.CheckDiskInfos;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

public class CrystalDiskInfoDotnetLoadInformation 
{
    public required OutPutInfos InfoType { get; set; }
    public required object ataInfos { get; set; }

    void Distribution(
        [In] ObservableCollection<KeyValuePair<FieldInfo, object>> ataEnumerator, out object Extraction)
    {
        var Abstractmethod = typeof(ExtendOptimizedAbstract).GetMethods(BindingFlags.Instance | BindingFlags.Public).ToList().Find(a => a.IsAbstract);
        
        Extraction = Abstractmethod?.GetCustomAttribute<ExtendOptimizedWorker>() ?? throw new InvalidOperationException("getting Null exceptions");
        ExtendOptimizedAbstract extendOptimizedAbstract;
        Unsafe.SkipInit(out extendOptimizedAbstract);

        if (Extraction is not null and ExtendOptimizedWorker attr)
        {
            if (InfoType == OutPutInfos.ExtendedInfos)
            {
                extendOptimizedAbstract = new ExtendedInfos()
                { extendedInfos = attr.extendedInfos == null ? attr.extendedInfos = new() : throw new InvalidOperationException() };
            }
            else if (InfoType == OutPutInfos.OptimizedInfos)
            {
                extendOptimizedAbstract = new OptimizedInfos()
                {
                    GetingError = GetingError,
                    OptimizedInfosBuilder = attr.optimizedInfos == null ? attr.optimizedInfos = new ObservableCollection<KeyValuePair<string, string>>() : throw new InvalidOperationException() };
            }
            else goto GettingError;

            foreach (var ataValuePair in ataEnumerator)
            {
                var FieldKey = ataValuePair.Key;
                var FieldValule = ataValuePair.Value;
                extendOptimizedAbstract.Infos(FieldKey, ref FieldValule);
            }
        }
        else goto GettingError;
        return;
        GettingError:
        {
            GetingError();
        }

    }


    public void LoadInformation(out ObservableCollection<ExtendedInfosStruct> infoForCasts, out ReadOnlyCollectionBuilder<ObservableCollection<KeyValuePair<string, string>>> OptimizedListBuilder)
    {
        Unsafe.SkipInit(out infoForCasts);
        Unsafe.SkipInit(out OptimizedListBuilder);

        if (InfoType == OutPutInfos.ExtendedInfos)
            infoForCasts = new();
        else if (InfoType == OutPutInfos.OptimizedInfos)
            OptimizedListBuilder = [];
        else GetingError();


        if (Attribute.IsDefined(typeof(OptimizedInfos), typeof(ExtendOptimizedAttr)))
        {
            var attr = typeof(OptimizedInfos).GetCustomAttribute<ExtendOptimizedAttr>();
            attr?.AtaFieldInfos(GetingError, ataInfos, out var filedsInfosBuild);
            Unsafe.SkipInit(out filedsInfosBuild);
            if (filedsInfosBuild != null)
            {
                var enumerator = filedsInfosBuild.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    var ataEnumerator = enumerator.Current;

                    Distribution(ataEnumerator, out var ExtractedInfos);


                    if (ExtractedInfos is ExtendOptimizedWorker worker)
                    {
                        if (worker.extendedInfos is not null)
                            infoForCasts.Add(worker.extendedInfos);
                        if (worker.optimizedInfos is not null)
                            OptimizedListBuilder.Add(worker.optimizedInfos);
                    }
                }
            }
            else GetingError();
        }

    }


    void GetingError()
    {
        Debugger.Break();
        //not setted or Invalid Operation so
        throw new InvalidOperationException($"Getting exception ");
    }
}
