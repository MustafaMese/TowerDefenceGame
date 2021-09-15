using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileContent : MonoBehaviour
{
    [SerializeField] TileContentType type;
    [SerializeField] GameObject editorObject;
    [SerializeField] GameObject sceneObject;

    public TileContentType Type { get => type; }

    private void Awake()
    {
        if (editorObject != null)
            editorObject.SetActive(false);
        if (sceneObject != null)
            sceneObject.SetActive(true);
    }
}
