using System.IO;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CloudMaster))]
public class CloudMasterEditor : Editor
{
    CloudMaster master;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
    }

    void OnEnable()
    {
        master = (CloudMaster)target;
    }
}
