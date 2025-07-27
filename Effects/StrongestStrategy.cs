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

        // ������� ������� ����� �������
        var currentStrongest = list.OrderByDescending(e => e.Power).FirstOrDefault();

        if (currentStrongest != null)
        {
            if (newInstance.Power > currentStrongest.Power)
            {
                // ������� ������ ������
                container.OnExpire(currentStrongest);

                currentStrongest.OnExpired -= container.OnExpire;
                currentStrongest.OnAply -= container.OnAply;
                list.Remove(currentStrongest);
            }
            else
            {
                // ����� ������ ��� ����� � ����������
                return;
            }
        }

        // ��������� � �����������
        list.Add(newInstance);

        newInstance.OnExpired += container.OnExpire;
        newInstance.OnAply += container.OnAply;

        newInstance.IsStrongest = true;

        // �����: ������ ������ ��������� ������� �������
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