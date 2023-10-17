// =====================================================================
// Copyright 2013-2023 ToolBuddy
// All rights reserved
// 
// http://www.toolbuddy.net
// =====================================================================

using FluffyUnderware.Curvy.Generator.Modules;
using UnityEditor;

namespace FluffyUnderware.CurvyEditor.Generator.Modules
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(ModifierMixPaths))]
    public class ModifierMixPathsEditor : CGModuleEditor<ModifierMixPaths> { }
}