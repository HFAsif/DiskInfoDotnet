namespace System.Reflection
{
    [System.FlagsAttribute]
    public enum AssemblyFlags
    {
        PublicKey = 1,
        Retargetable = 256,
        WindowsRuntime = 512,
        ContentTypeMask = 3584,
        DisableJitCompileOptimizer = 16384,
        EnableJitCompileTracking = 32768,
    }
    public enum AssemblyHashAlgorithm
    {
        None = 0,
        MD5 = 32771,
        Sha1 = 32772,
        Sha256 = 32780,
        Sha384 = 32781,
        Sha512 = 32782,
    }
    public enum DeclarativeSecurityAction : short
    {
        None = (short)0,
        Demand = (short)2,
        Assert = (short)3,
        Deny = (short)4,
        PermitOnly = (short)5,
        LinkDemand = (short)6,
        InheritanceDemand = (short)7,
        RequestMinimum = (short)8,
        RequestOptional = (short)9,
        RequestRefuse = (short)10,
    }
    [System.FlagsAttribute]
    public enum ManifestResourceAttributes
    {
        Public = 1,
        Private = 2,
        VisibilityMask = 7,
    }
    [System.FlagsAttribute]
    public enum MethodImportAttributes : short
    {
        None = (short)0,
        ExactSpelling = (short)1,
        CharSetAnsi = (short)2,
        CharSetUnicode = (short)4,
        CharSetAuto = (short)6,
        CharSetMask = (short)6,
        BestFitMappingEnable = (short)16,
        BestFitMappingDisable = (short)32,
        BestFitMappingMask = (short)48,
        SetLastError = (short)64,
        CallingConventionWinApi = (short)256,
        CallingConventionCDecl = (short)512,
        CallingConventionStdCall = (short)768,
        CallingConventionThisCall = (short)1024,
        CallingConventionFastCall = (short)1280,
        CallingConventionMask = (short)1792,
        ThrowOnUnmappableCharEnable = (short)4096,
        ThrowOnUnmappableCharDisable = (short)8192,
        ThrowOnUnmappableCharMask = (short)12288,
    }
    [System.FlagsAttribute]
    public enum MethodSemanticsAttributes
    {
        Setter = 1,
        Getter = 2,
        Other = 4,
        Adder = 8,
        Remover = 16,
        Raiser = 32,
    }
}