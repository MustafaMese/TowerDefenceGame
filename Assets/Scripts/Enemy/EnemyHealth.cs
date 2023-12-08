using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private Enemy enemy;
    [SerializeField] private GameObject healthBarParent;
    [SerializeField] private Transform healthBarPivot;
    
    private int _maxHealth;
    private int _health;

    public int CurrentHealth => _health;
    public int MaxHealth => _maxHealth;
    
    public void Initialize(EnemyProperties enemyProperties, float healthPercentage)
    {
        _maxHealth = (int)(enemyProperties.maxHealth + enemyProperties.maxHealth * healthPercentage / 100);
        _health = _maxHealth;
        SetHealthBar(false);
        
        enemy.OnHealthChanged += OnHealthChange;
    }
    
    public void Initialize(int serializedEnemyMaxHealth, int serializedEnemyHealth)
    {
        _maxHealth = serializedEnemyMaxHealth;
        _health = serializedEnemyHealth;
        SetHealthBar(serializedEnemyMaxHealth > serializedEnemyHealth);
        
        enemy.OnHealthChanged += OnHealthChange;
    }

    private void OnHealthChange(int delta)
    {
        _health += delta;
        SetHealthBar(true);
        
        if (_health <= 0)
            enemy.OnDeath?.Invoke();
    }

    private void SetHealthBar(bool open)
    {
        if(open)
            healthBarParent.SetActive(true);
        
        float x = (float)_health / _maxHealth;
        Vector3 scale = healthBarPivot.localScale;
        scale.x = x;
        healthBarPivot.localScale = scale;
    }
}
