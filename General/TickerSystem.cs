using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ticker
{
    public float TickUpSpeed { get; set; }
    public float TickDownSpeed { get; set; }
    public bool IsZero => value <= 0.0f;

    public delegate void OnZeroDelegate();
    public event OnZeroDelegate OnZero;
    public delegate void OnTickDelegate(float progress);
    public event OnTickDelegate OnTick;

    public Func<bool> TickDownIf;
    public Func<bool> TickUpIf;

    private float value;
    private float maxValue;
    
    public Ticker(float maxValue) {
        this.maxValue = maxValue;
        value = maxValue;
        TickUpSpeed = 0.01f;
        TickDownSpeed = 0.02f;
    }

    public void Update() {
        if (TickDownIf != null && TickDownIf() && !IsZero) {
            value -= Time.deltaTime * TickDownSpeed;
            if (value <= 0.0f) {
                value = 0.0f;
                if (OnZero != null) {
                    OnZero.Invoke();
                }
            }
        }
        if (TickUpIf != null && TickUpIf() && value <= maxValue && !IsZero) {
            value += Time.deltaTime * TickUpSpeed;
        }
        if (OnTick != null) {
            OnTick.Invoke(value / maxValue);
        }
    }
    public void Add(float addition) {
        value = Mathf.Clamp(value + addition, 0.0f, maxValue);
        OnTick.Invoke(value / maxValue);
    }
}


public class TickerSystem : Singleton<TickerSystem>
{
    private List<Ticker> tickers = new List<Ticker>();

    public Ticker Create(float maxValue) {
        Ticker ticker = new Ticker(maxValue);
        tickers.Add(ticker);
        return ticker;
    }

    void Update() {
        foreach (Ticker ticker in tickers) {
            ticker.Update();
        }
    }
}
