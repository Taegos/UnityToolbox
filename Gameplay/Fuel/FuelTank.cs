using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelTank : MonoBehaviour
{
    [SerializeField] private float tankSize = 10.0f;
    [SerializeField] private float consumptionSpeed = 0.1f;
    [SerializeField] private float generationSpeed = 0.05f;
    [SerializeField] private float fuelCanRestoration = 0.3f;

    public delegate void OnTankEmptyDelegate();
    public event OnTankEmptyDelegate OnTankEmpty;
    public delegate void OnFuelUpdateDelegate(float value);
    public event OnFuelUpdateDelegate OnFuelUpdate;

    public Func<bool> ConsumeIf = () => true;
    public Func<bool> GenerateIf = () => false;

    private Ticker ticker;
    public bool IsEmpty => ticker.IsZero;

    void Start() {
        ticker = TickerSystem.Instance.Create(tankSize);
        ticker.TickUpSpeed = generationSpeed;
        ticker.TickDownSpeed = consumptionSpeed;
        ticker.OnTick += (float value) => OnFuelUpdate?.Invoke(value); 
        ticker.OnZero += () => OnTankEmpty?.Invoke();
        ticker.TickDownIf = ConsumeIf;
        ticker.TickUpIf = GenerateIf;
    }

    public void Add(float value) {
        ticker.Add(value);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.EnumTag() != Tag.FuelCan) return;
        ticker.Add(fuelCanRestoration);
        Destroy(other.gameObject);
    }
}