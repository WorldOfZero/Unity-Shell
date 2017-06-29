using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class CommandDiscovery
{
    private Dictionary<string, Command> commandMap;

    public static CommandDiscovery Build(params Assembly[] assemblies)
    {
        var commands = new Dictionary<string, Command>();

        foreach (var assembly in assemblies)
        {
            foreach (var type in assembly.GetTypes())
            {
                foreach (var method in type.GetMethods(BindingFlags.Instance | BindingFlags.Static |
                                                       BindingFlags.Public | BindingFlags.NonPublic |
                                                       BindingFlags.Default))
                {
                    foreach (var attribute in method.GetCustomAttributes(false).OfType<CommandAttribute>())
                    {
                        var command = new Command(method, method.Name);

                        if (commands.ContainsKey(command.ProgramName))
                        {
                            Debug.LogWarning("A command with the given name already exists: " + command.ProgramName);
                        }
                        else
                        {
                            commands.Add(command.ProgramName, command);
                        }
                    }
                }
            }
        }

        return new CommandDiscovery(commands);
    }

    protected CommandDiscovery(Dictionary<string, Command> commands)
    {
        commandMap = commands;
    }

    public void Invoke(string methodName)
    {
        var splitString = methodName.Split(' ');
        if (commandMap.ContainsKey(splitString[0]))
        {
            var argumentList = GenerateArgumentList(splitString.Skip(1));

            Execute(commandMap[splitString[0]].Program, argumentList);
        }
    }

    private IEnumerable<string> GenerateArgumentList(IEnumerable<string> arguments)
    {
        return arguments;
    }

    private void Execute(MethodInfo program, IEnumerable<string> arguments)
    {
        program.Invoke(null, arguments.ToArray());
    }
}
