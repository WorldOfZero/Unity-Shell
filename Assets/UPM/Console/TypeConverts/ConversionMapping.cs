using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversionMapping {
    public Type SourceType { get; private set; }
    public Type TargetType { get; private set; }
    public IDataConverter Converter { get; private set; }

    public ConversionMapping(IDataConverter converter)
    {
        SourceType = converter.Source;
        TargetType = converter.Target;
        Converter = converter;
    }
}
