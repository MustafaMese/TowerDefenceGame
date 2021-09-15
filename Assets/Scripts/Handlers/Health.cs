using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] int healthPoint;
    [SerializeField] UnitType unitType;

    public void TakeDamage(int damage)
    {
        healthPoint = Mathf.Max(healthPoint - damage, 0);
        if (healthPoint == 0)
            Death();
    }

    private void Death()
    {
        if (unitType == UnitType.ENEMY)
        {
            UIManager.Instance.IncreaseScore();
            transform.DOKill();
            Destroy(gameObject);
        }
        else if (unitType == UnitType.BASE)
        {
            UIManager.Instance.GameOver();
        }

    }
}
