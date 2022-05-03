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

    TextMeshProUGUI instructionsTextMesh;
	TextMeshProUGUI scoreTextMesh;
    Transform button;
    TextMeshProUGUI buttonTextMesh;
    public BoardObject board;

    void OnEnable()
    {
        // Subscribe to state change
        StateManager.OnStateChange += UpdateUIText;
        
        // 
        buttonTextMesh = buttonGO.GetComponent<TextMeshProUGUI>();
        instructionsTextMesh = instructionsGO.GetComponent<TextMeshProUGUI>(); 
        scoreTextMesh = scoreGO.GetComponent<TextMeshProUGUI>(); 
    }

    void OnDisable()
    {
        StateManager.OnStateChange -= UpdateUIText;
    }

    void UpdateUIText(StateManager.State newState)
    {
    	instructionsTextMesh.text = (string)newState.instructionsText;

        // Update button
        if (string.IsNullOrEmpty(newState.buttonText))
        {
            buttonGO.SetActive(false);
        } 
        else
        {
            buttonGO.SetActive(true);
            buttonTextMesh.text = newState.buttonText;
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
