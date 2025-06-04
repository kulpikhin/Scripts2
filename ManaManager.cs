using System.Threading;
using UnityEngine;
using UnityEngine.Events;

public class ManaManager : MonoBehaviour
{
    [SerializeField] int _mana;

    public event UnityAction<int> ManaChanged;

    public int CurrentMana { get; private set; }
    public int MaxMana { get; private set; }

    private void OnEnable()
    {
        MaxMana = _mana;
        CurrentMana = MaxMana;
    }

    public void SpentMana(int cost)
    {
        if (cost > 0)
        {
            CurrentMana = CurrentMana - cost;
            ManaChanged?.Invoke(CurrentMana);
        }
    }

    public bool EnoughMana(int cost)
    {
        bool isEnough;

        if (CurrentMana <= 0)
        {
            isEnough = false;
            Debug.Log("Negative cost value");

        }
        else if (cost > CurrentMana)
        {
            isEnough = false;
            Debug.Log("Not enough mana");
        }
        else
        {
            isEnough = true;
        }

        return isEnough;
    }

    public void RestoreMana(int restorePower)
    {
        if (restorePower > 0)
        {
            CurrentMana = Mathf.Clamp(CurrentMana + restorePower, 0, MaxMana);

            ManaChanged?.Invoke(CurrentMana);
        }
    }

    public void SetMaxMana(int newMaxMana)
    {
        SetPositiveMana(newMaxMana);
    }

    public void AddMaxMana(int term)
    {
        int newMaxMana = MaxMana + term;
        SetPositiveMana(newMaxMana);
    }

    private void SetPositiveMana(int newMaxMana)
    {
        if (newMaxMana > 0)
        {
            MaxMana = newMaxMana;
        }
        else
        {
            MaxMana = 1;
        }

        CurrentMana = MaxMana;
    }
}
