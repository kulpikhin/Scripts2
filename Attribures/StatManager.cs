using UnityEngine;

[RequireComponent(typeof(HealthManager), typeof(ManaManager), typeof(SpeedManager))]
[RequireComponent(typeof(ArmorManager))]
public class StatManager : MonoBehaviour
{
    public ArmorManager Armor { get; private set; }
    public HealthManager Health { get; private set; }
    public ManaManager Mana { get; private set; }
    public SpeedManager Speed { get; private set; }

    private void OnEnable()
    {
        Health = GetComponent<HealthManager>();
        Mana = GetComponent<ManaManager>();
        Speed = GetComponent<SpeedManager>();
        Armor = GetComponent<ArmorManager>();
    }
}

