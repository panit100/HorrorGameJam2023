// =====================================================================
// Copyright 2013-2022 ToolBuddy
// All rights reserved
// 
// http://www.toolbuddy.net
// =====================================================================

using UnityEngine;

namespace FluffyUnderware.DevTools
{
    /// <summary>
    /// A InitializeOnLoadMethodAttribute attribute that works both in editor and in build
    /// This attribute is a substitute to either Unity's InitializeOnLoadMethodAttribute and RuntimeInitializeOnLoadMethodAttribute
    /// </summary>
#if UNITY_2021_3_OR_NEWER == false
    [System.Obsolete("EnvironmentAgnosticInitializeOnLoadMethodAttribute is not compatible with Unity versions prior to 2021.3")]
#endif
    public class EnvironmentAgnosticInitializeOnLoadMethodAttribute
#if UNITY_EDITOR
        : UnityEditor.InitializeOnLoadMethodAttribute
#else
        : RuntimeInitializeOnLoadMethodAttribute
#endif
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="loadType">The execution time when in runtime. Is not used in editor</param>
        public EnvironmentAgnosticInitializeOnLoadMethodAttribute(RuntimeInitializeLoadType loadType)
#if UNITY_EDITOR
            : base()
#else
            : base(loadType)
#endif
        { }
    }
}