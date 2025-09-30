#pragma warning disable

namespace DiskInfoDotnet.ImportantLib.Shared;

using CrystalDiskInfoDotnet;
using CrystalDiskInfoDotnet.CheckDiskInfos;
using HelperClass;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HelperClass;

[SomeElementsInfos("This class coded by 0XDFORD")]
public class CommandLineViewModule
{
    public class CommandLineParser
    {
        //public required ILogger _logger;

        Dictionary<string, Option> optionsDict = new Dictionary<string, Option>(StringComparer.Ordinal);
        IList<Option> miscOptions = new List<Option>();
        IList<Option> fileOptions = new List<Option>();
        Option defaultOption;
        string[] args;
        public required Cac.Options cacOptions {  get; set; }

        public void Parse(Cac.Options options)
        {
            AddAllOptions();
            args = options.args;
            if (args.Length == 0)
            {
                
                cacOptions.outPutInfos = OutPutInfos.OptimizedInfos;
                return;
                //Usage();
                //Exit(1);
            }

            for (int i = 0; i < args.Length; i++)
            {
                var arg = args[i];

                string val = null;
                if (optionsDict.TryGetValue(arg, out var option))
                {
                    if (option.NeedArgument)
                    {
                        if (++i >= args.Length)
                            ExitError("Missing options value");
                        val = args[i];
                    }
                }
                else
                {
                    option = defaultOption;
                    val = arg;
                }

                if (!option.Set(val, out string errorString))
                    ExitError(errorString);

                if(option.ShortName.Contains("E") || option.ShortName.Contains("e"))
                {
                    cacOptions.outPutInfos = OutPutInfos.ExtendedInfos;
                    //break;
                }
                else if (option.ShortName.Contains("O") || option.ShortName.Contains("o"))
                {
                    cacOptions.outPutInfos = OutPutInfos.OptimizedInfos;
                    //break;
                }
                else if (option.ShortName.Contains("F") || option.ShortName.Contains("f"))
                {
                    cacOptions.outPutInfos = OutPutInfos.OptimizedInfos;
                    //break;
                }
            }
        }


        void AddAllOptions()
        {
            miscOptions.Add(new Cac.AdvanceNoArgOption("E", "Extended Infos", "Show this message"));

            miscOptions.Add(new Cac.AdvanceNoArgOption("O", "Optimized infos", "Show this message"));

            miscOptions.Add(new Cac.AdvanceNoArgOption("F", "Full infos", "Show this message"));
            //miscOptions.Add(new Cac.AdvanceArgOption("r", null, "Scan for .NET files in all subdirs", "dir", (val) =>
            //{
            //    //cacOptions = new Cac.Options();
            //}
            //));

            //miscOptions.Add(new Cac.AdvanceArgOption("r", null, "Scan for .NET files in all subdirs", "dir", (val) =>
            //{
            //    cacOptions = new CmdArgClass.Options();
            //}
            //));

            //fileOptions.Add(defaultOption);

            defaultOption = new Cac.AdvanceNoArgOption("", "", "");

            AddOptions(miscOptions);
            //AddOptions(fileOptions);
        }

        void AddOptions(IEnumerable<Option> options)
        {
           
            foreach (var option in options)
            {
                AddOption(option, option.ShortName);
                AddOption(option, option.LongName);
            }
        }

        void AddOption(Option option, string name)
        {
            if (name == null)
                return;
            if (optionsDict.ContainsKey(name))
                throw new ApplicationException($"Option {name} is present twice!");
            optionsDict[name] = option;
        }


        void Usage()
        {

        }

        void PrintInfos(string desc, Infos infos)
        {
            ALLExtensions.Log("{0}", desc);
            foreach (var info in infos.GetInfos())
                PrintOptionAndExplanation(info.name, info.desc);
        }

        void ExitError(string msg)
        {
            Usage();
            ALLExtensions.Log("\n\nERROR: {0}\n", msg);
            Exit(2);
        }

