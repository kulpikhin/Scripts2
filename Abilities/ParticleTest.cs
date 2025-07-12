using UnityEngine;

public class ParticleTest : MonoBehaviour
{
    public GameObject abilityEffectPrefab;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(abilityEffectPrefab, transform.position, Quaternion.identity);
        }
    }
}
