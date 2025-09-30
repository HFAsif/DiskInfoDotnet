namespace HelperClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
public class SomeElementsInfos : Attribute
{
    public readonly string Details;

    public SomeElementsInfos(string details)
    {
        Details = details;
    }


}