using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using UnityEngine;

namespace Node
{
    public class FSM : INode
    {
        private HashSet<Transition> currentTransitionSet  = new HashSet<Transition>();
        private HashSet<Transition> anyStateTransitionSet = new HashSet<Transition>();

        private readonly Stack<State> stateStack = new Stack<State>();
        private readonly Dictionary<State, HashSet<Transition>> stateMap = new Dictionary<State, HashSet<Transition>>();

        public State CurrentState { get; private set; } = new EmptyState();

        public static State AnyState      { get; } = new EmptyState();
        public static State PreviousState { get; } = new EmptyState();
        
        public void Update()
        {
            CheckTransitions();
            
            CurrentState.OnUpdate();
        }

        public void FixedUpdate()
        {
            CurrentState.OnFixedUpdate();
        }

        private void CheckTransitions()
        {
            var qualifiedTransition = anyStateTransitionSet.FirstOrDefault(t => t.Condition() && t.Destination != CurrentState);

            if (qualifiedTransition == null)
                qualifiedTransition = currentTransitionSet.FirstOrDefault(t => t.Condition() && t.Destination != CurrentState);

            if (qualifiedTransition != null)
            {
                CurrentState.OnExit();
                
                if (qualifiedTransition.Destination == PreviousState && stateStack.Count >= 2)
                {
                    stateStack.Pop();
                    qualifiedTransition.Destination = stateStack.Pop();
                }
                
                CurrentState = qualifiedTransition.Destination;
                CurrentState.OnEnter();
                
                stateStack.Push(CurrentState);
                currentTransitionSet = stateMap[CurrentState];

                foreach (var transition in anyStateTransitionSet)
                    transition.Source = CurrentState;
            }
        }

        public void AddTransition(State source, State destination, Func<bool> condition)
        {
            var transition = new Transition(source, destination, condition);

            if (!IsValidTransition(transition))
                return;
            
            if (ContainSpecialState(transition))
            {
                HandleSpecialState(transition);
                return;
            }
            
            if (!stateMap.ContainsKey(transition.Source))
                stateMap.Add(transition.Source, new HashSet<Transition>());

            if (!stateMap.ContainsKey(transition.Destination))
                stateMap.Add(transition.Destination, new HashSet<Transition>());
            
            stateMap[transition.Source].Add(transition);
        }

        private bool IsValidTransition(Transition transition)
        {
            if (transition.Source == null)
            {
                Debug.Log($"FSM: transition source can not be NULL");
                return false;
            }
            
            if (transition.Destination == null)
            {
                Debug.Log($"FSM: transition destination can not be NULL");
                return false;
            }
            
            if (transition.Source == PreviousState)
            {
                Debug.Log($"FSM: transition source can not be set to FSM.PreviousState");
                return false;
            }
            
            if (transition.Destination == AnyState)
            {
                Debug.Log($"FSM: transition destination can not be set to FSM.AnyState");
                return false;
            }

            return true;
        }

        private bool ContainSpecialState(Transition transition)
        {
            if (transition.Source == AnyState)
                return true;

            return false;
        }

        private void HandleSpecialState(Transition transition)
        {
            anyStateTransitionSet.Add(transition);
            
            if (!stateMap.ContainsKey(transition.Destination))
                stateMap.Add(transition.Destination, new HashSet<Transition>());
        }

        public int CountTransitionFrom(State source) => stateMap[source].Count;
        
        public void SetEntry(State state)
        {
            if(!stateMap.ContainsKey(state))
                return;
            
            if (CurrentState is EmptyState)
            {
                stateStack.Push(state);
                CurrentState = stateStack.Peek();
                CurrentState.OnEnter();
                currentTransitionSet = stateMap[state];
            
                foreach (var transition in anyStateTransitionSet)
                    transition.Source = CurrentState;
            }
        }

        public string HistoryString()
        {
            StringBuilder builder = new StringBuilder();
            
            foreach (var state in stateStack.Reverse())
            {
                builder.Append(state.ToString() + "-> ");
            }

            return builder.ToString();
        }

        public string MapString()
        {
            StringBuilder builder = new StringBuilder();

            foreach (var key in stateMap.Keys)
            {
                builder.Append($"{key} -> ");
                
                foreach (var transition in stateMap[key])
                {
                    builder.Append(transition.Destination + ", ");
                }

                builder.Remove(builder.Length - 2, 2);

                builder.Append("\n");
            }
            
            return builder.ToString();
        }
    }

    public class EmptyState :State { }
}