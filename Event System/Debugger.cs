using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Toolbox.EditorUI;
using Toolbox.DataStructures;
using UnityEditor;
using UnityEngine;

namespace Toolbox.EventSystem
{
    public class Debugger : EditorWindow
    {
        private TreeViewState state;
        private TreeView treeView;

        [MenuItem("Window/Game Event Tracer")]
        public static void ShowWindow()
        {
            var window = GetWindow<Debugger>();
            window.titleContent = new GUIContent("Game Events Tracer");
            window.Show();
            GetWindow(typeof(Debugger));
        }

        private void OnEnable()
        {
            if (treeView == null)
            {
                state = new TreeViewState();
                treeView = new TreeView(state, true);

            }
        }

        void OnGUI()
        {
            Sync();
            treeView.OnGUI();

            if (GUILayout.Button("Rebuild"))
            {
                state = new TreeViewState();
                treeView = new TreeView(state, true);
                Sync();
            }
        }

        private void Sync()
        {
            var next = new Tree<string>("ROOT");

            foreach (var tp in CollectEventInfo())
            {
                var baseEvent = tp.Item1;
                string eventName = baseEvent == null ? "NoEvent" : baseEvent.ToString();
                var eventTree = new Tree<string>(eventName);
                foreach (var listener in tp.Item2)
                {
                    var listenerTree = new Tree<string>(listener.ToString());
                    foreach (var actionInfo in listener.ActionInfos)
                    {
                        listenerTree.Children.Add(new Tree<string>(actionInfo.ToString()));
                    }
                    eventTree.Children.Add(listenerTree);
                }
                next.Children.Add(eventTree);
            }

            treeView.Sync(next);
        }

        private List<Tuple<BaseEvent, List<BaseEventListener>>> CollectEventInfo()
        {
            var trees = new List<Tuple<BaseEvent, List<BaseEventListener>>>();

            foreach (var listener in FindObjectsOfType<BaseEventListener>())
            {
                BaseEvent baseEvent = listener.BaseEvent;
                int index = trees.FindIndex((el) => { return el.Item1 == baseEvent; });
                if (index == -1)
                {
                    trees.Add(new Tuple<BaseEvent, List<BaseEventListener>>(
                        baseEvent,
                        new List<BaseEventListener>() { listener })
                    );
                }
                else
                {
                    trees[index].Item2.Add(listener);
                }
            }

            return trees;
        }
    }
}