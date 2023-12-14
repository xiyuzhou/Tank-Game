#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MazeEdit))]
public class MazeEditEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        MazeEdit mazeEditor = (MazeEdit)target;

        if (GUILayout.Button("CheckForWalls"))
        {
            mazeEditor.CheckForSameWalls();
        }
        if (GUILayout.Button("AddTag"))
        {
            mazeEditor.AddTag();
        }
        if (GUILayout.Button("AddWallComponent"))
        {
            mazeEditor.AddWallComponent();
        }
        if (GUILayout.Button("DestoryNotActive"))
        {
            mazeEditor.DestoryNotActive();
        }
    }
}
#endif