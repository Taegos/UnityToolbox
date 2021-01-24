using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toolbox.Algorithms
{
    public static class AStar 
    {
        private class Node
        {
            public Vector2Int Pos;

            public float GCost; //cost to start following a path
            public float HCost; //cost to target
            public float FCost => GCost + HCost;
            public Node Parent;
        }

        private static Stack<Vector2Int> TraceRoute(Node target) {
            Stack<Vector2Int> route = new Stack<Vector2Int>();
            Node next = target;
            while (next.Parent != null) { 
                route.Push(next.Pos);
                next = next.Parent;
            }

            return route;
        }

        //Make option for euclidian distance
        private static List<Vector2Int> GetNeighbours(Vector2Int origin) {
            return new List<Vector2Int>() {
                origin + Vector2Int.up,
                origin + Vector2Int.right,
                origin + Vector2Int.down,
                origin + Vector2Int.left
            };
        }

        private static List<Vector2Int> FindAlternateDest(Vector2Int current, Func<Vector2Int, bool> isTraversable, HashSet<Vector2Int> visited) {

            visited.Add(current);
            List<Vector2Int> traversables = new List<Vector2Int>();
            List<Vector2Int> neighbours = GetNeighbours(current);
            foreach (Vector2Int neighbour in neighbours) {
                if (isTraversable(neighbour)) {
                    traversables.Add(neighbour);
                }
            }

            if (traversables.Count > 0) {
                return traversables;
            }

            foreach (Vector2Int neighbour in neighbours) {
                if (visited.Contains(neighbour)) continue;
                return FindAlternateDest(neighbour, isTraversable, visited);
            }

            return new List<Vector2Int>();
        }

        public static Stack<Vector2Int> CalculateRoute(Vector2Int originPos, Vector2Int destPos, Func<Vector2Int, bool> isTraversable, Func<Vector2Int, Vector2Int, float> getMoveCostBetween, bool debug = false) { 
     
            if (isTraversable(destPos)) {
                return CalculateRoute_(originPos, destPos, isTraversable, getMoveCostBetween, debug);
            }

            Stack<Vector2Int> bestAlternate = null;
            foreach (Vector2Int alternateDest in FindAlternateDest(destPos, isTraversable, new HashSet<Vector2Int>())) {
                Stack<Vector2Int> nextAlternate = CalculateRoute_(originPos, alternateDest, isTraversable, getMoveCostBetween, debug);
                if (bestAlternate == null || nextAlternate.Count < bestAlternate.Count) {
                    bestAlternate = nextAlternate;
                }
            }

            return bestAlternate;
        }

        private static Stack<Vector2Int> CalculateRoute_(Vector2Int originPos, Vector2Int destPos, Func<Vector2Int, bool> isTraversable, Func<Vector2Int, Vector2Int, float> getMoveCostBetween, bool debug = false) {

            Node origin = new Node { Pos = originPos };
            origin.HCost = getMoveCostBetween(origin.Pos, destPos);
            List<Node> open = new List<Node>();
            Dictionary<Vector2Int, Node> visited = new Dictionary<Vector2Int, Node>();
            if (debug) {
                Debug.DrawLine((Vector2)originPos, (Vector2)destPos, Color.green, 2);
            }

            open.Add(origin);
            visited.Add(origin.Pos, origin);

            while (open.Count > 0) {

                //Change to priority queue or take vowel never touch a computer again
                int bestNodeIndex = 0;
                for (int i = 1; i < open.Count; i++) {
                    if (open[i].FCost < open[bestNodeIndex].FCost) {
                        bestNodeIndex = i;
                    }
                }

                Node current = open[bestNodeIndex];
                open.RemoveAt(bestNodeIndex);

                if (current.Pos == destPos) {
                    return TraceRoute(current);
                }

                foreach (Vector2Int neighbourPos in GetNeighbours(current.Pos)) {

                    if (!isTraversable(neighbourPos)) continue;

                    Node neighbour = new Node { Pos = neighbourPos };
                    neighbour.HCost = getMoveCostBetween(neighbour.Pos, destPos);
                    neighbour.GCost = current.GCost + getMoveCostBetween(neighbour.Pos, current.Pos);
                    Node existing;

                    if (!visited.TryGetValue(neighbour.Pos, out existing) || 
                        neighbour.FCost < existing.FCost) {
                        visited[neighbour.Pos] = neighbour;
                        neighbour.Parent = current;
                        open.Add(neighbour);
                        if (debug) {
                            Debug.DrawLine((Vector2)current.Pos, (Vector2)neighbour.Pos, Color.red, 3);
                        }
                    }
                }
            }

            return new Stack<Vector2Int>();
        }
    }
}