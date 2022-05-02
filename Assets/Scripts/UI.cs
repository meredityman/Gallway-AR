using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using StateLib;

public class UI : MonoBehaviour
{
    void OnEnable()
    {
        StateManager.OnStateChange += UpdateUIText;
    }

    void OnDisable()
    {
        StateManager.OnStateChange -= UpdateUIText;
    }

    void UpdateUIText(StateManager.State newState)
    {
    	Debug.Log("New State" + newState.name);
    }
}
