using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Tag
{
    Ground,
    DeathTrigger,
    FuelCan,
}

public class EnumTag : MonoBehaviour
{
    [SerializeField] private Tag tag;
    public Tag GetTag() {
        return tag;
    }
}