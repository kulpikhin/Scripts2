using System.Collections.Generic;

public class RefreshStrategy : IEffectStrategy
{
    public void Apply(EffectContainer container, EffectInstance newInstance)
    {
        var type = newInstance.Type;
        if (!container.Instances.TryGetValue(type, out var list))
        {
            list = new List<EffectInstance> { newInstance };
            container.Instances[type] = list;
            newInstance.OnTick += container.HandleTick;
            newInstance.OnExpired += container.OnExpire;
            newInstance.Start();
        }
        else
        {
            var old = list[0];
            if (newInstance.Power >= old.Power)
            {
                old.OnTick -= container.HandleTick;
                old.OnExpired -= container.OnExpire;
                old.Stop();
                list[0] = newInstance;
                newInstance.OnTick += container.HandleTick;
                newInstance.OnExpired += container.OnExpire;
                newInstance.Start();
            }
        }
    }

    public void HandleExpiration(EffectContainer container, EffectInstance expiredInstance)
    {
        var type = expiredInstance.Type;
        if (!container.Instances.TryGetValue(type, out var list)) return;
        expiredInstance.OnTick -= container.HandleTick;
        expiredInstance.OnExpired -= container.OnExpire;
        list.Remove(expiredInstance);
        if (list.Count == 0) container.Instances.Remove(type);
    }
}
