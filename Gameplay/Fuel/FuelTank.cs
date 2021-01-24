using System;
using Toolbox.EventSystem.Events;
using Toolbox.General;
using UnityEngine;
using UnityEngine.Events;

public class FuelTank : MonoBehaviour
{
    [SerializeField] private float tankSize = 10.0f;
    [SerializeField] private float consumptionSpeed = 0.1f;
    [SerializeField] private float generationSpeed = 0.05f;
    [SerializeField] private float fuelCanRestoration = 0.3f;

    public VoidEvent OnTankEmpty;
    public FloatEvent OnTankUpdate;

    private Ticker ticker;
    public bool IsEmpty => ticker.IsZeroOrBelow;

    void Start() {
        ticker = new Ticker(tankSize);
        ticker.OnTick += (float value) => OnTankUpdate?.Raise(value); 
        ticker.OnZeroOrBelow += () => OnTankEmpty?.Raise();
    }

    public void Add(float value) 
    {
        ticker.Add(value);
    }
    
    public void Generate()
    {
        ticker.TickUp(generationSpeed);
    }

    public void Consume()
    {
        ticker.TickDown(consumptionSpeed);
    }

    private void OnTriggerEnter(Collider other) {
        ticker.Add(fuelCanRestoration); 
        Destroy(other.gameObject);
    }
}