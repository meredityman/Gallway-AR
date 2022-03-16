using System.IO;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using BoardLib;

public class BoardObject : MonoBehaviour
{
    Board board;

    List<DevSiteGO> devSites;
    TextMeshProUGUI textMesh;
    int numDockedCards = 0;

    Dictionary<DevelopmentImpact, float> scores;
    public Dictionary<string, int> zoneNameToIndex;
    public Dictionary<string, Color[]> siteZoneToColors;

    // Start is called before the first frame update
    void OnEnable()
    {
        textMesh = GetComponentInChildren<TextMeshProUGUI>();

        // Get board
        board = BoardFactory.getDefaultBoard();
        SaveBoard();

        zoneNameToIndex = new Dictionary<string, int>();
        for( int zi = 0; zi < board.DevZones.Length; zi++){
            zoneNameToIndex[board.DevZones[zi].name] = zi;
        }

        siteZoneToColors = new Dictionary<string, Color[]>();
        for( int zi = 0; zi < board.DevZones.Length; zi++){
            siteZoneToColors[board.DevZones[zi].name] = new Color[]{
                board.DevZones[zi].c_docked,
                board.DevZones[zi].c_notDocked,
            };
        }

        // Setup board
        var boardPlane = transform.Find("Anchors/Plane");
        boardPlane.transform.localScale = new Vector3(board.Properties.boardSize.x * (float)1e-4, 1.0f, board.Properties.boardSize.y * (float)1e-4);

        // Creates Sites parent object
        Transform sitesTransform = transform.Find("Sites");
        if(transform.Find("Sites")){
            Destroy(sitesTransform.gameObject);
        }

        GameObject sitesGO = new GameObject("Sites");
        sitesTransform = sitesGO.transform;
        sitesTransform.parent = transform;
        sitesTransform.position = new Vector3(
            -board.Properties.boardSize.x * 0.5f * (float)1e-3,
            0.0f, 
            -board.Properties.boardSize.y * 0.5f * (float)1e-3
        );


        // Populate Sites 
        devSites = new List<DevSiteGO>();
        for( int i = 0; i < board.DevSites.Length; i++ ){
            Site devSite = board.DevSites[i];
            GameObject siteGO = Instantiate( Resources.Load("Prefabs/Site", typeof(GameObject)), transform, false) as GameObject;
            siteGO.name = String.Format("site_{0}", i);
            siteGO.transform.parent   = sitesTransform;
            siteGO.transform.localPosition = new Vector3(devSite.position.x, 0.0f, devSite.position.y);

            siteGO.GetComponent<DevSiteGO>().SetSite(devSite, i, (float)1e-3 * board.Properties.cardSize, siteZoneToColors[devSite.zone]);

            devSites.Add(siteGO.GetComponent<DevSiteGO>());
        }

    }

    void SaveBoard(){
        string jsonFile = Application.dataPath + "/board.json";
        string json = JsonUtility.ToJson(this.board);
        File.WriteAllText(jsonFile, json);
    }

    // Update is called once per frame
    void Update()
    {

        scores = new Dictionary<DevelopmentImpact, float>();
        foreach( DevelopmentImpact i in board.DevImpacts){
            scores[i] = 0.0f;
        }

        numDockedCards = 0;
        foreach( DevSiteGO site in devSites){
            CardGO card = site.getCard();
            if(card){
                numDockedCards++;

                string zoneName = site.getZoneName();

                foreach(Development dev in board.DevOptions){
                    Debug.Log(dev.name + ", " +  card.name);
                    if(dev.name == card.devName){
                        Debug.Log("Here");
                        for( int i_impact = 0; i_impact < board.DevImpacts.Length; i_impact++){
                            DevelopmentImpact impact = board.DevImpacts[i_impact];
                            float i_score = dev.scores[(i_impact * board.DevZones.Length) + zoneNameToIndex[zoneName] ];

                            scores[impact] += i_score;
                        }

                        break;
                    }
                }
            }
        }


        string scoresStr = "";
        foreach(var pair in scores){
            scoresStr += string.Format("{0} : {1}\n", pair.Key.name, pair.Value);
        }

        textMesh.text = string.Format("c: {0}\n{1}", numDockedCards, scoresStr);
        //Debug.Log("Hello");

    }

}
