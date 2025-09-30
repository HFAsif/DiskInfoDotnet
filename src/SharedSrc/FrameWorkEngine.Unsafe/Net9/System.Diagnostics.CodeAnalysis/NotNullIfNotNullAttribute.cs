#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Diagnostics.CodeAnalysis
{
    [System.AttributeUsageAttribute(System.AttributeTargets.Field | System.AttributeTargets.Parameter | System.AttributeTargets.Property, Inherited = false)]
    public sealed partial class AllowNullAttribute : System.Attribute
    {
        public AllowNullAttribute() { }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Parameter, Inherited = false)]
    public sealed partial class ConstantExpectedAttribute : System.Attribute
    {
        public ConstantExpectedAttribute() { }
        public object? Max { get { throw null; } set { } }
        public object? Min { get { throw null; } set { } }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Field | System.AttributeTargets.Parameter | System.AttributeTargets.Property, Inherited = false)]
    public sealed partial class DisallowNullAttribute : System.Attribute
    {
        public DisallowNullAttribute() { }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Method, Inherited = false)]
    public sealed partial class DoesNotReturnAttribute : System.Attribute
    {
        public DoesNotReturnAttribute() { }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Parameter, Inherited = false)]
    public sealed partial class DoesNotReturnIfAttribute : System.Attribute
    {
        public DoesNotReturnIfAttribute(bool parameterValue) { }
        public bool ParameterValue { get { throw null; } }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Class | System.AttributeTargets.Field | System.AttributeTargets.GenericParameter | System.AttributeTargets.Interface | System.AttributeTargets.Method | System.AttributeTargets.Parameter | System.AttributeTargets.Property | System.AttributeTargets.ReturnValue | System.AttributeTargets.Struct, Inherited = false)]
    public sealed partial class DynamicallyAccessedMembersAttribute : System.Attribute
    {
        public DynamicallyAccessedMembersAttribute(System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes memberTypes) { }
        public System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes MemberTypes { get { throw null; } }
    }
    [System.FlagsAttribute]
    public enum DynamicallyAccessedMemberTypes
    {
        All = -1,
        None = 0,
        PublicParameterlessConstructor = 1,
        PublicConstructors = 3,
        NonPublicConstructors = 4,
        PublicMethods = 8,
        NonPublicMethods = 16,
        PublicFields = 32,
        NonPublicFields = 64,
        PublicNestedTypes = 128,
        NonPublicNestedTypes = 256,
        PublicProperties = 512,
        NonPublicProperties = 1024,
        PublicEvents = 2048,
        NonPublicEvents = 4096,
        Interfaces = 8192,
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Constructor | System.AttributeTargets.Field | System.AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public sealed partial class DynamicDependencyAttribute : System.Attribute
    {
        public DynamicDependencyAttribute(System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes memberTypes, string typeName, string assemblyName) { }
        public DynamicDependencyAttribute(System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes memberTypes, System.Type type) { }
        public DynamicDependencyAttribute(string memberSignature) { }
        public DynamicDependencyAttribute(string memberSignature, string typeName, string assemblyName) { }
        public DynamicDependencyAttribute(string memberSignature, System.Type type) { }
        public string? AssemblyName { get { throw null; } }
        public string? Condition { get { throw null; } set { } }
        public string? MemberSignature { get { throw null; } }
        public System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes MemberTypes { get { throw null; } }
        public System.Type? Type { get { throw null; } }
        public string? TypeName { get { throw null; } }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Assembly | System.AttributeTargets.Class | System.AttributeTargets.Constructor | System.AttributeTargets.Event | System.AttributeTargets.Method | System.AttributeTargets.Property | System.AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
    public sealed partial class ExcludeFromCodeCoverageAttribute : System.Attribute
    {
        public ExcludeFromCodeCoverageAttribute() { }
        public string? Justification { get { throw null; } set { } }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Assembly | System.AttributeTargets.Class | System.AttributeTargets.Constructor | System.AttributeTargets.Delegate | System.AttributeTargets.Enum | System.AttributeTargets.Event | System.AttributeTargets.Field | System.AttributeTargets.Interface | System.AttributeTargets.Method | System.AttributeTargets.Module | System.AttributeTargets.Property | System.AttributeTargets.Struct, Inherited = false)]
    public sealed partial class ExperimentalAttribute : System.Attribute
    {
        public ExperimentalAttribute(string diagnosticId) { }
        public string DiagnosticId { get { throw null; } }
        public string? UrlFormat { get { throw null; } set { } }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    public sealed class FeatureGuardAttribute : System.Attribute
    {
        public FeatureGuardAttribute(System.Type featureType) { }
        public System.Type FeatureType { get { throw null; } }
    }
    [System.AttributeUsage(System.AttributeTargets.Property, Inherited = false)]
    public sealed class FeatureSwitchDefinitionAttribute : Attribute
    {
        public FeatureSwitchDefinitionAttribute(string switchName) { }
        public string SwitchName { get { throw null; } }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Field | System.AttributeTargets.Parameter | System.AttributeTargets.Property | System.AttributeTargets.ReturnValue, Inherited = false)]
    public sealed partial class MaybeNullAttribute : System.Attribute
    {
        public MaybeNullAttribute() { }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Parameter, Inherited = false)]
    public sealed partial class MaybeNullWhenAttribute : System.Attribute
    {
        public MaybeNullWhenAttribute(bool returnValue) { }
        public bool ReturnValue { get { throw null; } }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Method | System.AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    public sealed partial class MemberNotNullAttribute : System.Attribute
    {
        public MemberNotNullAttribute(string member) { }
        public MemberNotNullAttribute(params string[] members) { }
        public string[] Members { get { throw null; } }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Method | System.AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    public sealed partial class MemberNotNullWhenAttribute : System.Attribute
    {
        public MemberNotNullWhenAttribute(bool returnValue, string member) { }
        public MemberNotNullWhenAttribute(bool returnValue, params string[] members) { }
        public string[] Members { get { throw null; } }
        public bool ReturnValue { get { throw null; } }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Field | System.AttributeTargets.Parameter | System.AttributeTargets.Property | System.AttributeTargets.ReturnValue, Inherited = false)]
    public sealed partial class NotNullAttribute : System.Attribute
    {
        public NotNullAttribute() { }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Parameter | System.AttributeTargets.Property | System.AttributeTargets.ReturnValue, AllowMultiple = true, Inherited = false)]
    public sealed partial class NotNullIfNotNullAttribute : System.Attribute
    {
        public NotNullIfNotNullAttribute(string parameterName) { }
        public string ParameterName { get { throw null; } }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Parameter, Inherited = false)]
    public sealed partial class NotNullWhenAttribute : System.Attribute
    {
        public NotNullWhenAttribute(bool returnValue) { }
        public bool ReturnValue { get { throw null; } }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Constructor | System.AttributeTargets.Event | System.AttributeTargets.Method | System.AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed partial class RequiresAssemblyFilesAttribute : System.Attribute
    {
        public RequiresAssemblyFilesAttribute() { }
        public RequiresAssemblyFilesAttribute(string message) { }
        public string? Message { get { throw null; } }
        public string? Url { get { throw null; } set { } }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Class | System.AttributeTargets.Constructor | System.AttributeTargets.Method, Inherited = false)]
    public sealed partial class RequiresDynamicCodeAttribute : System.Attribute
    {
        public RequiresDynamicCodeAttribute(string message) { }
        public string Message { get { throw null; } }
        public string? Url { get { throw null; } set { } }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Class | System.AttributeTargets.Constructor | System.AttributeTargets.Method, Inherited = false)]
    public sealed partial class RequiresUnreferencedCodeAttribute : System.Attribute
    {
        public RequiresUnreferencedCodeAttribute(string message) { }
        public string Message { get { throw null; } }
        public string? Url { get { throw null; } set { } }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Constructor, AllowMultiple = false, Inherited = false)]
    public sealed partial class SetsRequiredMembersAttribute : System.Attribute
    {
        public SetsRequiredMembersAttribute() { }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Field | System.AttributeTargets.Parameter | System.AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public sealed partial class StringSyntaxAttribute : System.Attribute
    {
        public const string CompositeFormat = "CompositeFormat";
        public const string DateOnlyFormat = "DateOnlyFormat";
        public const string DateTimeFormat = "DateTimeFormat";
        public const string EnumFormat = "EnumFormat";
        public const string GuidFormat = "GuidFormat";
        public const string Json = "Json";
        public const string NumericFormat = "NumericFormat";
        public const string Regex = "Regex";
        public const string TimeOnlyFormat = "TimeOnlyFormat";
        public const string TimeSpanFormat = "TimeSpanFormat";
        public const string Uri = "Uri";
        public const string Xml = "Xml";
        public StringSyntaxAttribute(string syntax) { }
        public StringSyntaxAttribute(string syntax, params object?[] arguments) { }
        public object?[] Arguments { get { throw null; } }
        public string Syntax { get { throw null; } }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    [System.Diagnostics.ConditionalAttribute("CODE_ANALYSIS")]
    public sealed partial class SuppressMessageAttribute : System.Attribute
    {
        public SuppressMessageAttribute(string category, string checkId) { }
        public string Category { get { throw null; } }
        public string CheckId { get { throw null; } }
        public string? Justification { get { throw null; } set { } }
        public string? MessageId { get { throw null; } set { } }
        public string? Scope { get { throw null; } set { } }
        public string? Target { get { throw null; } set { } }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    public sealed partial class UnconditionalSuppressMessageAttribute : System.Attribute
    {
        public UnconditionalSuppressMessageAttribute(string category, string checkId) { }
        public string Category { get { throw null; } }
        public string CheckId { get { throw null; } }
        public string? Justification { get { throw null; } set { } }
        public string? MessageId { get { throw null; } set { } }
        public string? Scope { get { throw null; } set { } }
        public string? Target { get { throw null; } set { } }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Method | System.AttributeTargets.Parameter | System.AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public sealed partial class UnscopedRefAttribute : System.Attribute
    {
        public UnscopedRefAttribute() { }
    }
}