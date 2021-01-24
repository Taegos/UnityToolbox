using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Toolbox.DataStructures;
using System;
using System.Linq;

namespace Toolbox.Tilebox.Tilemap {

//    public class 
    public class Tilemap : MonoBehaviour
    {
        [SerializeField] private bool drawGridGizmo;

        private Dictionary<Vector2Int, List<GameObject>> tiles = new Dictionary<Vector2Int, List<GameObject>>();
        private Dictionary<GameObject, Vector2Int> tile2Pos = new Dictionary<GameObject, Vector2Int>();

        public bool Swap(GameObject tile, Vector2Int next) {
            Vector2Int current;

            if (tile == null || 
                !InsideMap(next) ||
                (tile.GetComponent<TileCollider>() != null && HasCollidable(next)) ||
                !Remove(tile, out current)) { 
                return false;
            }

            tiles[next].Add(tile);
            tile2Pos[tile] = next;

            foreach (TileTrigger trigger in FindObjectsOfType<TileTrigger>()) {
                if (tile == trigger.gameObject) {
                    continue;
                }
                bool exited = trigger.GetArea().Contains(current);
                bool entered = trigger.GetArea().Contains(next);
                if (exited && entered) {
                    trigger.OnTileStay(tile);
                } else if (entered) {
                    trigger.OnTileEnter(tile);
                } else if (exited) {
                    trigger.OnTileExit(tile);
                }
            }

            return true;
        }

        public GameObject Instantiate(GameObject prefabTile, Vector2Int pos) {
            GameObject clone = Instantiate(prefabTile);
            clone.transform.position = (Vector2)pos;
            clone.GetComponent<TilePosition>().Init(pos);

            if (!InsideMap(pos)) {
                tiles[pos] = new List<GameObject>();
            }

            tiles[pos].Add(clone);
            tile2Pos[clone] = pos;

            foreach (TileTrigger trigger in FindObjectsOfType<TileTrigger>()) {
                if (clone == trigger.gameObject) {
                    continue;
                }
                if (trigger.GetArea().Contains(pos)) {
                    trigger.OnTileEnter(prefabTile);
                }
            }

            return clone;
        }

        public bool Destroy(GameObject tile, Vector2Int pos) {
            if (!Remove(tile)) {
                return false;
            }

            foreach (TileTrigger trigger in FindObjectsOfType<TileTrigger>()) {
                if (tile == trigger.gameObject) {
                    continue;
                }
                if (trigger.GetArea().Contains(pos)) {
                    trigger.OnTileEnter(tile);
                }
            }

            GameObject.Destroy(tile);
            return true;
        }

        public bool IsTraverseable(Vector2Int pos) {
            return InsideMap(pos) && !HasCollidable(pos);
        }

        public Vector2Int ScreenToTile(Vector2 screen) {
            return WorldToTile(Camera.main.ScreenToWorldPoint(screen));
        }

        public Vector2Int WorldToTile(Vector2 world) {
            return new Vector2Int(Mathf.RoundToInt(world.x), Mathf.RoundToInt(world.y));
        }

        private bool InsideMap(Vector2Int pos) {
            return tiles.ContainsKey(pos);
        }

        private bool HasCollidable(Vector2Int pos) {
            foreach (GameObject tile in tiles[pos]) {
                if (tile.GetComponent<TileCollider>() != null) {
                    return false;
                }
            }
            return true;
        }

        private bool Remove(GameObject tile, out Vector2Int pos) {
            if (!tile2Pos.TryGetValue(tile, out pos) ||
                !tiles.ContainsKey(pos)) {
                return false;
            }

            return tiles[pos].Remove(tile);
        }

        private bool Remove(GameObject tile) {
            Vector2Int pos;
            return Remove(tile, out pos);
        }


        private void OnDrawGizmos() {
            if (!drawGridGizmo)
                return;

            Color color = Color.grey;
            color.a = 0.3f;
            Gizmos.color = color;

            int size = 10000;
            for (int x = -size / 2; x < size / 2; x++) {
                Vector3 from = new Vector2(x + 0.5f, -size / 2);
                Vector3 to = new Vector2(x + 0.5f, size / 2);
                Gizmos.DrawLine(from, to);
            }
            for (int y = -size / 2; y < size / 2; y++) {
                Vector3 from = new Vector2(-size / 2, y + 0.5f);
                Vector3 to = new Vector2(size / 2, y + 0.5f);
                Gizmos.DrawLine(from, to);
            }

            DrawCollisionMap();
        }

        private void DrawCollisionMap() {
            if (tiles == null) return;
            int size = 200;
            Color color = Color.red;
            color.a = 0.3f;
            Gizmos.color = color;

            for (int x = -size/2; x < size/2; x++) {
                for (int y = -size/2; y < size/2; y++) {
                    Vector2Int pos = new Vector2Int(x, y);
                    if (!IsTraverseable(pos)) {
                        Gizmos.DrawCube((Vector2)pos, Vector2.one);
                    }
                }
            }
        }
    }
}