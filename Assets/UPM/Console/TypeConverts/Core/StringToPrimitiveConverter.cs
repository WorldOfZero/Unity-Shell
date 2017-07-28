using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class StringToPrimitiveConverter : IDataConverter<string, PrimitiveType> {

    public override PrimitiveType Convert(string source)
    {
        switch (source)
        {
            case "Quad":
                return PrimitiveType.Quad;
            case "Plane":
                return PrimitiveType.Plane;
            case "Cube":
                return PrimitiveType.Cube;
            case "Sphere":
                return PrimitiveType.Sphere;
            case "Capsule":
                return PrimitiveType.Capsule;
            case "Cylinder":
                return PrimitiveType.Cylinder; 
        }
        throw new InvalidConversionException<string>(source);
    }

    public override Type Source {
        get { return typeof(string); }
    }

    public override Type Target {
        get { return typeof(PrimitiveType); }
    }
}
