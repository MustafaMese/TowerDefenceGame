using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Tower : MonoBehaviour
{
    private readonly float minShootRate = 0.5f;
    private readonly float maxShootRate = 0.1f;

    private readonly float minRadius = 2f;
    private readonly float maxRadius = 4f;

    [SerializeField] UnitType unitType;
    [SerializeField] Transform projectileSpawnPoint;
    [SerializeField] float projectileMovementDuration;
    [SerializeField] float radius;
    [SerializeField] LayerMask mask;

    private WaitForSeconds cooldown;
    private Transform target;
    private ProjectileObjectPoolHandler handler;

    public void SetProjectilePoolHandler(ProjectileObjectPoolHandler handler) => this.handler = handler;

    private void Start()
    {
        DOTween.Init();
        StartCoroutine(Shoot());

        radius = UnityEngine.Random.Range(minRadius, maxRadius);
        cooldown = new WaitForSeconds(UnityEngine.Random.Range(minShootRate, maxShootRate));
    }

    // TODO ŞU ANDA COLLİDERLARI EKLEMEN LAIZM.

    private IEnumerator Shoot()
    {
        while (true)
        {
            if (target == null || Vector3.Distance(target.position, transform.position) >= radius)
                target = FindClosestThing(transform.position, mask, radius);

            if (target != null)
            {
                var projectile = handler.Pop();
                projectile.transform.position = projectileSpawnPoint.position;
                projectile.Fire(target, handler);
            }

            yield return cooldown;
        }

    }

    private Transform FindClosestThing(Vector3 currentPosition, int layerMask, float radius)
    {
        Collider closestThingCollider = CheckColliders(currentPosition, radius, layerMask);

        if (closestThingCollider != null)
            return closestThingCollider.transform;
        else
            return null;
    }

    private Collider CheckColliders(Vector3 currentPosition, float radius, int layerMask)
    {
        Collider[] hitColliders;
        int numColliders = GetColliders(currentPosition, radius, layerMask, out hitColliders);

        Collider closestTarget = null;
        var closestDistanceSqr = Mathf.Infinity;

        for (int i = 0; i < numColliders; i++)
        {
            if (hitColliders[i].transform.position == currentPosition) continue;

            var directionToTarget = hitColliders[i].transform.position - currentPosition;
            var dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                closestTarget = hitColliders[i];
            }
        }

        return closestTarget;
    }

    private int GetColliders(Vector3 currentPosition, float radius, int layerMask, out Collider[] hitColliders)
    {
        int maxColliders = 10;
        hitColliders = new Collider[maxColliders];
        return Physics.OverlapSphereNonAlloc(currentPosition, radius, hitColliders, layerMask);
    }

}
