// =====================================================================
// Copyright 2013-2023 ToolBuddy
// All rights reserved
// 
// http://www.toolbuddy.net
// =====================================================================

using FluffyUnderware.Curvy.Shapes;
using FluffyUnderware.DevTools;
using FluffyUnderware.DevTools.Extensions;
using UnityEngine;

namespace FluffyUnderware.Curvy.Generator
{
    /// <summary>
    /// Shape (2D spline) resource loader class
    /// </summary>
    public class CGShapeResourceLoader : ICGResourceLoader
    {
#if UNITY_2021_3_OR_NEWER
        [EnvironmentAgnosticInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
#else
#if UNITY_EDITOR
        [UnityEditor.InitializeOnLoadMethod]
#else
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
#endif
#endif
        protected static void InitializeOnLoad() =>
            CGResourceHandler.RegisterResourceLoader(
                "Shape",
                new CGShapeResourceLoader()
            );

        public Component Create(CGModule cgModule, string context)
        {
            CurvySpline spl = CurvySpline.Create();
            spl.transform.position = Vector3.zero;
            spl.RestrictTo2D = true;
            spl.Closed = true;
            spl.Orientation = CurvyOrientation.None;
            spl.gameObject.AddComponent<CSCircle>().Refresh();
            return spl;
        }

        public void Destroy(CGModule cgModule, Component obj, string context, bool kill)
        {
            if (obj != null)
                obj.gameObject.Destroy(
                    false,
                    false
                ); //isUndoable is set to false because that's how it was working before. Try make this operation undoable if needed
        }
    }
}