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

        // Находим текущий самый сильный
        var currentStrongest = list.OrderByDescending(e => e.Power).FirstOrDefault();

        if (currentStrongest != null)
        {
            if (newInstance.Power > currentStrongest.Power)
            {
                // Снимаем старый эффект
                container.OnExpire(currentStrongest);

                currentStrongest.OnExpired -= container.OnExpire;
                currentStrongest.OnAply -= container.OnAply;
                list.Remove(currentStrongest);
            }
            else
            {
                // Новый слабее или равен — игнорируем
                return;
            }
        }

        // Добавляем и подписываем
        list.Add(newInstance);

        newInstance.OnExpired += container.OnExpire;
        newInstance.OnAply += container.OnAply;

        newInstance.IsStrongest = true;

        // ВАЖНО: только сейчас применяем влияние эффекта
        container.OnAply(newInstance);
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
        }
        else
        {
            list.OrderByDescending(e => e.Power).First().IsStrongest = true;
        }
    }
}