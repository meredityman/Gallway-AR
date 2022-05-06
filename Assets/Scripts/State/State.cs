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
			public string instructionsText;
			public string buttonText;

			public State(StateName name, string instructionsText, string buttonText)
			{
				this.name = name;
				this.instructionsText = instructionsText;
				this.buttonText = buttonText;
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
			return this.activeState == stateName;
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

		public void init()
		{
			this.activeState = StateName.Init;

			if (OnStateChange != null)
			{
				OnStateChange(getCurrentState());
			}
		}

		public StateManager()
		{
			State state_init = new State(StateName.Init, "Please point the camera to the board marker.", "Place board");
			State state_board = new State(StateName.Board, "When you feel good about your arrangement press the button.", "Evaluate board");
			State state_cards = new State(StateName.Cards, "Slowly move camera over each card to secure them in their positions.", "");
			State state_score = new State(StateName.Score, "", "Restart");

			states.Add(StateName.Init,  state_init);
			states.Add(StateName.Board, state_board);
			states.Add(StateName.Cards, state_cards);
			states.Add(StateName.Score, state_score);

			this.activeState = StateName.Init;

			if (OnStateChange != null)
			{
				OnStateChange(getCurrentState());
			}
		}
	}
}
