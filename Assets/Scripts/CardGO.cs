using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using BoardLib;

public class CardGO : MonoBehaviour
{

    TextMeshProUGUI textMesh;

    int index;
    public string devName;

    Color c_notDocked;
    Color c_docked;

    DevSiteGO site;

    Material mat;
    
    // Start is called before the first frame update
    void Start()
    {
        textMesh = GetComponentInChildren<TextMeshProUGUI>();
        textMesh.text = string.Format("Card {0}\n{1}", index, devName);

        mat = transform.Find("Geom/Quad").GetComponent<MeshRenderer>().material;
        mat.color = c_notDocked;
    }


    public void SetCard(in Development dev, int index, Vector2 size, Color[] colors){
        devName = dev.name;
        transform.localScale = new Vector3(size.x, Mathf.Min(size.x, size.y), size.y);

        c_docked = colors[0];
        c_notDocked = colors[1];
        // var canvas = transform.Find("Canvas");
        // canvas.localScale =   new Vector3(1.0f / size.x, 1.0f, 1.0f / size.y);
    }

    // Update is called once per frame
    void Update()
    {
        if(site) {
            this.transform.position = site.transform.position;
            this.transform.rotation = site.transform.rotation;
        }
    }

    private void OnTriggerEnter(Collider other){
        var site = other.GetComponentInParent<DevSiteGO>();

        if (site) {
            if(site.TryAttach(this)){
                this.site = site;
            }
        }
    }   
    private void OnTriggerStay(Collider other){
        var site = other.GetComponentInParent<DevSiteGO>();

        if (site) {
            if(site.TryAttach(this)){
                this.site = site;
                mat.color = c_docked;
            }
        }
    }   

    private void OnTriggerExit(Collider other){
        var site = other.GetComponentInParent<DevSiteGO>();
        if(site){
            site.Remove(this);
            this.site = null;
            mat.color = c_notDocked;
        }
    }   
}
