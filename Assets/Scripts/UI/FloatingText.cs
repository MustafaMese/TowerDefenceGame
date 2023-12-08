using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    [SerializeField] private TMP_Text textMesh;

    public void Activate(char[] arr, float duration, Vector3 startPos, Vector3 endPos, Action<FloatingText> OnEnd)
    {
        transform.position = startPos;
        textMesh.SetCharArray(arr);

        transform.DOMove(endPos, duration)
            .OnComplete(() => OnEnd.Invoke(this));
    }
}