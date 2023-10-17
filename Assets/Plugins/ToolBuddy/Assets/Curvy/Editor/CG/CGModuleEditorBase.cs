// =====================================================================
// Copyright 2013-2023 ToolBuddy
// All rights reserved
// 
// http://www.toolbuddy.net
// =====================================================================

using System;
using System.Collections.Generic;
using FluffyUnderware.Curvy.Generator;
using FluffyUnderware.DevTools;
using FluffyUnderware.DevTools.Extensions;
using FluffyUnderware.DevToolsEditor;
using FluffyUnderware.DevToolsEditor.Extensions;
using JetBrains.Annotations;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

namespace FluffyUnderware.CurvyEditor.Generator
{
    public class CGModuleEditorBase : CurvyEditorBase<CGModule>
    {
        public CGGraph Graph { get; internal set; }

        public AnimBool ShowDebugStats { get; set; }
        public bool ShowDebugVisuals { get; set; }
        protected bool HasDebugVisuals;

        protected override void OnEnable()
        {
            base.OnEnable();
            ShowDebugStats = new AnimBool(true);
            ShowDebugStats.speed = 3;
            HasDebugVisuals = false;
            EndIPE();
        }

        /// <summary>
        /// Called by the graph when an IPE session starts
        /// </summary>
        internal virtual void BeginIPE() { }

        /// <summary>
        /// Called by the graph when an IPE session ends
        /// </summary>
        internal virtual void EndIPE() { }

        /// <summary>
        /// Called for the IPE Target when the module should TRS it's IPE editor to the given values
        /// </summary>
        internal virtual void OnIPESetTRS(Vector3 position, Quaternion rotation, Vector3 scale) { }

        /// <summary>
        /// Called for the IPE initiator to get the TRS values for the target
        /// </summary>
        internal virtual void OnIPEGetTRS(out Vector3 position, out Quaternion rotation, out Vector3 scale)
        {
            position = Vector3.zero;
            rotation = Quaternion.identity;
            scale = Vector3.one;
        }


        /// <summary>
        /// Scene View GUI
        /// </summary>
        /// <remarks>Called only if the module is initialized and configured</remarks>
        public virtual void OnModuleSceneGUI() { }

        /// <summary>
        /// Scene View Debug GUI
        /// </summary>
        /// <remarks>Called only when Show Debug Visuals is activated</remarks>
        public virtual void OnModuleSceneDebugGUI() { }

        /// <summary>
        /// Inspector Debug GUI
        /// </summary>
        /// <remarks>Called only when Show Debug Values is activated </remarks>
        public virtual void OnModuleDebugGUI() { }

        protected override void OnCustomInspectorGUI()
        {
            base.OnCustomInspectorGUI();
            for (int m = 0; m < Target.UIMessages.Count; m++)
                EditorGUILayout.HelpBox(
                    Target.UIMessages[m],
                    MessageType.Warning
                );
        }

        protected override void OnReadNodes() =>
            base.OnReadNodes();

        public override void OnInspectorGUI()
        {
            //With the new prefab system (Unity 2018.3) prefabs don't show inspector, and when opening prefab editor, its objects are of type PrefabAssetType.NotAPrefab, so no way to know if its from prefab or not?

            if (Target == null)
                return;

            if (!Target.IsInitialized)
                Target.Initialize();

            DTGroupNode slotSection;

            if (DTGUI.IsLayout
                && IsInsideInspector
                && (Target.Input.Count > 0 || Target.Output.Count > 0)
                && !Node.FindNode(
                    "Slots",
                    out slotSection
                ))
            {
                Node.AddSection(
                    "Slots",
                    OnShowSlots
                ).SortOrder = 99999;
                Node.Sort();
            }

            if (GUILayout.Button(
                    new GUIContent(
                        CurvyStyles.OpenGraphTexture,
                        "Edit Graph"
                    )
                )
                && Target.Generator)
            {
                CGGraph win = CGGraph.Open(Target.Generator);
                win.Sel.SetSelectionTo(Target);
                win.Canvas.FocusSelection();
            }

            base.OnInspectorGUI();
        }

