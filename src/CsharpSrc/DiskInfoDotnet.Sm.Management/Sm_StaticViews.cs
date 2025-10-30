namespace DiskInfoDotnet.Sm.Management;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

[HelperClass.SomeElementsInfos($"{nameof(DiskInfoDotnet.Sm.Management.Sm_StaticViews)} does contains system management static method")]
public class Sm_StaticViews
{
    public static void GetSMManagerList(out ReadOnlyCollectionBuilder<object> list, LoadMScopModule loadMScopModule)
    {
        list = new ReadOnlyCollectionBuilder<object>();
        var AllScopInfos = loadMScopModule.GetType().GetProperties();
        foreach (var prop in AllScopInfos)
        {
            if (prop is not null && typeof(IEnumerable).IsAssignableFrom(prop.PropertyType) && prop.PropertyType.IsGenericType)
            {
                if (Attribute.IsDefined(prop.PropertyType.GetGenericArguments().First(), typeof(Win32_Attribute)))
                {
                    var val = prop.GetValue(loadMScopModule);

                    if (val is IEnumerable enumerable)
                    {
                        var enumerator = enumerable.GetEnumerator();
                        if (enumerator.MoveNext())
                        {
                            do
                            {
                                var current = enumerator.Current;
                                list.Add(current);
                            }
                            while (enumerator.MoveNext());
                        }
                    }
                }
            }
        }
    }
}
