using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using TMPro;

using StateLib;

public class World : MonoBehaviour
{
    public StateManager stateManager;
    private GameObject worldImageTarget;

    public GameObject boardGO;
    public GameObject cardsGO;

    void OnEnable()
    {   
        worldImageTarget = GameObject.Find("WorldImageTarget");

        // Create state manager, set to INIT state.
        stateManager = new StateManager();
        // stateManager.init();
        Debug.Log(stateManager);
        Debug.Log(stateManager == null);
        Debug.Log(stateManager.activeState);

        // Subscribe to state change
        StateManager.OnStateChange += HandleStateChange;
    }

    void OnDisable()
    {
        StateManager.OnStateChange -= HandleStateChange;
    }

    public void OnBoardTargetDetected()
    {
        Debug.Log("Target Detected");
        // Debug.Log(stateManager);

        // if (stateManager.isIn(StateName.Init))
        // {   
        //     worldImageTarget.SetActive(false);
        //     Debug.Log("Deactivated Target");
        //     stateManager.goToNextState();
        // }
    }

    public void handleUIClick()
    {   
        // Add security precautions
        this.stateManager.goToNextState();
    }

    void FixedUpdate()
    {
        if (stateManager.isIn(StateName.Cards))
        {
            if (boardGO.GetComponent<BoardObject>().IsBoardFull())
            {
                this.stateManager.goToNextState();
            }
        }
    }

    void HandleStateChange(StateManager.State newState)
    {
        switch(newState.name)
        {
            case StateName.Init:
                worldImageTarget.SetActive(true);
                boardGO.SetActive(false);
                cardsGO.SetActive(false);
                break;
            case StateName.Board:
                //worldImageTarget.SetActive(false);
                boardGO.SetActive(true);
                cardsGO.SetActive(true);
                break;
            case StateName.Cards:
                boardGO.SetActive(true);
                cardsGO.SetActive(true);
                break;
            case StateName.Score:
                boardGO.SetActive(true);
                cardsGO.SetActive(true);
                break;
        }
    }
}
