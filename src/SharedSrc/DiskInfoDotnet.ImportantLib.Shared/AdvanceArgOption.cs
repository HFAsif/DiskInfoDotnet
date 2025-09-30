namespace DiskInfoDotnet.ImportantLib.Shared;

using CrystalDiskInfoDotnet;
using CrystalDiskInfoDotnet.CheckDiskInfos;
using System;

public class CmdArgClass
{
    public class Options
    {
        public required string[] args;
        public OutPutInfos outPutInfos {  get; set; }
        public Options()
        {

        }
    }

//    public class AdvanceArgOption : Cmd.Option
//    {
//        readonly Action<string> action;
//        readonly string typeName;

//        public override string ArgumentValueName => typeName;

//        public AdvanceArgOption(string shortName, string longName, string description, string typeName, Action<string> action)
//            : base(shortName, longName, description)
//        {
//            this.typeName = typeName ?? "value";
//            this.action = action;
//#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
//            Default = null;
//#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
//        }

//        public override bool Set(string val, out string error)
//        {
//            action(val);
//            error = "";
//            return true;
//        }
//    }

    public class AdvanceNoArgOption : Cmd.Option
    {
        bool triggered;

        public override bool NeedArgument => false;


        public AdvanceNoArgOption(string shortName, string longName, string description)
            : base(shortName, longName, description)
        {

        }

        public override bool Set(string val, out string error)
        {
            triggered = true;
            error = "";
            return true;
        }

        public bool Get() => triggered;
    }
}

