using System.Collections.Generic;

public class StackStrategy : IEffectStrategy
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
            var baseInst = list[0];
            baseInst.Stop();
            baseInst.Power += newInstance.Power;

            if (baseInst.Duration < newInstance.Duration)
                baseInst.Duration = newInstance.Duration;

            baseInst.Init(container);
            baseInst.Start();
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