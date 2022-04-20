using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using Vuforia;

using BoardLib;

public class SetUpImageTargets : MonoBehaviour
{
    Board board;

  	void Start()
	{
		VuforiaApplication.Instance.OnVuforiaStarted += CreateImageTargets;
        board = BoardFactory.getDefaultBoard();
	}

    void CreateImageTargets()
    {
	    Texture2D texture;
	    float printedTargetSize = 0.3f;
	  	string targetName;

	  	Transform cardsTransform = transform.Find("Cards");


	    for( int i = 0; i < 24; i++ ) {
	    	targetName = String.Format("targetImage_{0}", i);
			// Development dev = board.DevOptions[i % board.DevTypes.Length ];

			// GameObject cardGO = Instantiate( Resources.Load("Prefabs/Card", typeof(GameObject)), transform, false) as GameObject;
			// cardGO.name = String.Format("card_{0}_{1}", i, dev.type);
			// cardGO.tag = CARD_TAG;
			// cardGO.transform.parent = cardsTransform;
	    	texture = new Texture2D(2, 2);


			var mTarget = VuforiaBehaviour.Instance.ObserverFactory.CreateImageTarget(
	            texture,
	            printedTargetSize,
	            targetName
            );
	        // add the Default Observer Event Handler to the newly created game object
	        mTarget.gameObject.AddComponent<DefaultObserverEventHandler>();

	        Debug.Log("Instant Image Target Created " + mTarget);
        	mTarget.gameObject.transform.SetParent(transform);
		}




    }
}
