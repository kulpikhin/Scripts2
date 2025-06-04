using System.Collections.Generic;
using System.Linq;

public class StrongestStrategy : IEffectStrategy
{
    public void Apply(EffectContainer container, EffectInstance newInstance)
    {
        var type = newInstance.Type;

        if (!container.Instances.TryGetValue(type, out var list))
        {
            list = new List<EffectInstance>();
            container.Instances[type] = list;
        }

        var duplicate = list.FirstOrDefault(e => e.Power == newInstance.Power);

        if (duplicate != null)
        {
            duplicate.OnExpired -= container.OnExpire;
            list.Remove(duplicate);
        }

        list.Add(newInstance);

        newInstance.OnExpired += container.OnExpire;
        newInstance.OnAply += container.OnAply;

        foreach (var inst in list) inst.IsStrongest = false;
        list.OrderByDescending(e => e.Power).First().IsStrongest = true;
    }

    public void HandleExpiration(EffectContainer container, EffectInstance expiredInstance)
    {
        var type = expiredInstance.Type;
        if (!container.Instances.TryGetValue(type, out var list)) return;

        expiredInstance.OnExpired -= container.OnExpire;
        expiredInstance.OnAply -= container.OnAply;

        list.Remove(expiredInstance);

        if (list.Count == 0)
        {
            if (container.TypeCoroutines.TryGetValue(type, out var coro))
            {
                container.StopCoroutine(coro);
                container.TypeCoroutines.Remove(type);
            }

            container.Instances.Remove(type);
        }
        else
        {
            foreach (var inst in list) inst.IsStrongest = false;
            list.OrderByDescending(e => e.Power).First().IsStrongest = true;
        }
    }
}