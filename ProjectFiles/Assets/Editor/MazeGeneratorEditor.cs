#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MazeGenerator))]
public class MazeGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        MazeGenerator mazeGenerator = (MazeGenerator)target;

        if (GUILayout.Button("Generate Maze Instant"))
        {
            mazeGenerator.GenerateMazeInstant(mazeGenerator.mazeSize);
        }
    }
}
#endif