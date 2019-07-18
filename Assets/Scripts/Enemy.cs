using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected float MaxHealth = 100f;
    [SerializeField] protected float HealthRemaining = 100f;
    [SerializeField] protected TextMesh HealthText;

    [SerializeField] private Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public virtual void HealDamage(float damage)
    {
        HealthRemaining = HealthRemaining + damage < MaxHealth ? HealthRemaining + damage : MaxHealth;
        UpdateHealthBar();
    }

    public virtual void TakeDamage(float damage)
    {
        HealthRemaining -= damage;
        UpdateHealthBar();
        if (HealthRemaining <= 0)
        {
            Die();
        }
    }

    protected virtual void UpdateHealthBar()
    {
        HealthText.text = string.Format("{0} / {1}", HealthRemaining, MaxHealth);
    }

    protected virtual void Die()
    {
    }
}

// ui healthbar