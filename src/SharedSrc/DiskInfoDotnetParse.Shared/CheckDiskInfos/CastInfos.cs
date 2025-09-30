
namespace DiskInfoDotnetParse.Shared.CheckDiskInfos;

using HelperClass;
#if LoggerExist
using Microsoft.Extensions.Logging;
#endif

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

internal class CastInfos
{

#if LoggerExist
    static ILogger logger;
#endif

    public static void GenereteCastInfos(
#if LoggerExist
        ILogger _logger
#endif
        )
    {

#if LoggerExist
        logger = _logger;
#endif
        var currentAsm = AppDomain.CurrentDomain.GetAssemblies().GetEnumerator();
        //var memList = currentAsm.GetType().GetMembers();

        #region MyRegion
        //var asm = (Assembly)currentAsm.Current;

        ////var types = from type in asm.GetTypes()
        ////            where Attribute.IsDefined(type, typeof(SomeElementsInfos))
        ////            select type;

        ////var tpenum = types.GetEnumerator();

        ////if (tpenum.MoveNext())
        ////{
        ////    do
        ////    {
        ////        var tpcr = tpenum.Current;
        ////        var tarGetattr = tpcr.GetCustomAttribute<SomeElementsInfos>();
        ////        smList.Add(tpcr);

        ////        foreach (var method in tpcr.GetRuntimeMethods())
        ////        {
        ////            var smm = method.GetCustomAttribute<SomeElementsInfos>();
        ////            if (smm != null)
        ////            {
        ////                smList.Add(method);
        ////            }
        ////        }
        ////    }
        ////    while (tpenum.MoveNext());
        ////}


        //foreach (var type in smTypeList)
        //{

        //    if (Attribute.IsDefined(type, typeof(SomeElementsInfos)))
        //    {
        //        smTpList.Add(type);
        //    }
        //    foreach (var method in type.GetRuntimeMethods())
        //    {
        //        var smm = method.GetCustomAttribute<SomeElementsInfos>();
        //        if (smm != null)
        //        {
        //            smMtList.Add(method);
        //        }
        //    }
        //} 
        #endregion


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

                #region MyRegion

                //var types =  (from type in asm.GetTypes()
                //            where Attribute.IsDefined(type, typeof(SomeElementsInfos))
                //            select type).ToList().Cast<object>();

                //if (types.Count() > 0)
                //{
                //    smTpList.Add(types);
                //}


                //var methods = from type in asm.GetTypes() from method in type.GetRuntimeMethods() where method.GetCustomAttribute<SomeElementsInfos>() is not null select method;

                //if(methods.Count() > 0)
                //{
                //    smMtList = methods.ToList();
                //}
                #endregion
            }
            while (currentAsm.MoveNext());
        }

        foreach (var smUnion in smTpList.Union(smMtList))
        {
            //var smDynamic = ((dynamic)smUnion).GetCustomAttribute<SomeElementsInfos>();
            //Console.WriteLine("class name {0}, " + smDynamic.Details, Environment.NewLine + ((dynamic)smUnion).Name);

            if (smUnion is Type type)
            {
                attributeWorker(type);
                #region MyRegion
                //logger.LogInformation("class name {0}, " + someElementsInfos.Details, Environment.NewLine + type.FullName);

                //var customattrs = type.GetCustomAttributes(typeof(SomeElementsInfos));
                //if (customattrs is not null)
                //{

                //    foreach (var smtpattr in customattrs)
                //    {
                //        if (smtpattr is SomeElementsInfos someElementsInfos)
                //        {
                //            logger.LogInformation("class name {0}, " + someElementsInfos.Details, Environment.NewLine + type.FullName);
                //        }
                //    }
                //} 
                #endregion
            }
            else if (smUnion is MethodInfo method)
            {

                var smmattrs = method.GetCustomAttributes(typeof(SomeElementsInfos));
                if (smmattrs is not null)
                {
                    foreach (var smttr in smmattrs)
                    {
                        if (smttr is SomeElementsInfos smE)
#if LoggerExist
                        logger.LogInformation("method name {0}, " + smE.Details, Environment.NewLine + method.Name);
#else
                        Console.WriteLine("method name {0}, " + smE.Details, Environment.NewLine + method.Name);
#endif
                    }
                }
                //logger.LogInformation("method name {0}, " + smmattr.Details, Environment.NewLine + method.Name);
            }

        }
    }

    static void attributeWorker(Type smUnion)
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
#if LoggerExist
                        logger.LogInformation("class name {0}, " + smE.Details, Environment.NewLine + type.FullName);
#else
                        Console.WriteLine("class name {0}, " + smE.Details, Environment.NewLine + type.FullName);
#endif
                    }
                }
            }
        }
    }
}
