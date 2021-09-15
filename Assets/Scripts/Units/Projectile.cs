using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float speed;
    private bool canFire;
    private Transform target;
    private ProjectileObjectPoolHandler handler;

    public void Fire(Transform target, ProjectileObjectPoolHandler handler)
    {
        canFire = true;
        this.target = target;
        this.handler = handler;
        speed = handler.ProjectileSpeed;
    }

    private void Update()
    {
        if (canFire)
        {
            if (target == null)
            {
                Stop();
                return;
            }

            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, target.position) < 0.1f)
                Stop();

        }

    }

    private void Stop()
    {
        canFire = false;
        handler.Push(this);
    }
}
