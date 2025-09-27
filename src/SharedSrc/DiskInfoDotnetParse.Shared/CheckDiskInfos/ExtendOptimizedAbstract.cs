namespace CrystalDiskInfoDotnet.CheckDiskInfos;
using System.Reflection;

abstract class ExtendOptimizedAbstract
{
    [ExtendOptimizedWorker]
    public abstract void Infos(FieldInfo fieldInfo, ref readonly object fieldVal);

}