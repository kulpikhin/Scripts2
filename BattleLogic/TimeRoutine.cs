using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeRoutine : MonoBehaviour
{
    private Coroutine _timeCoroutine;
    private WaitForSeconds _waitSecond;
    private bool _IsActive;

    private void OnEnable()
    {
        _waitSecond = new WaitForSeconds(1);
        _IsActive = true;
        StartTimer();
    }

    private void StartTimer()
    {
        if (_timeCoroutine != null)
        {
            StopCoroutine(_timeCoroutine);
        }

        _timeCoroutine = StartCoroutine(SpentSecondCoroutine());
    }

    private IEnumerator SpentSecondCoroutine()
    {
        while (_IsActive)
        {
            yield return _waitSecond;

            TimeEvent.Spent();
        }
    }
}
