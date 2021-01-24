using Toolbox.Tilebox.Tilemap;
using UnityEngine;

public class TilePosition : MonoBehaviour
{
    private Vector2Int pos;
//    private Tilemap tilemap;

    void Start()
    {
  //      tilemap = FindObjectOfType<Tilemap>();
    }

    public void Init(Vector2Int startPos) {
        pos = startPos;
    }

    public Vector2Int Get() {
        return pos;
    }

    public void Set(Vector2Int next) {
    //    if (tilemap.Swap(gameObject, pos, next)) {
        //    pos = next;
      //  }
    }
}