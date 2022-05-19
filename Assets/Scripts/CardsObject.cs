using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

using Vuforia;

using BoardLib;

public class CardsObject : MonoBehaviour
{
    private protected string CARD_TAG = "Card";

    public int NumCards = 42;
    Board board;

    public GameObject trackedObjects;
    public GameObject boardObject;
    public GameObject modelsObject;

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

        modelsObject.SetActive(true);

        for( int i = 0; i < NumCards; i++ ) {

            var type_name = board.Cards[i].devName;
            Debug.Log("type_name: " + type_name);

            Development dev = board.DevOptions[0];
            for( int di = 0; di < board.DevOptions.Length; di++){
                if( type_name == board.DevOptions[di].name){
                    dev = board.DevOptions[di];
                    break;
                }
            }

            GameObject cardGO = Instantiate( Resources.Load("Prefabs/Card", typeof(GameObject)), transform, false) as GameObject;
            cardGO.name = String.Format("card_{0}_{1}", i, dev.type);
            cardGO.tag = CARD_TAG;

            cardGO.GetComponent<CardGO>().SetCard(dev, i, (float)1e-3 * board.Properties.cardSize, devTypeToColors[dev.type]);

            string imageTargetFilename = String.Format("{0}_{1}", i.ToString().PadLeft(3, '0'), board.Cards[i].name);
            Debug.Log(imageTargetFilename);

            // Attach model
            modelsObject.transform.Find(imageTargetFilename).transform.parent = cardGO.transform.Find("Geom/Model").transform; 
            var model = cardGO.transform.Find(String.Format("Geom/Model/{0}", imageTargetFilename));
            Debug.Log(model);
            model.transform.localScale = Vector3.Scale(model.transform.localScale, new Vector3(printedTargetSize, printedTargetSize, printedTargetSize));
            
            // // model.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            // model.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
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
            
            DefaultObserverEventHandler handler;
            handler = targetImageObject.gameObject.GetComponent<DefaultObserverEventHandler>();
            handler.OnTargetFound = new UnityEvent();
            handler.OnTargetLost = new UnityEvent();
            // Debug.Log("handler component");
            // Debug.Log(targetImageObject.gameObject.GetComponent<DefaultObserverEventHandler>().OnTargetFound);
            handler.OnTargetFound.AddListener(cardGO.GetComponent<CardGO>().HandleTargetFound);
            handler.OnTargetLost.AddListener(cardGO.GetComponent<CardGO>().HandleTargetLost);


            targetImageObject.gameObject.transform.SetParent(trackedObjects.transform);
            // targetImageObject.gameObject.GetComponent<>();

            GameObject debugObj =  Instantiate( Resources.Load("Prefabs/TrackedDebug", typeof(GameObject)), targetImageObject.transform, false) as GameObject;
        }

        modelsObject.SetActive(false);
    }


    // Update is called once per frame
    void OnDisable(){

    }

}
