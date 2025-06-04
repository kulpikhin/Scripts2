public interface IEffectStrategy
{
    void Apply(EffectContainer container, EffectInstance newInstance);

    void HandleExpiration(EffectContainer container, EffectInstance expiredInstance);
}

