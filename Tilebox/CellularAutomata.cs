using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toolbox.ProcGen {

    [Serializable]
    public class CellularAutomataArgs
    {
        public int width = 100;
        public int height = 100;
        public float wallChance = 0.5f;
//        Vector2Int 
       // public float edgeCutoff = 0.15f;
        public int passes = 14;
        public int seed = -1;
        public int cullClustersSmallerThan = -1;
    }

    public static class CellularAutomata
    {

        public static bool[,] Generate(CellularAutomataArgs args) {
            bool[,] map = CreateNoiseMap(args.width, args.height, args.wallChance);
            map = ApplyAutomata(map, args.passes);
            map = CullSmallClusters(map, args.cullClustersSmallerThan);
            return map;
        }

        public static bool[,] CreateNoiseMap(int w, int h, float wallChance = 0.5f) 
        {
            int cutoff = 25;
            float radius = w / 2;
            Vector2 mid = new Vector2(radius, radius);
            bool[,] map = new bool[w, h];

            for (int x = 0; x < w; x++) {
                for (int y = 0; y < h; y++) {
                   float distToMid = Vector2.Distance(new Vector2(x, y), mid);
                   if (x < cutoff || x > w - cutoff || y < cutoff || y > h - cutoff) { 
                      //  map[x, y] = false;
                    //    continue;
                   }
//                   float cutoffRoll = 1.0f - Mathf.Clamp(distToMid / radius, 0.0f, 1.0f);
//                   float factor = (1.0f - cutoff) + cutoffRoll * cutoff;
                   float roll = 1.0f - wallChance;

                   map[x, y] = UnityEngine.Random.value < roll ? true : false;
                }
            }

            return map;
        }

        public static bool[,] ApplyAutomata(bool[,] map, int passes = 14) {
            //Random.InitState(seed);         
            for (int i = 0; i < passes; i++) {
                for (int x = 0; x < map.GetLength(0); x++) {
                    for (int y = 0; y < map.GetLength(1); y++) {

                        int neighbours = 0;

                        if (IsWalkable(map, x - 1, y + 1)) neighbours++;
                        if (IsWalkable(map, x, y + 1)) neighbours++;
                        if (IsWalkable(map, x + 1, y + 1)) neighbours++;
                        if (IsWalkable(map, x + 1, y)) neighbours++;
                        if (IsWalkable(map, x + 1, y - 1)) neighbours++;
                        if (IsWalkable(map, x, y - 1)) neighbours++;
                        if (IsWalkable(map, x - 1, y - 1)) neighbours++;
                        if (IsWalkable(map, x - 1, y)) neighbours++;

                        if (neighbours > 4) {
                            map[x, y] = true;
                        }
                        if (neighbours < 4) {
                            map[x, y] = false;
                        }
                    }
                }
            }
            return map;
        }

        public static bool[,] CullSmallClusters(bool[,] map, int minClusterSize) {
            if (minClusterSize <= 0) return map;
            HashSet<Vector2Int> visited = new HashSet<Vector2Int>();

            for (int x = 0; x < map.GetLength(0); x++) {
                for (int y = 0; y < map.GetLength(1); y++) {
                    Vector2Int pos = new Vector2Int(x, y);
                    if (!IsWalkable(map, pos) || visited.Contains(pos)) {
                        continue;
                    }

                    List<Vector2Int> cullArea = FloodFill(pos, map, visited);
                    if (cullArea.Count < minClusterSize) {
                        foreach (Vector2Int cullPos in cullArea) {
                            map[cullPos.x, cullPos.y] = false;
                        }
                    }
                }
            }

            return map;
        }

        private static List<Vector2Int> FloodFill(Vector2Int curr, bool[,] map, HashSet<Vector2Int> visited) {
            List<Vector2Int> result = new List<Vector2Int>();
            Stack<Vector2Int> stack = new Stack<Vector2Int>();
            stack.Push(curr);

            while (stack.Count != 0) {
                Vector2Int next = stack.Pop();
                if (visited.Contains(next) || 
                    !IsWalkable(map, next)) {
                    continue;
                }

                result.Add(next);
                visited.Add(next);
                stack.Push(next + Vector2Int.up);
                stack.Push(next + Vector2Int.right);
                stack.Push(next + Vector2Int.down);
                stack.Push(next + Vector2Int.left);
            }

            return result;
        }

        private static bool Inside(bool[,] map, int x, int y) {
            return 0 < x && x < map.GetLength(0) &&
                0 < y && y < map.GetLength(1);
        }

        private static bool IsWalkable(bool[,] map, int x, int y) {
            return Inside(map, x, y) && map[x, y];
        }

        private static bool IsWalkable(bool[,] map, Vector2Int pos) {
            return IsWalkable(map, pos.x, pos.y);
        }
    }
}