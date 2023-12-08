using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVisual : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField, Range(1, 24)] private int animationFrequencyFrame = 5;

    private Sprite[] _animationSprites;
    
    public void Initialize(EnemyProperties properties)
    {
        Terminate = false;

        _animationSprites = new Sprite[properties.animationSprites.Length];
        Array.Copy(properties.animationSprites, _animationSprites, properties.animationSprites.Length);
        
        StartCoroutine(Animate());
    }

    private IEnumerator Animate()
    {
        var value = (float)1 / animationFrequencyFrame;
        WaitForSeconds seconds = new WaitForSeconds(value);

        int currentState = 0;
        while (true)
        {
            spriteRenderer.sprite = _animationSprites[currentState];
            currentState = 1;
            yield return seconds;
            
            spriteRenderer.sprite = _animationSprites[currentState];
            currentState = 0;
            yield return seconds;
        }
    }

    private bool Terminate { get; set; }

    public void Stop()
    {
        Terminate = true;
    }
}
