using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Tile))]
public class TileEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        GUILayout.BeginVertical();
        ObjectField(serializedObject.FindProperty("meshRenderer"), " Mesh Renderer ", typeof(MeshRenderer));
        ObjectField(serializedObject.FindProperty("grid"), " Grid Renderer ", typeof(MeshRenderer));
        ObjectField(serializedObject.FindProperty("next"), " Next ", typeof(Tile));
        ObjectField(serializedObject.FindProperty("north"), " North ", typeof(Tile));
        ObjectField(serializedObject.FindProperty("south"), " South ", typeof(Tile));
        ObjectField(serializedObject.FindProperty("east"), " East ", typeof(Tile));
        ObjectField(serializedObject.FindProperty("west"), " West ", typeof(Tile));
        ObjectField(serializedObject.FindProperty("content"), " Content ", typeof(TileContent));
        GUILayout.EndVertical();

        serializedObject.ApplyModifiedProperties();
    }

    private void ObjectField(SerializedProperty serializedProperty, string v, Type type)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label(v);
        serializedProperty.objectReferenceValue = EditorGUILayout.ObjectField(serializedProperty.objectReferenceValue, type, true, GUILayout.Width(200));
        GUILayout.EndHorizontal();
    }
}
