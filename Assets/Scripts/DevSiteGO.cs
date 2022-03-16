using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;

using BoardLib;

public class DevSiteGO : MonoBehaviour
{
    // Start is called before the first frame update
    TextMeshProUGUI textMesh;

    string zoneName;

    Color c_notDocked;
    Color c_docked;
    public string getZoneName(){
        return zoneName;
    }
    int index;

    public CardGO getCard(){
        return card;
    }

    CardGO card;
    Material mat;
    MeshRenderer rend;
    void Start()
    {
        textMesh = GetComponentInChildren<TextMeshProUGUI>();
        textMesh.text = string.Format("Site {0}\n{1}", index, zoneName);

        mat = transform.Find("Geom/Quad").GetComponent<MeshRenderer>().material;
        rend = transform.Find("Geom/Quad").GetComponent<MeshRenderer>();
        mat.color = c_notDocked;
    }

    public void SetSite(in Site site, int index, Vector2 size, Color[] colors){
        zoneName = site.zone;
        
        transform.localScale = new Vector3(size.x, Mathf.Min(size.x, size.y), size.y);  

        c_docked = colors[0];
        c_notDocked = colors[1];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool TryAttach(CardGO card){

        if(this.card){
            if( Vector3.Distance(transform.position, this.card.transform.position ) <
                Vector3.Distance(transform.position, card.transform.position ) ){
                return false;
            } 
        }
        this.card = card;
        rend.enabled = false;
        return true;
    }


    public void Remove(CardGO card){
        if(card == this.card)
            this.card = null;
            rend.enabled = true;
    }
}
