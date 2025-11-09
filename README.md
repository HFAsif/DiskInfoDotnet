Diskinfo is dotnet format of crystaldiskinfo 50%

https://www.nuget.org/packages/DiskInfoDotnet

Sample Code
```
using System.Linq;
```

```
var IsElevated = DiskInfoDotnet.MainEntry.Run(out var ataLists, out var loadMScopModule, out var extractionResult, true);
DiskInfoDotnet.Sm.Management.Sm_StaticViews.GetSMManagerList(out var SmmanagerList, loadMScopModule);
System.Console.WriteLine(extractionResult);
System.Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(SmmanagerList, Newtonsoft.Json.Formatting.Indented));
System.Console.WriteLine(System.Environment.NewLine + "Extracting Optimized Infos {0} ", System.Environment.NewLine);
if (IsElevated && ataLists is System.Collections.IEnumerable collection)
{
    foreach (var item in collection)
    {
        item.GetType().GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance).ToList().ForEach(field =>
        {
            if (new System.Type[] { typeof(ushort), typeof(uint), typeof(ulong), typeof(byte), typeof(string), typeof(int) }.Contains(field.FieldType)
                    && !string.IsNullOrEmpty(field.GetValue(item)?.ToString()) && field.GetValue(item)?.ToString() is string)
            {
                if ((field.GetValue(item)?.ToString()).Contains("-")) return;
                if ((field.GetValue(item)?.ToString()).All(System.Char.IsDigit) && int.TryParse((field.GetValue(item)?.ToString()), out var rs) && rs is 0) return;
                System.Console.WriteLine(field.Name + "  " + ((field.GetValue(item)?.ToString()) ?? "null"));
            }
        });
        System.Console.WriteLine(System.Environment.NewLine);
    }
}
System.Console.ReadLine();
```

added
1. Extract Device information from sm management , using json properties 
2. Anycpu 
3. vs studio 2022 dev powershell [Digital signeture and other things ]
4. digital signeture at [DiskInfoDotnet.Related] 
5. code optimized
6. device information
7. using task
8. delay sign in +
9. using probing privatePath at app.config 
10. has some more things

u can get information in two format cmd ; args 
```
exe -E ex {u will get all information which extracted }
exe -O op {u will get optimized information except bool 0 and null }
```

example 
<img width="1920" height="1030" alt="{3F5CD021-19C8-4837-99AD-B5A4740ACD54}" src="https://github.com/user-attachments/assets/214340bd-0b31-47db-93dc-b1bd61271f82" />


