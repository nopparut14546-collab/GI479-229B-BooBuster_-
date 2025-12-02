using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100;

    public UnityEvent OnTakeDamage;
    public UnityEvent OnDead;

    public float currentHealth;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damageAmount)
    {
        if (!IsDead())
        {
            OnTakeDamage?.Invoke();
            currentHealth -= damageAmount;

            if (IsDead())
            {
                OnDead?.Invoke();
            }
        }
    }

    public bool IsDead()
    {
        return currentHealth <= 0;
    }

    // Update is called once per frame
    void Update()
    {

    }
}