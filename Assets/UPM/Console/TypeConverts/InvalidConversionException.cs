using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvalidConversionException<TSource> : Exception
{
    public TSource Source { get; private set; }

    public InvalidConversionException(TSource source)
    {
        Source = source;
    }
}
