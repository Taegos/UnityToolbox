using System;
using System.Collections;
using System.Collections.Generic;
using Toolbox.General.DataStructures;
using UnityEditor;
using UnityEngine;

namespace Toolbox.EditorUI
{
    [Serializable]
    public class FoldoutItem
    {
        public bool Open { get; set; }
        public string Label { get; set; }

        public FoldoutItem(string label = "")
        {
            Label = label;
        }
    }


    [Serializable]
    public class TreeViewItem : Tree<FoldoutItem>
    {
        public TreeViewItem(string data, params Tree<FoldoutItem>[] children) : base(new FoldoutItem(data), children)
        {
        }
    }


    [Serializable]
    public class TreeViewState : SerializableTree<FoldoutItem>
    {
        public TreeViewState() : base(new Tree<FoldoutItem>(new FoldoutItem())) {}
    }

    
    [Serializable]
    public class TreeView
    {
        private TreeViewState state;
        private bool hideRoot;

        public TreeView(TreeViewState state, bool hideRoot = false)
        {
            this.state = state;
            this.hideRoot = hideRoot;
        }

        public void Sync(Tree<string> data)
        {
            Sync(data, state.Root);
        }

        private void Sync(Tree<string> data, Tree<FoldoutItem> current)
        {
            current.Data.Label = data.Data;

            //

            int diff = current.Children.Count - data.Children.Count;
            if (diff > 0)
                current.Children.RemoveRange(data.Children.Count, diff);

            for (int i = 0; i < data.Children.Count; i++)
            {
                if (i >= current.Children.Count)
                    current.Children.Add(new Tree<FoldoutItem>(new FoldoutItem()));
                Sync(data.Children[i], current.Children[i]);
            }
        }

        public void OnGUI()
        {
            if (hideRoot)
                foreach (var child in state.Root.Children)
                    OnGUI(child);
            else
                OnGUI(state.Root);
        }

        private void OnGUI(Tree<FoldoutItem> current)
        {
            var item = current.Data;
            if (current.Children.Count == 0)
            {
                EditorGUILayout.LabelField(item.Label);
                return;
            }
            item.Open = EditorGUILayout.Foldout(item.Open, item.Label);
            if (!item.Open)
                return;
            EditorGUI.indentLevel++;
            foreach (var child in current.Children)
                OnGUI(child);
            EditorGUI.indentLevel--;
        }
    }
}