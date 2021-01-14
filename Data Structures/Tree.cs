using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Toolbox.General.DataStructures
{
    [Serializable]
    public struct FlattenedTreeNode<T>
    {
        public T Data { get; set; }
        public int Depth { get; set; }

        public override string ToString()
        {
            return Data.ToString() + ": " + Depth;
        }
    }

    [Serializable]
    public class FlattenedTree<T>
    {
        private List<FlattenedTreeNode<T>> items;

        public FlattenedTree(List<FlattenedTreeNode<T>> items)
        {
            this.items = items;
        }

        public Tree<T> Unflatten ()
        {
            Dictionary<int, Tree<T>> tmp = new Dictionary<int, Tree<T>>();
            foreach (var node in items)
            {
                var next = new Tree<T>(node.Data);
                if (tmp.ContainsKey(node.Depth - 1))
                    tmp[node.Depth - 1].Children.Add(next);
                tmp[node.Depth] = next;
            }
            return tmp[0];
        }
    }

    [Serializable]
    public class Tree<T>
    {
        public T Data { get; set; }
        public List<Tree<T>> Children { get; }

        public Tree(T data, params Tree<T>[] children)
        {
            Data = data;
            Children = children.ToList();
        }

        public override string ToString()
        {
            string res = Data.ToString() + ": ";

            for (int i = 0; i < Children.Count; i++)
            {
                if (i > 0)
                    res += ", ";
                var child = Children[i];
                res += child.Data.ToString();
            }
            res += ". ";

            foreach (var node in Children)
                res += node.ToString();

            return res;
        }

        public FlattenedTree<T> Flatten()
        {
            var items = new List<FlattenedTreeNode<T>>();
            Flatten(this, items, 0);
            return new FlattenedTree<T>(items);
        }

        private void Flatten(Tree<T> current, List<FlattenedTreeNode<T>> result, int depth)
        {
            result.Add(new FlattenedTreeNode<T>
            {
                Data = current.Data,
                Depth = depth
            });

            foreach (var child in current.Children)
                Flatten(child, result, depth + 1);
        }
    }

    [Serializable]
    public class SerializableTree<T> : ISerializationCallbackReceiver
    {
        private FlattenedTree<T> flattened;
        public Tree<T> Root { get; private set; }

        public SerializableTree(Tree<T> root)
        {
            Root = root;
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            Root = flattened.Unflatten();
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            flattened = Root.Flatten();
        }
    }
}