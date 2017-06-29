using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class General : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    [Command]
    public static GameObject Find(string name)
    {
        Debug.Log(GameObject.Find(name));
        return GameObject.Find(name);
    }

    [Command]
    public static GameObject SetPosition([Input]GameObject input)
    {
        input.transform.position = Vector3.zero;
        return input;
    }

    [Command]
    public static void Hello()
    {
        Debug.Log("Hello");
    }

    [Command]
    public static void Spawn(string name, string type)
    {
        PrimitiveType primitiveType = PrimitiveType.Quad;
        switch (type)
        {
            case "Sphere": 
                primitiveType = PrimitiveType.Sphere;
                break;
        }

        var primitive = GameObject.CreatePrimitive(primitiveType);
        primitive.name = name;

        //Editor.Instantiate(primitive);
    }

    [Command]
    public static void FPS()
    {
        Debug.Log(1 / Time.deltaTime);
    }
}
