using UnityEngine;
using UnityEngine.Events;

public class HealthManager : MonoBehaviour
{
    [SerializeField] int _health;

    public event UnityAction<int> HealthChanged;
    public event UnityAction CharacterDied;

    private bool IsDead;

    public int CurrentHealth { get; private set; }
    public int MaxHealth { get; private set; }

    private void OnEnable()
    {
        MaxHealth = _health;
        CurrentHealth = MaxHealth;
        IsDead = false;
    }

    public void TakeDamage(int damage)
    {
        if (!IsDead && BattleCondition.GameContinues)
        {
            if (damage > 0)
            {
                CurrentHealth = Mathf.Clamp(CurrentHealth - damage, 0, MaxHealth);

                HealthChanged?.Invoke(CurrentHealth);

                if (CurrentHealth <= 0)
                {
                    IsDead = true;
                    CharacterDied?.Invoke();
                }
            }
        }
    }

    public void Heal(int healPower)
    {
        if (!IsDead && BattleCondition.GameContinues)
        {
            if (healPower > 0)
            {
                CurrentHealth = Mathf.Clamp(CurrentHealth + healPower, 0, MaxHealth);

                HealthChanged?.Invoke(CurrentHealth);
            }
        }
    }

    public void SetMaxHealth(int newMaxHealth)
    {
        SetPositiveHealth(newMaxHealth);
    }

    public void AddMaxHealth(int term)
    {
        int newMaxHealth = MaxHealth + term;
        SetPositiveHealth(newMaxHealth);
    }

    private void SetPositiveHealth(int newMaxHealth)
    {
        if (newMaxHealth > 0)
        {
            MaxHealth = newMaxHealth;
        }
        else
        {
            MaxHealth = 1;
        }

        CurrentHealth = MaxHealth;
    }
}
