#nullable enable
namespace CrystalDiskInfoDotnet.CheckDiskInfos;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
class ExtendOptimizedAttr : Attribute
{
    public void AtaFieldInfos(Action GetingError, object ataInfos, out ReadOnlyCollectionBuilder<ObservableCollection<KeyValuePair<FieldInfo, object>>>? FiledsInfosBuild)
    {
        FiledsInfosBuild = [];

        if (ataInfos is not null and IList ataList)
        {
            var enumerator = ataList.GetEnumerator();
            if (enumerator.MoveNext())
            {
                do
                {
                    var ataClass = enumerator.Current;
                    var ataFields = ataClass.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);
                    ObservableCollection<KeyValuePair<FieldInfo, object>> FiledValuePairs = [];
                    foreach (var atafield in ataFields)
                    {
                        if (atafield.GetValue(ataClass) is not null and object ataFieldVal)
                        {
                            FiledValuePairs.Add(new KeyValuePair<FieldInfo, object>(atafield, ataFieldVal));
                        }
                    }
                    FiledsInfosBuild.Add(FiledValuePairs);
                }
                while (enumerator.MoveNext());
            }
        }
        else goto ErrorCode;
        return;

    ErrorCode:
        GetingError();
    }
}