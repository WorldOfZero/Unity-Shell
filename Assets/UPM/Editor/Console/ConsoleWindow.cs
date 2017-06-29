using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class ConsoleWindow : EditorWindow
{
    private StringBuilder currentLine = new StringBuilder();
    private List<string> previousCommands = new List<string>();
    private Vector2 scrollPosition = Vector2.zero;

    public GUISkin layout;

    [MenuItem("Tools/Unity Shell %`")]
    static void InitializeWindow()
    {
        var window = GetWindow<ConsoleWindow>("Unity Shell", true);
        window.autoRepaintOnSceneChange = true;
        window.Show();
        window.layout = Resources.Load<GUISkin>("Skins/UnityShellDefault");
    }

    public void OnGUI()
    {
        if (EditorWindow.focusedWindow == this)
        {
            if (Event.current != null)
            {
                if (Event.current.type == EventType.KeyDown)
                {
                    switch (Event.current.keyCode)
                    {
                        case KeyCode.Backspace:
                            currentLine = currentLine.Remove(currentLine.Length - 1, 1);
                            break;

                        case KeyCode.Return:
                            CommandDiscovery.Build(this.GetType().Assembly, typeof(CommandDiscovery).Assembly).Invoke(currentLine.ToString());
                            previousCommands.Add(currentLine.ToString());
                            currentLine = currentLine.Remove(0, currentLine.Length);
                            break;

                        default:
                            currentLine = currentLine.Append(Event.current.character);
                            break;
                    }
                    //Debug.Log("Current Keycode: " + Event.current.keyCode.ToString() + " Character: " +
                    //          Event.current.character);
                    currentLine.Replace("\n", "");
                    currentLine.Replace("\0", "");
                    Event.current.Use();
                    this.Repaint();
                } else if (Event.current.type == EventType.KeyUp)
                {
                    Event.current.Use(); // Use the current event on Key Up so other elements don't mess with it.
                }
            }
        }

        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, false, true, this.layout.horizontalScrollbar, this.layout.verticalScrollbar, this.layout.scrollView);
        foreach (var previous in previousCommands)
        {
            EditorGUILayout.LabelField("> " + previous, this.layout.label);
        }
        EditorGUILayout.LabelField("> " + currentLine.ToString(), this.layout.label);
        
        EditorGUILayout.EndScrollView();
    }

    [Command]
    public static void Test()
    {
        Debug.Log("Test Message");
    }
}
