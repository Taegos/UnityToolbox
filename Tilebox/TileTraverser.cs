using System.Collections;
using System.Collections.Generic;
using Toolbox.Algorithms;
using Toolbox.Tilebox.Tilemap;
using UnityEngine;

[RequireComponent(typeof(TilePosition))]
public class TileTraverser : MonoBehaviour
{
    public enum FreedomOfMovement
    {
        Manhattan,
        Euclidian
    }

    [SerializeField] private float speed;
    public FreedomOfMovement freedomOfMovement;

    private TilePosition position;
//    private Tilemap tilemap;
    private Stack<Vector2Int> route = new Stack<Vector2Int>();

    public delegate void OnTraversingDelegate();
    public event OnTraversingDelegate OnTraversing;

    private void Start() {
        position = GetComponent<TilePosition>();
//        tilemap = FindObjectOfType<Tilemap>();
    }

    private float GetMoveCostBetween(Vector2Int origin, Vector2Int dest) {
        return Vector2.Distance(origin, dest);
    }

    private bool IsTraversable(Vector2Int pos) {
//        if (tilemap.Exists(gameObject, pos)) return true;
//        return tilemap.IsTraverseable(pos);

return false;
    }

    public void GoTo(Vector2Int dest) {
        route = AStar.CalculateRoute(position.Get(), dest, IsTraversable, GetMoveCostBetween);
        if (route.Count > 0) {
            position.Set(route.Peek());
        }
    }

    private void Update() {
        if (route.Count == 0) return;
        OnTraversing?.Invoke();
        Vector2Int next = route.Peek();
        Vector2 dir = (next - (Vector2)transform.position).normalized;
        transform.Translate(dir * Time.deltaTime * speed);
        
        if (Vector2.Distance(next, transform.position) <= 0.05f) {
            route.Pop();
            if (route.Count > 0) {
                position.Set(route.Peek());
            }
        }
    }
}