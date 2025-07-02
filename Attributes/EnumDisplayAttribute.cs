namespace DPAS.Api.Attributes
{
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public sealed class EnumDisplayAttribute : Attribute
    {
        public EnumDisplayAttribute() { }

        public EnumDisplayAttribute(string name)
        {
            Name = name;
        }

        public string? Name { get; set; }
    }
}