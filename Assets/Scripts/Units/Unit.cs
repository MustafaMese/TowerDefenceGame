using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] UnitType unitType;
    [SerializeField] Mover mover;
    [SerializeField] Health health;

    public Action<Tile> OnMove;

    private float movementDuration;

    public float MovementDuration { set => movementDuration = value; }

    private void Start()
    {
        OnMove += Move;
    }

    public void Move(Tile tile)
    {
        mover.Move(tile, movementDuration);
    }

    public void TakeDamage(int damage)
    {
        health.TakeDamage(damage);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Projectile")
            TakeDamage(10);
    }
}
