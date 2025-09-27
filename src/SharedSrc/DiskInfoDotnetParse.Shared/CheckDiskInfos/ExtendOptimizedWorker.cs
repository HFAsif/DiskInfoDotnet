#nullable enable
namespace CrystalDiskInfoDotnet.CheckDiskInfos;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
sealed class ExtendOptimizedWorker : Attribute
{
    public ExtendedInfosStruct? extendedInfos;
    public ObservableCollection<KeyValuePair<string, string>>? optimizedInfos;
}
