using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(Board))]
public class BoardEditor : Editor
{
    Board board;

    bool show = false;
    private bool showTiles;

    private void OnEnable()
    {
        board = (Board)target;
    }

    private void OnSceneGUI()
    {
        //Check the event type and make sure it's left click.
        if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);

            if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity) || !hit.collider.TryGetComponent<Tile>(out Tile tile)) return;

            board.ToggleDestination(tile);

            Event.current.Use();
        }
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        GUILayout.BeginVertical();

        Vector2Field(serializedObject.FindProperty("boardSize"), " Board Size ");
        ObjectField(serializedObject.FindProperty("tilePrefab"), " Tile Prefab  ", typeof(Tile));
        ObjectField(serializedObject.FindProperty("contentFactory"), " Tile Content Factory ", typeof(TileContentFactory));

        GUILayout.BeginHorizontal();

        show = GUILayout.Button(" Instatiate ", GUILayout.Height(20), GUILayout.Width(80));
        if (show)
            InstantiateTiles();

        show = GUILayout.Button(" Delete ", GUILayout.Height(20), GUILayout.Width(80));
        if (show)
            Delete();

        GUILayout.EndHorizontal();

        GUILayout.EndVertical();

        serializedObject.ApplyModifiedProperties();
    }


    private void Vector2Field(SerializedProperty serializedProperty, string v)
    {
        GUILayout.BeginHorizontal();
        serializedProperty.vector2Value = EditorGUILayout.Vector2Field(v, serializedProperty.vector2Value);
        GUILayout.EndHorizontal();
    }

    private void ObjectField(SerializedProperty serializedProperty, string v, Type type)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label(v);
        serializedProperty.objectReferenceValue = EditorGUILayout.ObjectField(serializedProperty.objectReferenceValue, type, true, GUILayout.Width(200));
        GUILayout.EndHorizontal();
    }

    private void Delete()
    {
        DestroyChildren(GetChildren());
        board.tiles.Clear();
    }

    private void InstantiateTiles()
    {
        Vector2Int size = Vector2Int.FloorToInt(board.boardSize);
        Tile tilePrefab = board.tilePrefab;

        if (size == default || tilePrefab == default) return;

        Delete();

        Vector2 offset = new Vector2((size.x - 1) * 0.5f, (size.y - 1) * 0.5f);
        for (int i = 0, y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++, i++)
            {
                Tile tile = PrefabUtility.InstantiatePrefab(tilePrefab, board.transform) as Tile;
                tile.transform.localPosition = new Vector3(x - offset.x, 0f, y - offset.y);

                //board.OnTileCreated(tile);
                board.InitializeTile(tile);
                board.AddTile(tile);

                if (x > 0)
                    Tile.AddEastWestNeighbors(tile, board.tiles[i - 1]);
                if (y > 0)
                    Tile.AddNorthSouthNeighbors(tile, board.tiles[i - size.x]);

                tile.IsAlternative = x % 2 == 0;

                if ((y & 1) == 0)
                {
                    tile.IsAlternative = !tile.IsAlternative;
                }
            }
        }
    }


    private void DestroyChildren(Transform[] transforms)
    {
        for (var i = 0; i < transforms.Length; i++)
            DestroyImmediate(transforms[i].gameObject);
    }

    private Transform[] GetChildren()
    {
        var children = new List<Transform>();
        foreach (Transform child in board.transform)
        {
            children.Add(child);
        }

        return children.ToArray();
    }
}
