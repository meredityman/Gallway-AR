using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace StateLib
{

	public enum StateName 
	{
		Init = 0, 
		Board = 1, 
		Cards = 2, 
		Score = 3
	};

	public class StateManager
	{
		public struct State
		{
		    public StateName name;
		    public string instructionText;

		    public State(StateName name, string instructionText)
		    {
		        this.name = name;
		        this.instructionText = instructionText;
		    }
		}

	    public StateName activeState;
	    Dictionary<StateName, State> states = new Dictionary<StateName, State>();
	   	
	   	// Delegate
	    public delegate void StateChange(State newState);
	    public static event StateChange OnStateChange;
	    // 

	    public bool isIn(StateName stateName)
	    {
	    	return activeState == stateName;
	    }

	    public State getCurrentState()
	    {
	    	return states[this.activeState];
	    }

	    public void goToNextState()
	    {
	    	Array Arr = Enum.GetValues(typeof(StateName));
	        int j = Array.IndexOf(Arr, this.activeState) + 1;
	        
	    	this.activeState = (Arr.Length==j) ? (StateName)Arr.GetValue(0) : (StateName)Arr.GetValue(j);

	    	if (OnStateChange != null)
	    	{
	    		OnStateChange(getCurrentState());
	    	}
	    }

	    public StateManager()
	    {
	    	State state_init = new State(StateName.Init, "Please point to the camera to the board marker. Touch the button when you are ready");
	    	State state_board = new State(StateName.Board, "When you feel good about your arrangement – touch the button.");
	    	State state_cards = new State(StateName.Cards, "Slowly move camera over each card to secure them in their positions.");
	    	State state_score = new State(StateName.Score, "");

	    	states.Add(state_init.name, state_init);
	    	states.Add(state_board.name, state_board);
	    	states.Add(state_cards.name, state_cards);
	    	states.Add(state_score.name, state_score);

	    	activeState = state_init.name;
	    }
	}
}
