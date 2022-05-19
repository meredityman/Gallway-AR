using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Vuforia;

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

    private bool isLocked = false;
    private bool canBecomeLocked = false;
    private bool isTrackingTarget = false;
    private GameObject buildingModel;
    
    // Start is called before the first frame update
    void Start()
    {
        textMesh = GetComponentInChildren<TextMeshProUGUI>();
        textMesh.text = string.Format("Card {0}\n{1}", index, devName);

        mat = transform.Find("Geom/Quad").GetComponent<MeshRenderer>().material;
        mat.color = c_notDocked;

        buildingModel = transform.Find("Geom/Model").gameObject;
        buildingModel.SetActive(false);

        gameObject.SetActive(false);
    }

    public void HandleTargetFound()
    {
        isTrackingTarget = true;
        gameObject.SetActive(true);
    }

    public void HandleTargetLost()
    {
        isTrackingTarget = false;
        // GameObject.SetActive(false);
    }

    void OnEnable()
    {
        StateLib.StateManager.OnStateChange += HandleWorldStateChange;
    }

    void OnDisable()
    {
        StateLib.StateManager.OnStateChange -= HandleWorldStateChange;
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
                switch (status) {
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
                if (isTrackingTarget)
                {
                    switch (status) {
                        case  DefaultObserverEventHandler.TrackingStatusFilter.Tracked:
                        case  DefaultObserverEventHandler.TrackingStatusFilter.Tracked_ExtendedTracked:
                        case  DefaultObserverEventHandler.TrackingStatusFilter.Tracked_ExtendedTracked_Limited:
                            this.transform.position = target.transform.position;
                            this.transform.rotation = target.transform.rotation;
                            break;
                    }
                }
            }

            if (isLocked)
            {
                return;
            }

            DevSiteGO closestSite = board.getClosestSite(target.transform.position);

            if (closestSite != null)
            {
                if(closestSite != site) {
                    this.LeaveSite(site);
                    this.EnterSite(closestSite);

                    if (this.canBecomeLocked && !this.isLocked)
                    {
                        this.isLocked = true;
                        AnimateBuilding();
                        DisableTarget();
                    }
                }
            } else {
                this.LeaveSite(site);
            }

            if (this.site && this.canBecomeLocked && !this.isLocked)
            {
                this.isLocked = true;
                AnimateBuilding();
                DisableTarget();
            }
        }
    }

    private void AnimateBuilding()
    {
        this.buildingModel.SetActive(true);
        LeanTween.scale( this.buildingModel, new Vector3(1.25f, 1.25f, 1.25f), 1f).setEase(LeanTweenType.easeInOutCirc).setLoopPingPong( 1 );
    }

    private void DisableTarget()
    {
       this.target.GetComponent<ImageTargetBehaviour>().enabled = false;
       this.target.gameObject.SetActive(false);
    }

    private void EnableTarget()
    {
        this.target.GetComponent<ImageTargetBehaviour>().enabled = true;
        this.target.gameObject.SetActive(true);
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

    private void HandleWorldStateChange(StateLib.StateManager.State newState)
    {
        if(newState.name == StateName.Init)
        {
            EnableTarget();
        }

        if(newState.name == StateName.Cards || newState.name == StateName.Score)
        {
            this.canBecomeLocked = true;
        }
        else
        {
            this.canBecomeLocked = false;
            this.isLocked = false;
            this.buildingModel.SetActive(false);
        }
    } 
}