        void PrintOption(Option option)
        {
            string defaultAndDesc;
            if (option.NeedArgument && option.Default != null)
                defaultAndDesc = $"{option.Description} ({option.Default})";
            else
                defaultAndDesc = option.Description;
            PrintOptionAndExplanation(GetOptionAndArgName(option, option.ShortName ?? option.LongName), defaultAndDesc);
            if (option.ShortName != null && option.LongName != null)
                PrintOptionAndExplanation(option.LongName, $"Same as {option.ShortName}");
        }

        void PrintOptionAndExplanation(string option, string explanation)
        {
            const int maxCols = 16;
            const string prefix = "  ";
            string left = string.Format($"{{0,-{maxCols}}}", option);
            if (option.Length > maxCols)
            {
                ALLExtensions.Log("{0}{1}", prefix, left);
                ALLExtensions.Log("{0}{1} {2}", prefix, new string(' ', maxCols), explanation);
            }
            else
                ALLExtensions.Log("{0}{1} {2}", prefix, left, explanation);
        }

        string GetOptionAndArgName(Option option, string optionName)
        {
            if (option.NeedArgument)
                return optionName + " " + option.ArgumentValueName.ToUpperInvariant();
            else
                return optionName;
        }

        class Info
        {
            public object value;
            public string name;
            public string desc;

            public Info(object value, string name, string desc)
            {
                this.value = value;
                this.name = name;
                this.desc = desc;
            }
        }


        class Infos
        {
            List<Info> infos = new List<Info>();

            public void Add(object value, string name, string desc) => infos.Add(new Info(value, name, desc));
            public IEnumerable<Info> GetInfos() => infos;

            public bool GetValue(string name, out object value)
            {
                foreach (var info in infos)
                {
                    if (name.Equals(info.name, StringComparison.OrdinalIgnoreCase))
                    {
                        value = info.value;
                        return true;
                    }
                }
                value = null;
                return false;
            }
        }

        void Exit(int exitCode) => throw new GettingExceptions(exitCode);


    }

    #region Rv
    public class NameRegex
    {
        Regex regex;
        public const char invertChar = '!';

        public bool MatchValue { get; private set; }

        public NameRegex(string regex)
        {
            if (regex.Length > 0 && regex[0] == invertChar)
            {
                regex = regex.Substring(1);
                MatchValue = false;
            }
            else
                MatchValue = true;

            this.regex = new Regex(regex);
        }

        // Returns true if the regex matches. Use MatchValue to get result.
        public bool IsMatch(string s) => regex.IsMatch(s);

        public override string ToString()
        {
            if (!MatchValue)
                return invertChar + regex.ToString();
            return regex.ToString();
        }
    }

    public class NameRegexes
    {
        IList<NameRegex> regexes;
        public bool DefaultValue { get; set; }
        public const char regexSeparatorChar = '&';
        public IList<NameRegex> Regexes => regexes;

        public NameRegexes() : this("") { }
        public NameRegexes(string regex) => Set(regex);

        public void Set(string regexesString)
        {
            regexes = new List<NameRegex>();
            if (regexesString != "")
            {
                foreach (var regex in regexesString.Split(new char[] { regexSeparatorChar }))
                    regexes.Add(new NameRegex(regex));
            }
        }

        public bool IsMatch(string s)
        {
            foreach (var regex in regexes)
            {
                if (regex.IsMatch(s))
                    return regex.MatchValue;
            }

            return DefaultValue;
        }

        public override string ToString()
        {
            var s = "";
            for (int i = 0; i < regexes.Count; i++)
            {
                if (i > 0)
                    s += regexSeparatorChar;
                s += regexes[i].ToString();
            }
            return s;
        }
    }
    public abstract class Option
    {
        const string SHORTNAME_PREFIX = "-";
        const string LONGNAME_PREFIX = "--";

        string shortName;
        string longName;
        string description;
        object defaultVal;

        public string ShortName => shortName;
        public string LongName => longName;
        public string Description => description;

        public object Default
        {
            get => defaultVal;
            protected set => defaultVal = value;
        }

