namespace CrystalDiskInfoDotnet.CheckDiskInfos;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using HelperClass;

[ExtendOptimizedAttr]
[SomeElementsInfos("It does contains all infos except 0 and bool value")]
class OptimizedInfos : ExtendOptimizedAbstract
{
    public required Action GetingError;
    public required dynamic OptimizedInfosBuilder { get; set; }

    public override void Infos(FieldInfo fieldInfo, ref readonly object fieldVal)
    {
        //if (OptimizedInfos != typeof(ObservableCollection<KeyValuePair<string, string>>))
        //    throw new InvalidOperationException("null object");

        string fieldValStr = fieldVal.ToString() ?? throw new InvalidCastException($"getting problem at field val class name {nameof(OptimizedInfos)}"); 

        if (new Type[] { typeof(ushort), typeof(uint), typeof(ulong), typeof(byte), typeof(string), typeof(int) }.Contains(fieldInfo.FieldType)
                            && !string.IsNullOrEmpty(fieldVal?.ToString()))
        {
            if (fieldValStr.Contains("-")) return;
            if (fieldValStr.All(Char.IsDigit) && int.TryParse(fieldValStr, out var rs) && rs is 0) return;

            OptimizedInfosBuilder.Add(new KeyValuePair<string, string>(fieldInfo.Name, fieldValStr));
        }
    }
}
