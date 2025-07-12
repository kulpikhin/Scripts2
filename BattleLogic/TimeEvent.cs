using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TimeEvent
{
    public static event Action SpentSecond;

    public static void Spent()
    {
        SpentSecond?.Invoke();
    }
}