        public virtual bool NeedArgument => true;
        public virtual string ArgumentValueName => "value";

        // Returns true if the new value is set, or false on error. error string is also updated.
        public abstract bool Set(string val, out string error);

        public Option(string shortName, string longName, string description)
        {
            if (shortName != null)
                this.shortName = SHORTNAME_PREFIX + shortName;
            if (longName != null)
                this.longName = LONGNAME_PREFIX + longName;
            this.description = description;
        }
    }

    public class BoolOption : Option
    {
        bool val;
        public BoolOption(string shortName, string longName, string description, bool val)
            : base(shortName, longName, description) => Default = this.val = val;

        public override string ArgumentValueName => "bool";

        public override bool Set(string newVal, out string error)
        {
            if (string.Equals(newVal, "false", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(newVal, "off", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(newVal, "0", StringComparison.OrdinalIgnoreCase))
            {
                val = false;
            }
            else
                val = true;
            error = "";
            return true;
        }

        public bool Get() => val;
    }

    public class IntOption : Option
    {
        int val;

        public IntOption(string shortName, string longName, string description, int val)
            : base(shortName, longName, description) => Default = this.val = val;

        public override string ArgumentValueName => "int";

        public override bool Set(string newVal, out string error)
        {
            if (!int.TryParse(newVal, out int newInt))
            {
                error = $"Not an integer: '{newVal}'";
                return false;
            }
            val = newInt;
            error = "";
            return true;
        }

        public int Get() => val;
    }

    public class StringOption : Option
    {
        string val;

        public override string ArgumentValueName => "string";

        public StringOption(string shortName, string longName, string description, string val)
            : base(shortName, longName, description) => Default = this.val = val;

        public override bool Set(string newVal, out string error)
        {
            val = newVal;
            error = "";
            return true;
        }

        public string Get() => val;
    }

    public class NameRegexOption : Option
    {
        NameRegexes val;

        public override string ArgumentValueName => "regex";

        public NameRegexOption(string shortName, string longName, string description, string val)
            : base(shortName, longName, description) => Default = this.val = new NameRegexes(val);

        public override bool Set(string newVal, out string error)
        {
            try
            {
                var regexes = new NameRegexes();
                regexes.Set(newVal);
                val = regexes;
            }
            catch (ArgumentException)
            {
                error = $"Could not parse regex '{newVal}'";
                return false;
            }
            error = "";
            return true;
        }

        public NameRegexes Get() => val;
    }

    public class RegexOption : Option
    {
        Regex val;

        public override string ArgumentValueName => "regex";

        public RegexOption(string shortName, string longName, string description, string val)
            : base(shortName, longName, description) => Default = this.val = new Regex(val);

        public override bool Set(string newVal, out string error)
        {
            try
            {
                val = new Regex(newVal);
            }
            catch (ArgumentException)
            {
                error = $"Could not parse regex '{newVal}'";
                return false;
            }
            error = "";
            return true;
        }

        public Regex Get() => val;
    }

    public class NoArgOption : Option
    {
        Action action;
        bool triggered;

        public override bool NeedArgument => false;

        public NoArgOption(string shortName, string longName, string description)
            : this(shortName, longName, description, null)
        {
        }

        public NoArgOption(string shortName, string longName, string description, Action action)
            : base(shortName, longName, description) => this.action = action;

        public override bool Set(string val, out string error)
        {
            triggered = true;
            action?.Invoke();
            error = "";
            return true;
        }

        public bool Get() => triggered;
    }

    public class OneArgOption : Option
    {
        Action<string> action;
        string typeName;

        public override string ArgumentValueName => typeName;

        public OneArgOption(string shortName, string longName, string description, string typeName, Action<string> action)
            : base(shortName, longName, description)
        {
            this.typeName = typeName ?? "value";
            this.action = action;
            Default = null;
        }

        public override bool Set(string val, out string error)
        {
            action(val);
            error = "";
            return true;
        }
    }
    #endregion
}
