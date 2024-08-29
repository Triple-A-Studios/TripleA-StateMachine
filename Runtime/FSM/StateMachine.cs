using System;
using UnityEngine;
using System.Collections.Generic;

namespace TripleA.FSM
{
	public class StateMachine : MonoBehaviour
	{
		private StateNode m_currentNode;
		private Dictionary<Type, StateNode> m_nodes = new();
		private HashSet<ITransition> m_anyTransitions = new();

		public IState CurrentState => m_currentNode.State;

		#region Public Methods

		public void OnUpdate()
		{
			var transition = GetTransition();
			if (transition != null)
			{
				ChangeState(transition.To);
			}

			m_currentNode.State?.OnUpdate();
		}

		public void OnFixedUpdate()
		{
			m_currentNode.State?.OnFixedUpdate();
		}

		public void OnLateUpdate()
		{
			m_currentNode.State?.OnLateUpdate();
		}

		/// <summary>
		///		Sets the current state of the state machine.
		/// </summary>
		/// <param name="state">The state to set.</param>
		public void SetState(IState state)
		{
			m_currentNode = m_nodes[state.GetType()];
			m_currentNode.State?.OnEnter();
		}

		/// <summary>
		///		Adds a transition that can be triggered from any state to the specified state.
		/// </summary>
		/// <param name="to">The state to transition to.</param>
		/// <param name="condition">The condition that must be satisfied for the transition to occur.</param>
		public void AddAnyTransition(IState to, IPredicate condition)
		{
			m_anyTransitions.Add(new Transition(GetOrAddNode(to).State, condition));
		}

		/// <summary>
		///		Adds a transition from one state to another state with a specified condition.
		/// </summary>
		/// <param name="from">The state to transition from.</param>
		/// <param name="to">The state to transition to.</param>
		/// <param name="condition">The condition that must be satisfied for the transition to occur.</param>
		public void AddTransition(IState from, IState to, IPredicate condition)
		{
			GetOrAddNode(from).AddTransition(GetOrAddNode(to).State, condition);
		}

		#endregion

		#region Private Methods

		private void ChangeState(IState state)
		{
			if (m_currentNode.State == state) return;
			var previousState = m_currentNode.State;
			var nextState = m_nodes[state.GetType()].State;

			previousState?.OnExit();
			nextState?.OnEnter();
			m_currentNode = m_nodes[state.GetType()];
		}

		private ITransition GetTransition()
		{
			foreach (var transition in m_anyTransitions)
			{
				if (transition.Condition.Evaluate())
					return transition;
			}

			foreach (var transition in m_currentNode.Transitions)
			{
				if (transition.Condition.Evaluate())
					return transition;
			}

			return null;
		}

		private StateNode GetOrAddNode(IState state)
		{
			var node = m_nodes.GetValueOrDefault(state.GetType());
			if (node == null)
			{
				node = new StateNode(state);
				m_nodes.Add(state.GetType(), node);
			}
			return node;
		}

		#endregion

		private class StateNode
		{
			public IState State { get; }
			public HashSet<ITransition> Transitions { get; }

			public StateNode(IState state)
			{
				State = state;
				Transitions = new HashSet<ITransition>();
			}

			public void AddTransition(IState to, IPredicate condition)
			{
				Transitions.Add(new Transition(to, condition));
			}
		}
	}
}