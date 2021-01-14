using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ID
{
    Undefined,
    Ground,
    DeathTrigger,
    FuelCan,
}

public class IDTag : MonoBehaviour
{
    [SerializeField] private ID id;
    public ID Id => id;
}