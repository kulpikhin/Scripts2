using System;
using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Projectile : MonoBehaviour
{
    public float travelDuration = 1f;

    public event Action<IDamageable, Projectile> EndFlying;

    public IDamageable _target;

    private Vector2 _targetPos;
    private Vector3 _startPos;
    private float _elapsed;
    private Coroutine _flyingCoroutine;

    public void Launch(Vector2 startPosition, IDamageable target)
    {
        _target = target;
        _targetPos = target._transform.position;
        _startPos = startPosition;
        _elapsed = 0f;

        StartFly();
    }

    private void StartFly()
    {
        if (_flyingCoroutine != null)
        {
            StopCoroutine(_flyingCoroutine);
        }

        _flyingCoroutine = StartCoroutine(Flying());
    }

    private IEnumerator Flying()
    {
        while(_elapsed < travelDuration)
        {
            _elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(_elapsed / travelDuration);
            transform.position = Vector2.Lerp(_startPos, _targetPos, t);
            yield return null;
        }

        EndFlying?.Invoke(_target,this);

        Destroy(gameObject);
    }
}
