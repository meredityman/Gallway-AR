using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using TMPro;

using StateLib;

public class UI : MonoBehaviour
{
    public GameObject scoreGO;
    public GameObject buttonGO;
    public GameObject instructionsGO;
    public BoardObject board;

    public TextMeshProUGUI instructionsTextMesh;
    public TextMeshProUGUI scoreTextMesh;
    public Transform button;
    public TextMeshProUGUI buttonTextMesh;

    void OnEnable()
    {
        // Subscribe to state change
        StateManager.OnStateChange += UpdateUIText;
        
        // 
    }

    void OnDisable()
    {
        StateManager.OnStateChange -= UpdateUIText;
    }

    void UpdateUIText(StateManager.State newState)
    {

        buttonTextMesh = buttonGO.GetComponent<TextMeshProUGUI>();
        instructionsTextMesh = instructionsGO.GetComponent<TextMeshProUGUI>(); 
        scoreTextMesh = scoreGO.GetComponent<TextMeshProUGUI>(); 

    	instructionsTextMesh.text = (string)newState.instructionsText;
        buttonTextMesh.text = (string)newState.buttonText;

        //  NULL obj reference wtf?...

        // Update button
        if (string.IsNullOrEmpty(newState.buttonText))
        {
            buttonGO.SetActive(false);
        } 
        else
        {
            buttonGO.SetActive(true);
        }

        if (newState.name == StateName.Score)
        {
            scoreTextMesh.text = board.getScoreText();
        }
        else
        {
            scoreTextMesh.text = "";
        }
    }
}
