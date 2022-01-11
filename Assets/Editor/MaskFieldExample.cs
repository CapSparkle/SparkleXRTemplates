using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MaskFieldExample : EditorWindow
{
    static int flags = 0;
    static string[] options = new string[] { "CanJump", "CanShoot", "CanSwim" };

    [MenuItem("Examples/Mask Field usage")]
    static void Init()
    {
        MaskFieldExample window = (MaskFieldExample)GetWindow(typeof(MaskFieldExample));
        window.Show();
    }

    void OnGUI()
    {
        flags = EditorGUILayout.MaskField("Player Flags", flags, options);
    }
}
