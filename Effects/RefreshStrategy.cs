using System.Collections.Generic;

public class RefreshStrategy : IEffectStrategy
{
    public void Apply(EffectContainer container, EffectInstance newInstance)
    {
        var type = newInstance.Type;
        if (!container.Instances.TryGetValue(type, out var list))
        {
            list = new List<EffectInstance>();
            container.Instances[type] = list;
        }
        else if (list.Count > 0)
        {
            var old = list[0];
            if (newInstance.Power >= old.Power)
            {
                // Expire the old instance so its OnExpired logic executes
                old.Stop(container._owner);
                list.Clear();
            }
            else
            {
                return;
            }
        }

        list.Add(newInstance);
        newInstance.OnTick += container.HandleTick;
        newInstance.OnExpired += container.OnExpire;
        newInstance.OnAply += container.OnAply;
        newInstance.Start();
    }

    public void HandleExpiration(EffectContainer container, EffectInstance expiredInstance)
    {
        var type = expiredInstance.Type;
        if (!container.Instances.TryGetValue(type, out var list)) return;
        expiredInstance.OnTick -= container.HandleTick;
        expiredInstance.OnExpired -= container.OnExpire;
        expiredInstance.OnAply -= container.OnAply;
        list.Remove(expiredInstance);
        if (list.Count == 0) container.Instances.Remove(type);
    }
}


