using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

using TMPro;

using StateLib;

public class UI : MonoBehaviour
{
    public GameObject scoreGO;
    public GameObject buttonGO;
    public GameObject progressBarGO;
    public GameObject hintGO;
    public BoardObject board;

    private TextMeshProUGUI instructionsTextMesh;
    private TextMeshProUGUI scoreTextMesh;
    private TextMeshProUGUI buttonTextMesh;
    private Slider slider;
    private Button button;

    void OnEnable()
    {
        // Subscribe to state change
        StateManager.OnStateChange += UpdateUIText;
        
        slider = progressBarGO.GetComponent<Slider>();
        button = buttonGO.GetComponent<Button>();
    }

    void OnDisable()
    {
        StateManager.OnStateChange -= UpdateUIText;
    }

    void FixedUpdate()
    {
        slider.value = (float)board.getBoardCompletionPercentage();
        if (slider.value == 1.0f)
        {
            button.interactable = true;
        }
    }

    void UpdateUIText(StateManager.State newState)
    {
        buttonTextMesh = buttonGO.transform.Find("ButtonCaption").GetComponent<TextMeshProUGUI>();
        instructionsTextMesh = hintGO.transform.Find("InstructionsText").GetComponent<TextMeshProUGUI>(); 
        scoreTextMesh = scoreGO.transform.Find("ScoreText").GetComponent<TextMeshProUGUI>(); 

    	instructionsTextMesh.text = (string)newState.instructionsText;
        buttonTextMesh.text = (string)newState.buttonText;

        // Update button
        if (string.IsNullOrEmpty(newState.buttonText))
        {
            buttonGO.SetActive(false);
        } 
        else
        {
            buttonGO.SetActive(true);
        }

        if (newState.name == StateName.Cards)
        {
            progressBarGO.SetActive(true);
            button.interactable = false;
        }
        else
        {
            progressBarGO.SetActive(false);
        }

        if (newState.name == StateName.Score)
        {
            hintGO.SetActive(false);
            scoreGO.SetActive(true);
            scoreTextMesh.text = board.getScoreText();
        }
        else
        {
            hintGO.SetActive(true);
            scoreGO.SetActive(false);
            scoreTextMesh.text = "";
        }
    }
}
