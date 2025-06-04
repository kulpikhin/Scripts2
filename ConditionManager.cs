using UnityEngine;

[RequireComponent(typeof(HealthManager))]
public class ConditionManager : MonoBehaviour
{
    private HealthManager healthManager;

    public bool IsDead { get; private set; }

    private void Start()
    {
        healthManager = GetComponent<HealthManager>();
        IsDead = false;
    }

    private void OnEnable()
    {
        healthManager.CharacterDied += Die;
    }

    private void OnDisable()
    {
        healthManager.CharacterDied -= Die;
    }

    private void Die()
    {
        IsDead = true;
    }
}
