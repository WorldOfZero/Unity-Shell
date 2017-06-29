using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;


public class CommandAttribute : System.Attribute
{
}

public class Command
{
    private Type inputType;
    private Type outputType;
    private Dictionary<string, Type> arguments;
    private MethodInfo program;
    private string programName;

    public Command(MethodInfo program, string name)
    {
        this.program = program;
        this.programName = name;
    }

    public MethodInfo Program
    {
        get { return program; }
    }

    public string ProgramName {
        get { return programName; }
    }
}