using System;
using System.Collections.Generic;
using UnityEngine;

using BoardLib;

public class CardsObject : MonoBehaviour
{
    public int NumCards = 42;
    Board board;


    public Dictionary<string, Color[]> devTypeToColors;

    // Start is called before the first frame update
    void OnEnable()
    {
        board = BoardFactory.getDefaultBoard();

        devTypeToColors = new Dictionary<string, Color[]>();
        for( int zi = 0; zi < board.DevTypes.Length; zi++){
            devTypeToColors[board.DevTypes[zi].name] = new Color[]{
                board.DevTypes[zi].c_docked,
                board.DevTypes[zi].c_notDocked,
            };
        }


        Transform cardsTransform = transform.Find("Cards");
        if(transform.Find("Cards")){
            Destroy(cardsTransform.gameObject);
        }

        GameObject cardsGO = new GameObject("Cards");
        cardsTransform = cardsGO.transform;
        cardsTransform.parent = transform;

        for( int i = 0; i < NumCards; i++ ){
            Development dev = board.DevOptions[i % board.DevTypes.Length ];

            GameObject cardGO = Instantiate( Resources.Load("Prefabs/Card", typeof(GameObject)), transform, false) as GameObject;
            cardGO.name = String.Format("card_{0}_{1}", i, dev.type);
            cardGO.transform.parent = cardsTransform;


            float a = Mathf.PI * (((float)i  / NumCards) - 0.5f);
            cardGO.transform.position = transform.position +  new Vector3( Mathf.Sin(a), 0.0f, Mathf.Cos(a));
            cardGO.transform.rotation = Quaternion.Euler(
                transform.rotation.eulerAngles +  new Vector3( 0.0f, Mathf.Rad2Deg *  a, 0.0f));


            cardGO.GetComponent<CardGO>().SetCard(dev, i, (float)1e-3 * board.Properties.cardSize, devTypeToColors[dev.type]);
        }

    }


    // Update is called once per frame
    void OnDisable(){

    }

}
