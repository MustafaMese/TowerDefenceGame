using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerVisual : MonoBehaviour
{
     [SerializeField] private SpriteRenderer body;
     [SerializeField] private SpriteRenderer head;

     public void Initialize(int sortingValue)
     {
          body.sortingOrder = sortingValue;
          head.sortingOrder = sortingValue + 1;
     }
}
