using System.Collections;
using System.Collections.Generic;
using Toolbox.EventSystem;
using Toolbox.Tilebox.Tilemap;
using UnityEngine;

[RequireComponent(typeof(TilePosition))]
public class TileTrigger : MonoBehaviour
{
    [SerializeField] private Vector2Int area = Vector2Int.one;
    [SerializeField] private GameObjectUnityEvent onTileEnter;
    [SerializeField] private GameObjectUnityEvent onTileStay;
    [SerializeField] private GameObjectUnityEvent onTileExit;

    public List<Vector2Int> GetArea() {
        Vector2Int origin = GetComponent<TilePosition>().Get();
        List<Vector2Int> result = new List<Vector2Int>();
        for (int x = -area.x/2; x < (area.x/2) + 1; x++) {
            for (int y = -area.y / 2; y < (area.y/2) + 1; y++) {
                result.Add(origin + new Vector2Int(x, y));
            }
        }
        return result;
    }

    public void OnTileEnter(GameObject tile) {
        onTileEnter?.Invoke(tile);
    }

    public void OnTileStay(GameObject tile) {
        onTileStay?.Invoke(tile);
    }

    public void OnTileExit(GameObject tile) {
        onTileExit?.Invoke(tile);
    }

    private void OnDrawGizmos() {
        Color red = Color.red;
        red.a = 0.2f;
        Gizmos.color = red;
        foreach (Vector2Int trigger in GetArea()) {
            Gizmos.DrawCube(new Vector3(trigger.x, trigger.y, 0), Vector2.one);
        }
    }
}