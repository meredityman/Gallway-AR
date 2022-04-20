using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable : MonoBehaviour
{
    
    private Transform selectedObjectTransform;
    private bool isDragging = false;
    private Vector2 offset = new Vector2(0.0f, 0.0f);
    private protected string CARD_TAG = "Card";

	RaycastHit hit;
    
    Camera cam;

    void Start()
    {
        offset = new Vector2(0.0f, 0.0f);
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
    	Vector3 v3;
    	float height = 0.0f;

    	// Debug.Log(Input.GetMouseButtonDown(0));
    	// Debug.Log(Input.touchCount);

        if (Input.touchCount != 1) 
        {
        	isDragging = false;
        	return;
        }

        Touch touch = Input.touches[0];
        Vector3 pos = touch.position;

        if (touch.phase == TouchPhase.Began) 
        {
        	Ray ray = cam.ScreenPointToRay(touch.position);
        	if (Physics.Raycast(ray, out hit)) 
        	{
        		if (hit.transform.tag == CARD_TAG) 
        		{
        			selectedObjectTransform = hit.transform;
        			// height = selectedObjectTransform.position.z - cam.transform.position.z;
        			v3 = new Vector3(pos.x, pos.y, height);
        			v3 = cam.ScreenToWorldPoint(v3);
        			offset = selectedObjectTransform.position - v3;
        			isDragging = true;
        		}
        	}
        }

        if (isDragging && touch.phase == TouchPhase.Moved) 
        {
        	v3 = new Vector3(Input.mousePosition.x, Input.mousePosition.y, height);
        	v3 = cam.ScreenToWorldPoint(v3);
        	selectedObjectTransform.position = v3 + new Vector3(offset.x, offset.y, 0.0f);
        }

        if (isDragging && (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)) 
        {
        	isDragging = false;
        }
    }

    void OnGUI()
    {
        Vector3 point = new Vector3();
        Event   currentEvent = Event.current;
        Vector2 mousePos = new Vector2();

        mousePos.x = currentEvent.mousePosition.x;
        mousePos.y = cam.pixelHeight - currentEvent.mousePosition.y;

        GUILayout.BeginArea(new Rect(200, 2, 250, 120));
       // GUILayout.Label("Mouse position: " + mousePos);
        GUILayout.Label("World position: " + point.ToString("F3"));
        GUILayout.Label("Dragging: " + isDragging.ToString());
        GUILayout.Label("Offset: " + offset.ToString());
        GUILayout.Label("Hit: " + hit.collider.tag);
        GUILayout.Label("Hit Name: " + hit.transform.name);

        GUILayout.EndArea();
    }

    // void OnMouseMove() {
    // 	if (isDragging) {

    // 	}
    // }
}
