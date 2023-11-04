using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomPostprocessingManager : Singleton<CustomPostprocessingManager>
{
   public Fog fog; 
   protected override void InitAfterAwake()
   {
      fog = FindFirstObjectByType<Fog>();
   }
}
