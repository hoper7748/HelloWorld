using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CustomDebug
{

    public static void Log(string value)
    {
#if UNITY_EDITOR
        Debug.Log($"{value}");
#endif

    }



}

