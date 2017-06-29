using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
public class CommandAttribute : System.Attribute
{
    public string CustomName = ""; // maybe we wanna make a command named something that already exists, we can just add a custom name and use that instead :)
                              // like this [Command(CustomName = "NameMe")]

    public bool OnlyIngame = false; // maybe we make it so commands marked with this can only be run while the game is playing
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