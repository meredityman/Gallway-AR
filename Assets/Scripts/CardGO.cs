using System;
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
    public Transform target;

    Color c_notDocked;
    Color c_docked;

    DevSiteGO site;

    Material mat;

    private Rigidbody rb;
    private GameObject arCamera;
    
    // Start is called before the first frame update
    void Start()
    {
        textMesh = GetComponentInChildren<TextMeshProUGUI>();
        textMesh.text = string.Format("Card {0}\n{1}", index, devName);

        mat = transform.Find("Geom/Quad").GetComponent<MeshRenderer>().material;
        mat.color = c_notDocked;

        rb = GetComponent<Rigidbody>();
        arCamera = GameObject.Find("ARCamera");
    }


    public void SetCard(in Development dev, int i, Vector2 size, Color[] colors){
        index = i;
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

    void FixedUpdate()  {
        // Image target position
        Vector3 targetPosition = target.transform.position; 

        // The trackable poses (ImageTarget, CylinderTarget, MultiTarget, VuMark, ObjectTarget, ModelTarget, Anchor, DeviceTrackable) are now all reported in World Coordinate System.
        //

        // Raycast intersection with y=0.0 plane
        Vector3 targetScreenPos = arCamera.GetComponent<Camera>().WorldToScreenPoint(targetPosition);
        Ray ray = arCamera.GetComponent<Camera>().ScreenPointToRay(targetScreenPos);
        float delta = ray.origin.y - 0.0f;
        Vector3 dirNorm = ray.direction / ray.direction.y;
        Vector3 IntersectionPos = ray.origin - dirNorm * delta;

        // Vector3 targetOffset = target.transform.localPosition - arCamera.transform.position;
        // positionInPlane += targetOffset;

        // targetPosition.y = 0.0f;

        // Set rb position
        rb.position = IntersectionPos;
        // rb.position = targetPosition;

        Vector3 rotInPlane = target.rotation.eulerAngles;
        rotInPlane.x = 0.0f;
        rotInPlane.z = 0.0f;
        rb.rotation = Quaternion.Euler(rotInPlane);
    }

    void OnGUI () {
        GUI.Label (new Rect (0,100 + 50 * index, 100,50), String.Format("Card {1} Pos: {0}", transform.position, index) );
        GUI.Label (new Rect (0,150 + 50 * index, 100,50), String.Format("Card {1} Local Pos: {0}", transform.localPosition, index) );
        // GUI.Label (new Rect (0,150,100,50), String.Format("Card Target Transform: {0}", target.transform.position) );
        // GUI.Label (new Rect (0,200,100,50), String.Format("Offset: {0}", target.position - arCamera.transform.position) );
        if (index == 0) 
        {
            GUI.Label (new Rect (0,250,100,100), String.Format("AR camera: {0}", arCamera.transform.position ) );
            GUI.Label (new Rect (0,350,100,100), String.Format("Camera Euler: {0}", arCamera.transform.rotation.eulerAngles ) );
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
