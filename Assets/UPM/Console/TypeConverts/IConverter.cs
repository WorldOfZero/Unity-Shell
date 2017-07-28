using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IDataConverter
{
    public abstract Type Source { get; }
    public abstract Type Target { get; }
    public abstract object GeneralConversion(object source);
}

public abstract class IDataConverter<TSource, TTarget> : IDataConverter
{
    public override object GeneralConversion(object source)
    {
        return Convert((TSource)source);
    }

    public abstract TTarget Convert(TSource source);
}
