using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ticker
{
    public bool IsZeroOrBelow => value <= 0.0f;

    public delegate void OnZeroOrBelowDelegate();
    public event OnZeroOrBelowDelegate OnZeroOrBelow;
    public delegate void OnTickDelegate(float progress);
    public event OnTickDelegate OnTick;

    private float value;
    private float maxValue;
    
    public Ticker(float maxValue) {
        this.maxValue = maxValue;
        value = maxValue;
    }

    public void TickUp(float speed)
    {
        if (value >= maxValue && !IsZeroOrBelow) return;
        value += Time.deltaTime * speed;
        OnTick?.Invoke(value / maxValue);
    }

    public void TickDown(float speed)
    {
        if (IsZeroOrBelow) return;
        value -= Time.deltaTime * speed;
        OnTick?.Invoke(value / maxValue);
        if (value <= 0.0f)
        {
            value = 0.0f;
            OnZeroOrBelow?.Invoke();
        }
    }
    public void Add(float addition) {
        value = Mathf.Clamp(value + addition, 0.0f, maxValue);
        OnTick.Invoke(value / maxValue);
    }
}