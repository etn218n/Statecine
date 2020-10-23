using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace NodeCanvas
{
    public abstract class BaseFSM
    {
        protected HashSet<Transition> anyTransitonSet = new HashSet<Transition>();
        protected HashSet<Transition> currentTransitionSet = new HashSet<Transition>();

        protected readonly Stack<INode> nodeStack = new Stack<INode>();
        protected readonly Dictionary<INode, HashSet<Transition>> nodeMap = new Dictionary<INode, HashSet<Transition>>();
        
        protected AnyNode anyNode = new AnyNode();
        protected PreviousNode previousNode = new PreviousNode();

        protected INode entryNode   = new EmptyNode();
        protected INode exitNode    = new EmptyNode();
        protected INode currentNode = new EmptyNode();

        protected string name = string.Empty;
        
        public IReadOnlyCollection<Transition> CurrentTransitionSet => currentTransitionSet;
        public IReadOnlyCollection<Transition> AnyTransitonSet => anyTransitonSet;
        public IReadOnlyCollection<INode> NodeStack => nodeStack;
        public IReadOnlyCollection<INode> Nodes => nodeMap.Keys;
        public IReadOnlyCollection<Transition> TransitionsFrom(INode node) => nodeMap[node];
        
        public AnyNode AnyNode => anyNode;
        public PreviousNode PreviousNode => previousNode;

        public INode EntryNode   => entryNode;
        public INode ExitNode    => exitNode;
        public INode CurrentNode => currentNode;

        public string Name => name;

        protected BaseFSM(string name)
        {
            this.name = name;
        }

        protected Transition CheckForQualifiedTransition()
        {
            var qualifiedTransition = anyTransitonSet.FirstOrDefault(t => t.Condition() && t.Destination != currentNode);

            if (qualifiedTransition == null)
                qualifiedTransition = currentTransitionSet.FirstOrDefault(t => t.Condition() && t.Destination != currentNode);

            return qualifiedTransition;
        }
        
        public void AddTransition(INode source, INode destination, Func<bool> condition)
        {
            var transition = new Transition(source, destination, condition);

            if (!IsValidTransition(transition))
                return;
            
            if (transition.Source == AnyNode)
            {
                HandleAnyNode(transition);
                return;
            }
            
            if (!nodeMap.ContainsKey(transition.Source))
                nodeMap.Add(transition.Source, new HashSet<Transition>());

            if (!nodeMap.ContainsKey(transition.Destination))
                nodeMap.Add(transition.Destination, new HashSet<Transition>());
            
            nodeMap[transition.Source].Add(transition);
        }
        
        protected void SetCurrentNode(INode node)
        {
            currentNode = node;
            
            nodeStack.Push(currentNode);
            
            currentTransitionSet = nodeMap[currentNode];
            
            foreach (var transition in anyTransitonSet)
                transition.Source = currentNode;
        }

        protected void HandleAnyNode(Transition transition)
        {
            anyTransitonSet.Add(transition);
            
            if (!nodeMap.ContainsKey(transition.Destination))
                nodeMap.Add(transition.Destination, new HashSet<Transition>());
        }

        protected bool IsValidTransition(Transition transition)
        {
            if (!IsValidNode(transition.Source))
                return false;
            
            if (!IsValidNode(transition.Destination))
                return false;

            if (transition.Source is AnyNode)
            {
                if (transition.Source != anyNode)
                {
                    Debug.Log($"{name}: AnyNode does not belong to this FSM.");
                    return false;
                }
            }
            
            if (transition.Destination is PreviousNode)
            {
                if (transition.Destination != previousNode)
                {
                    Debug.Log($"{name}: PreviousNode does not belong to this FSM.");
                    return false;
                }
            }

            if (transition.Source is PreviousNode)
            {
                Debug.Log($"{name}: transition source can not be FSM.PreviousNode.");
                return false;
            }
            
            if (transition.Destination is AnyNode)
            {
                Debug.Log($"{name}: transition destination can not be FSM.AnyNode.");
                return false;
            }

            return true;
        }
        
        public abstract void SetEntry(INode node);
        public abstract bool IsValidNode(INode node);
    }
}