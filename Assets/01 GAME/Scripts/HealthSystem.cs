using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public event EventHandler OnDamaged;
    public event EventHandler OnDied;
    [SerializeField] int healthAmountMax = 100;
    int _healthAmount;

    void Awake()
    {
        _healthAmount = healthAmountMax;
    }

    public void Damage(int damageAmount)
    {
        _healthAmount -= damageAmount;
        _healthAmount = Mathf.Clamp(_healthAmount, 0, healthAmountMax);

        OnDamaged?.Invoke(this, EventArgs.Empty);
        if (IsDead())
        {
            OnDied?.Invoke(this, EventArgs.Empty);
        }
    }

    public bool IsDead() => _healthAmount == 0;

    public int GetHealthAmount() => _healthAmount;

    public bool IsFullHealth() => _healthAmount == healthAmountMax;

    public float GetHealthAmountNormalized() => (float) _healthAmount / healthAmountMax;

    public void SetHealthAmountMax(int healthAmountMax, bool updateHealthAmount)
    {
        _healthAmount = healthAmountMax;
        if (updateHealthAmount)
        {
            this.healthAmountMax = _healthAmount;
        }
    }
}