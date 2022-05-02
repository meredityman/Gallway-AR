using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using BoardLib;
using StateLib;

public class CardGO : MonoBehaviour
{

    TextMeshProUGUI textMesh;

    int index;
    public string devName;
    public Transform target;
    public BoardObject board;

    Color c_notDocked;
    Color c_docked;

    DevSiteGO site;

    Material mat;

    private GameObject arCamera;
    
    // Start is called before the first frame update
    void Start()
    {
        textMesh = GetComponentInChildren<TextMeshProUGUI>();
        textMesh.text = string.Format("Card {0}\n{1}", index, devName);

        mat = transform.Find("Geom/Quad").GetComponent<MeshRenderer>().material;
        mat.color = c_notDocked;

        arCamera = GameObject.Find("ARCamera");
    }


    public void SetCard(in Development dev, int i, Vector2 size, Color[] colors)
    {
        index = i;
        devName = dev.name;
        transform.localScale = new Vector3(size.x, Mathf.Min(size.x, size.y), size.y);

        c_docked = colors[0];
        c_notDocked = colors[1];
    }

    // Update is called once per frame
    void FixedUpdate()  
    {
        if(target != null) {
            var status = target.GetComponent<DefaultObserverEventHandler>().StatusFilter;
            
            if(site) {
                switch (status){
                    case  DefaultObserverEventHandler.TrackingStatusFilter.Tracked:
                        this.transform.position = target.transform.position;
                        this.transform.rotation = target.transform.rotation;
                        break;
                    case  DefaultObserverEventHandler.TrackingStatusFilter.Tracked_ExtendedTracked:
                    case  DefaultObserverEventHandler.TrackingStatusFilter.Tracked_ExtendedTracked_Limited:
                        this.transform.position = site.transform.position;
                        this.transform.rotation = site.transform.rotation;
                        break;
                }
            } else {
                switch (status) {
                    case  DefaultObserverEventHandler.TrackingStatusFilter.Tracked:
                    case  DefaultObserverEventHandler.TrackingStatusFilter.Tracked_ExtendedTracked:
                    case  DefaultObserverEventHandler.TrackingStatusFilter.Tracked_ExtendedTracked_Limited:
                        this.transform.position = target.transform.position;
                        this.transform.rotation = target.transform.rotation;
                        break;
                }
            }

            if (board && board.stateManager.isIn(StateName.Cards))
            {
                DevSiteGO closestSite = board.getClosestSite(target.transform.position);

                if (closestSite != null)
                {
                    if(closestSite != site) {
                        this.LeaveSite(site);
                        this.EnterSite(closestSite);

                        // Possibly disable target, once card is inside the site.
                    }
                } else {
                    this.LeaveSite(site);
                }
            }

        }
    }

    void OnGUI () 
    {
        if (index == 0) 
        {
            GUI.Label (new Rect (0,250,100,100), String.Format("AR camera: {0}", arCamera.transform.position ) );
            GUI.Label (new Rect (0,350,100,100), String.Format("Camera Euler: {0}", arCamera.transform.rotation.eulerAngles ) );
            GUI.Label (new Rect (0,100 + 50 * index, 100,50), String.Format("Card {1} Pos: {0}", transform.position, index) );
            GUI.Label (new Rect (0,150 + 50 * index, 100,50), String.Format("Card {1} Local Pos: {0}", transform.localPosition, index) );
        }
    }

    private void EnterSite(DevSiteGO site)
    {
        if (site) {
            if(site.TryAttach(this)){
                this.site = site;
            }
        }
    }

    private void LeaveSite(DevSiteGO site)
    {
        if(site) {
            site.Remove(this);
            this.site = null;
            mat.color = c_notDocked;
        }
    }

    // private void OnTriggerEnter(Collider other){
    //     var site = other.GetComponentInParent<DevSiteGO>();

    //     if (site) {
    //         if(site.TryAttach(this)){
    //             this.site = site;
    //         }
    //     }
    // }   
    // private void OnTriggerStay(Collider other){
    //     var site = other.GetComponentInParent<DevSiteGO>();

    //     if (site) {
    //         if(site.TryAttach(this)){
    //             this.site = site;
    //             mat.color = c_docked;
    //         }
    //     }
    // }   

    // private void OnTriggerExit(Collider other){
    //     var site = other.GetComponentInParent<DevSiteGO>();
    //     if(site){
    //         site.Remove(this);
    //         this.site = null;
    //         mat.color = c_notDocked;
    //     }
    // }   
}
