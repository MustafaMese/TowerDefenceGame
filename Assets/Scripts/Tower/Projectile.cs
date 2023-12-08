using System;
using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public IEnumerator Move(float damage, Vector3 point, Enemy enemy, Action<Projectile> OnEnd)
    {
        transform.position = point;
        transform.SetParent(enemy.transform);
        
        float time = 0;
        Vector3 startPosition = transform.localPosition;
        while (time < 0.15f)
        {
            if (enemy == null || !enemy.gameObject.activeInHierarchy || enemy.IsDead)
                break;
            
            transform.localPosition = Vector3.Lerp(startPosition, Vector3.zero, time / 0.15f);
            time += Time.deltaTime;
            yield return null;
        }
        if (enemy != null)
            enemy.TakeDamage(-(int)damage);
        
        transform.SetParent(null);
        OnEnd.Invoke(this);
    }
}
