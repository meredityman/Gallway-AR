using System;
using System.Collections.Generic;
using UnityEngine;

using Vuforia;

using BoardLib;

public class CardsObject : MonoBehaviour
{
    private protected string CARD_TAG = "Card";

    public int NumCards = 42;
    Board board;

    public GameObject trackedObjects;
    public GameObject boardObject;

    public Dictionary<string, Color[]> devTypeToColors;

    // Start is called before the first frame update
    void OnEnable()
    {
        board = BoardFactory.getDefaultBoard();
        VuforiaApplication.Instance.OnVuforiaStarted += InitCards;
    }

    void InitCards()
    {
        float printedTargetSize = (float)1e-3 * board.Properties.cardSize.x;

        devTypeToColors = new Dictionary<string, Color[]>();
        for( int zi = 0; zi < board.DevTypes.Length; zi++){
            devTypeToColors[board.DevTypes[zi].name] = new Color[]{
                board.DevTypes[zi].c_docked,
                board.DevTypes[zi].c_notDocked,
            };
        }

        for( int i = 0; i < NumCards; i++ ) {
            Development dev = board.DevOptions[i % board.DevTypes.Length ];

            GameObject cardGO = Instantiate( Resources.Load("Prefabs/Card", typeof(GameObject)), transform, false) as GameObject;
            cardGO.name = String.Format("card_{0}_{1}", i, dev.type);
            cardGO.tag = CARD_TAG;

            cardGO.GetComponent<CardGO>().SetCard(dev, i, (float)1e-3 * board.Properties.cardSize, devTypeToColors[dev.type]);

            string imageTargetFilename = String.Format("{0}_{1}", i.ToString().PadLeft(3, '0'), board.Cards[i].name);

            // Attach model
            // GameObject.Find(String.Format("Models/{0}", imageTargetFilename)).transform.parent = cardGO.transform.Find("Geom/Model").transform; 
            // var model = cardGO.transform.Find(String.Format("Geom/Model/{0}", imageTargetFilename));
            // model.transform.localScale = Vector3.one;
            // model.GetComponent<MeshRenderer>().enabled = true;

            // Create image target for the card
            string targetName = String.Format("targetImage_{0}_{1}", i, dev.type);


            string resourcePath = String.Format("Cards/{0}", imageTargetFilename);       

            var texture = Resources.Load<Texture2D>( resourcePath);


            Debug.Log("Loading Target: " + resourcePath);
            var targetImageObject = VuforiaBehaviour.Instance.ObserverFactory.CreateImageTarget(
                texture,
                printedTargetSize,
                targetName
            );
            Debug.Log(targetImageObject);

            // add the Default Observer Event Handler to the newly created game object
            targetImageObject.gameObject.AddComponent<DefaultObserverEventHandler>();

            // Put into the tree
            cardGO.transform.SetParent(this.transform);
            cardGO.GetComponent<CardGO>().target = targetImageObject.gameObject.transform;
            cardGO.GetComponent<CardGO>().board = boardObject.GetComponent<BoardObject>();
            cardGO.transform.position = new Vector3(10.0f, 0.0f, 10.0f); 


            targetImageObject.gameObject.transform.SetParent(trackedObjects.transform);


            GameObject debugObj =  Instantiate( Resources.Load("Prefabs/TrackedDebug", typeof(GameObject)), targetImageObject.transform, false) as GameObject;
            // targetImageObject.gameObject.transform.SetParent(cardTargetsTransform);
        }

        // GameObject.Find("Ground Plane Stage/Models").SetActive(false);
    }


    // Update is called once per frame
    void OnDisable(){

    }

}
