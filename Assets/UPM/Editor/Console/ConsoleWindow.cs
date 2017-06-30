using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class ConsoleWindow : EditorWindow
{
    /// <summary>
    /// Clipboard: makes it so we can Copy and Paste stuff :)
    /// </summary>
    public static string clipboard
    {
        get { return GUIUtility.systemCopyBuffer; }
        set { GUIUtility.systemCopyBuffer = value; }
    }


    private StringBuilder currentLine = new StringBuilder(); // current command line
    private static List<string> previousCommands = new List<string>(); // console contents / previousCommands

    //UI variables
    private bool ConsoleLogEnabled = true; // should this console display stuff from the default unity console =?
    private static bool ScrollToBottomOnNewCommand = true; // should the scrollview scroll to the bottom when entering a new command ?
    private Vector2 scrollPosition = Vector2.zero; // current ScrollPosition
    public GUISkin layout;

    [MenuItem("Tools/Unity Shell %`")]
    static void InitializeWindow()
    {
        var window = GetWindow<ConsoleWindow>("Unity Shell", true);
        window.titleContent.tooltip = "Unity Shell: all around console (In Editor or In Game)"; 
        window.autoRepaintOnSceneChange = true;
        window.Show();
        window.layout = Resources.Load<GUISkin>("Skins/UnityShellDefault");
    }

    #region Preferences
    private static bool PreferencesLoaded = false;

    [PreferenceItem("Unity Shell")]
    public static void PreferencesGUI()
    {
        //get values    (happens on start up)
        if (!PreferencesLoaded)
        {
            //editorprefs something
            EditorPrefs.GetBool("ScrollToBottmWhenNewCommand", true);
            PreferencesLoaded = true;
        }

        //GUI that is showing   (editorGuiLayout stuff)
        EditorGUILayout.HelpBox("Hello, I'm testing this PreferencesGUI(), thingy \nand its working okey :)", MessageType.Info);
        EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));

        bool NewValue1 = EditorPrefs.GetBool("ScrollToBottmWhenNewCommand", true);
        EditorGUILayout.LabelField("Auto Scroll down when Typing");
        NewValue1 = EditorGUILayout.Toggle(NewValue1);

        EditorGUILayout.EndHorizontal();

        //set values    (happens when you change the values in prefrences)
        if (GUI.changed)
        {
            EditorPrefs.SetBool("ScrollToBottmWhenNewCommand", NewValue1);
            PrefsUpdate();
            //editorprefs something
        }
    }

    public static void PrefsUpdate()
    {
        ScrollToBottomOnNewCommand = EditorPrefs.GetBool("ScrollToBottmWhenNewCommand", true);
    }
    #endregion

    #region OnEnable and OnDisable/OnDestroy
    void OnDestroy()
    {
        Application.logMessageReceived -= HandleConsole;
    }
    void OnDisable()
    {
        Application.logMessageReceived -= HandleConsole;
    }
    void OnEnable()
    {
        Application.logMessageReceived -= HandleConsole; // reset before, setting it again.. dont know why i cant use !=
        Application.logMessageReceived += HandleConsole;
    }
    #endregion

    void HandleConsole(string logmessage, string trace, LogType type)
    {
        if (ConsoleLogEnabled)
        {
            Log("<color=grey>"+logmessage+"</color>"); // just adding a custom color, so its easier to see :)
            Repaint();
        }
    }

    public void OnGUI()
    {
        #region Events
        if (EditorWindow.focusedWindow == this)
        {
            Event e = Event.current; // the current event

            switch (e.type)
            {
                //this Event makes it posible todo Copy and Paste
                case EventType.ValidateCommand:

                    //Debug.Log("CommandName: " + e.commandName);
                    if (e.commandName == "Paste")
                    {
                        currentLine.Append(clipboard); // paste from clipboard
                    }
                    else if (e.commandName == "Copy")
                    {
                        clipboard = currentLine.ToString(); // set new clipboard
                    }

                    e.Use(); // use the event 
                    Repaint();
                    break;

                case EventType.KeyDown:
                    switch (e.keyCode)
                    {
                        case KeyCode.Backspace:
                            if(currentLine.Length > 0) // fixes so you cant remove nothing
                                currentLine = currentLine.Remove(currentLine.Length - 1, 1);
                            break;

                        case KeyCode.Return:

                            if (currentLine.Length == 0) // dont execute nothing, and/or make a new empty line
                                return;

                            previousCommands.Add("> " + currentLine.ToString());
                            CommandDiscovery.Build().Invoke(currentLine.ToString()); // sorry for changing, but i added a function that gets All the Project Assemblies inside this unity project, but if we cant get an Assembly we can manualy add it
                            currentLine = currentLine.Remove(0, currentLine.Length);

                            if (ScrollToBottomOnNewCommand) // scrolls the scrollView to the bottom so we can see what we executed :)
                                scrollPosition = new Vector2(scrollPosition.x, float.MaxValue); // <- this actualy works :D
                            break;

                        default:
                            currentLine = currentLine.Append(e.character);

                            if (ScrollToBottomOnNewCommand) // scrolls the scrollView to the bottom so we can see what we typed :)
                                scrollPosition = new Vector2(scrollPosition.x, float.MaxValue); // <- this actualy works :D
                            break;
                    }

                    //Debug.Log("Current Keycode: " + Event.current.keyCode.ToString() + " Character: " +
                    //          Event.current.character);
                    currentLine.Replace("\n", "");
                    currentLine.Replace("\0", "");

                    e.Use();
                    this.Repaint();
                    break;

                case EventType.KeyUp:
                    e.Use();
                    break;
            }
        }
        #endregion

        #region GUILayout
        //GUI.skin = layout; //globaly sets the GUISkin 

        #region Mini ToolBar
        //TODO: make the toolbar using the custom GUISkin, instead of default unity
        EditorGUILayout.BeginHorizontal(EditorStyles.toolbar, GUILayout.ExpandWidth(true));
        if (GUILayout.Button("Clear", EditorStyles.toolbarButton, GUILayout.Width(45f)))
        {
            ClearLog();
        }

        if (GUILayout.Button("Save", EditorStyles.toolbarButton, GUILayout.Width(45f)))
        {
            //save not added atm
        }

        ConsoleLogEnabled = GUILayout.Toggle(ConsoleLogEnabled, "Console Log", EditorStyles.toolbarButton, GUILayout.Width(100f));

        GUILayout.FlexibleSpace();

        EditorGUILayout.EndHorizontal();
        #endregion

        #region ScrollArea
        Rect ContentRect = new Rect(0, 16, position.width, position.height-16); // 16 + 16 = 32   and 3 from the left.
        GUILayout.BeginArea(ContentRect);

        scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, true, this.layout.horizontalScrollbar, this.layout.verticalScrollbar, this.layout.scrollView);
        GUILayout.BeginVertical();

        foreach (var previous in previousCommands)
        {
            GUILayout.Label(previous, this.layout.label);
        }

        GUILayout.Label("> " + currentLine.ToString(), this.layout.label);

        GUILayout.EndVertical();
        GUILayout.EndScrollView();
        GUILayout.EndArea();

        #endregion

        #endregion
    }

    [Command(CustomName = "Clear")] // added custom names, so we can have names that are simpler than the Methode name
    public static void ClearLog()
    {
        previousCommands = new List<string>();
    }

    public void Log(string message)
    {
        previousCommands.Add(message);
    }

    [Command]
    public static void Test()
    {
        Debug.Log("Test Message");
    }
}
