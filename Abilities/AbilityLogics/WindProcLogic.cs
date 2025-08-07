using UnityEngine;


[CreateAssetMenu(menuName = "Logic/WindProcLogic")]
public class WindProcLogic : AbilityLogic
{
    protected override void InstantiateVFX(IDamageable target, ParticleSystem VFXPrefab)
    {
        Debug.Log("windVFX");

        Vector3 place = target._transform.position;

        ParticleSystem vfx = Instantiate(VFXPrefab, place - new Vector3(0, 2, 0), Quaternion.Euler(-100, 0, 0));
        vfx.Play();
    }
}
