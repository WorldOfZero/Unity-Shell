using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringToEnumConverter : IDataConverter<string, Enum> {

    public override Type Source
    {
        get { return typeof(string); }
    }

    private Type _target = typeof(Enum);
    public override Type Target
    {
        get { return typeof(Enum); }
    }

    // TODO: Implement 2 argument convert function
    public Type ConversionEnumType
    {
        get { return _target; }
        set { _target = value; }
    }

    public override Enum Convert(string source)
    {
        return (Enum)Enum.Parse(ConversionEnumType, source);
    }
}
