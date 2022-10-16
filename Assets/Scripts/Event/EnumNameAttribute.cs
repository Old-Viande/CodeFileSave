using System;

[AttributeUsage(AttributeTargets.Field)]
public class EnumNameAttribute : Attribute
{
    public string Name;

    public EnumNameAttribute(string name)
    {
        Name = name;
    }
}
