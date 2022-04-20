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
}
