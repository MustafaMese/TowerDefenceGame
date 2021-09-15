using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileObjectPoolHandler : MonoBehaviour
{
    [SerializeField] Projectile projectilePrefab;
    [SerializeField] float projectileSpeed;
    private ObjectPool<Projectile> projectiles;

    public float ProjectileSpeed { get => projectileSpeed; }

    private void Awake()
    {
        projectiles = new ObjectPool<Projectile>();
        projectiles.SetObject(projectilePrefab);
        projectiles.Fill(50, transform);
    }

    public Projectile Pop()
    {
        var pr = projectiles.Pop();
        pr.transform.SetParent(null);
        return pr;
    }

    public void Push(Projectile projectile)
    {
        projectile.transform.SetParent(transform);
        projectiles.Push(projectile);
    }
}
