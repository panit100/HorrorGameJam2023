using System;
using Unity.VisualScripting;

public interface IAstronosisDebug
{
     void DebugToggle(debugMode mode)
    {
        
    }
     [Serializable]
     enum debugMode
    {
        None,
        IgnoreDependencie,
        Playing,
    }

 
}

