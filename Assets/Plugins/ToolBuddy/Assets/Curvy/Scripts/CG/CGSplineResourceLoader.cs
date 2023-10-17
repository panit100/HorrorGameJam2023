// =====================================================================
// Copyright 2013-2023 ToolBuddy
// All rights reserved
// 
// http://www.toolbuddy.net
// =====================================================================

using FluffyUnderware.DevTools;
using FluffyUnderware.DevTools.Extensions;
using UnityEngine;

namespace FluffyUnderware.Curvy.Generator
{
    /// <summary>
    /// Spline resource loader class
    /// </summary>
    public class CGSplineResourceLoader : ICGResourceLoader
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
                "Spline",
                new CGSplineResourceLoader()
            );

        public Component Create(CGModule cgModule, string context)
        {
            CurvySpline spl = CurvySpline.Create();
            spl.transform.position = Vector3.zero;
            spl.Closed = true;
            spl.Add(
                new Vector3(
                    0,
                    0,
                    0
                ),
                new Vector3(
                    5,
                    0,
                    10
                ),
                new Vector3(
                    -5,
                    0,
                    10
                )
            );
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