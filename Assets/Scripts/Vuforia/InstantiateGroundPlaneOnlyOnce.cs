using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class InstantiateGroundPlaneOnlyOnce : MonoBehaviour
{
    public void onInteractiveHitTest(HitTestResult result) {
	    var listenerBehaviour = GetComponent<AnchorInputListenerBehaviour>();
	    if (listenerBehaviour != null)
	    {
	        listenerBehaviour.enabled = false;
	    }
    }

    public void performHitTestWithNormalizedCoordinates(Vector2 screenPos)
    {
        float x = Mathf.Clamp01(screenPos.x / Screen.width);
        float y = Mathf.Clamp01(screenPos.y / Screen.height);
        var listenerBehaviour = GetComponent<PlaneFinderBehaviour>();
        listenerBehaviour.PerformHitTest(new Vector2(x, y));
    }

    public void logPlaneInstantiated() 
    {
    	Debug.Log("Ground plane is instantiated");
    }
}