        private void OnShowSlots(DTInspectorNode node)
        {
            if (Target)
            {
                List<CGModuleInputSlot> inSlots = Target.Input;
                List<CGModuleOutputSlot> outSlots = Target.Output;

                EditorGUILayoutExtension.Vertical(
                    () =>
                    {
                        if (inSlots.Count > 0)
                        {
                            EditorGUILayout.LabelField(
                                "Input",
                                EditorStyles.boldLabel
                            );
                            showSlots(inSlots);
                        }

                        if (outSlots.Count > 0)
                        {
                            EditorGUILayout.LabelField(
                                "Output",
                                EditorStyles.boldLabel
                            );
                            showSlots(outSlots);
                        }
                    },
                    GUI.skin.box
                );

                GUILayout.Space(10);
            }
        }

        private void showSlots<T>(List<T> slots) where T : CGModuleSlot
        {
            foreach (CGModuleSlot slot in slots)
            {
                List<CGModule> linked = slot.GetLinkedModules();
                if (linked.Count > 1)
                    for (int i = 0; i < linked.Count; i++)
                    {
                        Object sel = EditorGUILayout.ObjectField(
                            i == 0
                                ? slot.Info.DisplayName
                                : " ",
                            linked[i],
                            typeof(CGModule),
                            true
                        );
                        if (sel != linked[i])
                            DTLog.Log(
                                "[Curvy] Linking modules from the inspector isn't supported yet! Use the Graph editor instead!",
                                Target
                            );
                    }
                else
                {
                    CGModule lm = linked.Count == 0
                        ? null
                        : linked[0];
                    Object sel = EditorGUILayout.ObjectField(
                        slot.Info.DisplayName,
                        lm,
                        typeof(CGModule),
                        true
                    );
                    if (sel != lm)
                        DTLog.Log(
                            "[Curvy] Linking modules from the inspector isn't supported yet! Use the Graph editor instead!",
                            Target
                        );
                }
            }
        }


        public void OnInspectorDebugGUIINTERNAL(UnityAction onChange)
        {
            if (Target)
            {
                ShowDebugStats.valueChanged.RemoveListener(onChange);
                ShowDebugStats.valueChanged.AddListener(onChange);
                GUILayoutExtension.Horizontal(
                    () =>
                    {
                        ShowDebugStats.target = GUILayout.Toggle(
                            ShowDebugStats.target,
                            new GUIContent(
                                CurvyStyles.DebugTexture,
                                "Show Values"
                            ),
                            EditorStyles.toolbarButton
                        );

                        if (HasDebugVisuals)
                        {
                            EditorGUI.BeginChangeCheck();
                            ShowDebugVisuals = GUILayout.Toggle(
                                ShowDebugVisuals,
                                new GUIContent(
                                    CurvyStyles.DebugSceneViewTexture,
                                    "Visualize"
                                ),
                                EditorStyles.toolbarButton
                            );
                            if (EditorGUI.EndChangeCheck())
                                SceneView.RepaintAll();
                        }
                        else
                            ShowDebugVisuals = false;

                        GUILayout.FlexibleSpace();
                        GUILayout.Label(
                            string.Format(
                                "{0:0.###} ms",
                                Target.DEBUG_ExecutionTime.AverageMS
                            )
                        );
                        GUILayout.Label(
                            string.Format(
                                "{0:0} %",
                                (Target.DEBUG_ExecutionTime.AverageMS / Target.Generator.DEBUG_ExecutionTime.AverageMS) * 100
                            )
                        );
                    },
                    CurvyStyles.Toolbar
                );

                GUILayout.Space(2);

                EditorGUILayoutExtension.FadeGroup(
                    visible =>
                    {
                        if (visible)
                            OnModuleDebugGUI();
                    },
                    ShowDebugStats.faded
                );
            }
        }


        [UsedImplicitly]
        private void CBSeedOptions()
        {
            if (!Target.RandomizeSeed)
            {
                Rect r = EditorGUILayout.GetControlRect(
                    true,
                    16,
                    EditorStyles.numberField,
                    null
                );
                r.width -= 16;
                EditorGUI.PropertyField(
                    r,
                    serializedObject.FindProperty("m_Seed")
                );
                r.x += r.width;
                r.width = 16;
                if (GUI.Button(
                        r,
                        new GUIContent(
                            CurvyStyles.RndSeedTexture,
                            "Randomize now!"
                        ),
                        CurvyStyles.ImageButton
                    ))
                {
                    Target.Seed = unchecked((int)DateTime.Now.Ticks);
                    Target.Generator.Refresh();
                }
            }
        }
    }
}