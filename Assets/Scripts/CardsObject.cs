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

        Transform cardTargetsTransform = GameObject.Find("CardTargets").transform;


        Transform cardsTransform = transform.Find("Cards");
        if(transform.Find("Cards")){
            Destroy(cardsTransform.gameObject);
        }

        GameObject cardsGO = new GameObject("Cards");
        cardsTransform = cardsGO.transform;
        cardsTransform.parent = transform;

        for( int i = 0; i < NumCards; i++ ) {
            Development dev = board.DevOptions[i % board.DevTypes.Length ];

            GameObject cardGO = Instantiate( Resources.Load("Prefabs/Card", typeof(GameObject)), transform, false) as GameObject;
            cardGO.name = String.Format("card_{0}_{1}", i, dev.type);
            cardGO.tag = CARD_TAG;

            // float a = Mathf.PI * (((float)i  / NumCards) - 0.5f);
            // cardGO.transform.position = transform.position +  new Vector3( Mathf.Sin(a), 0.0f, Mathf.Cos(a));
            // cardGO.transform.rotation = Quaternion.Euler(
            //     transform.rotation.eulerAngles +  new Vector3( 0.0f, Mathf.Rad2Deg *  a, 0.0f));


            cardGO.GetComponent<CardGO>().SetCard(dev, i, (float)1e-3 * board.Properties.cardSize, devTypeToColors[dev.type]);

            string imageTargetFilename = String.Format("{0}_{1}", i.ToString().PadLeft(3, '0'), board.Cards[i].name);

            Debug.Log(String.Format("Models/{0}", imageTargetFilename));
            Debug.Log(GameObject.Find(String.Format("Models/{0}", imageTargetFilename)));

            // Attach model
            // GameObject.Find(String.Format("Models/{0}", imageTargetFilename)).transform.parent = cardGO.transform.Find("Geom/Model").transform; 
            // var model = cardGO.transform.Find(String.Format("Geom/Model/{0}", imageTargetFilename));
            // model.transform.localScale = Vector3.one;
            // model.GetComponent<MeshRenderer>().enabled = true;

            // Create image target for the card
            string targetName = String.Format("targetImage_{0}_{1}", i, dev.type);

            string fullFilePath = Application.streamingAssetsPath + String.Format("/Output/{0}.jpg", imageTargetFilename);
            
            byte[] pngBytes = System.IO.File.ReadAllBytes(fullFilePath);
            Texture2D texture = new Texture2D(10, 10);
            texture.LoadImage(pngBytes);

            var targetImageObject = VuforiaBehaviour.Instance.ObserverFactory.CreateImageTarget(
                fullFilePath,
                printedTargetSize,
                targetName
            );
            Debug.Log(targetImageObject);

            // add the Default Observer Event Handler to the newly created game object
            targetImageObject.gameObject.AddComponent<DefaultObserverEventHandler>();
            targetImageObject.gameObject.AddComponent<CardTargetGO>();
            // targetImageObject.gameObject.transform.localScale = new Vector3(printedTargetSize, 1.0f, printedTargetSize);

           

            // Put into the tree
            cardGO.transform.SetParent(cardsTransform);
            cardGO.GetComponent<CardGO>().target = targetImageObject.gameObject.transform;
            targetImageObject.gameObject.transform.SetParent(cardTargetsTransform);
        }

        // GameObject.Find("Ground Plane Stage/Models").SetActive(false);
    }


    // Update is called once per frame
    void OnDisable(){

    }

}
