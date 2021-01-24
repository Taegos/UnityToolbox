using System.Collections;
using System.Collections.Generic;
using Toolbox.EventSystem;
using Toolbox.EventSystem.Events;
using Toolbox.Tilebox.Tilemap;
using UnityEngine;

public class TileCharacterController : MonoBehaviour
{
    [SerializeField] private VoidEvent onWorldUpdate;
//    private Tilemap tilemap;
    private TileTraverser traverser;

    private void Start() {
  //      tilemap = FindObjectOfType<Tilemap>();
        traverser = GetComponent<TileTraverser>();
        traverser.OnTraversing += onWorldUpdate.Raise;
    }
}