using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace NodeCanvas
{
    public class FSM : BaseFSM
    {
        public FSM(string name = "Untitled") : base(name)
        {
            this.AddTransition(currentNode, entryNode, () => true);
        }

        public void Update()
        {
            var qualifiedTransition = CheckForQualifiedTransition();
            
            if (qualifiedTransition != null)
            {
                currentNode.OnExit();
                
                if (qualifiedTransition.Destination == PreviousNode && nodeStack.Count >= 2)
                {
                    nodeStack.Pop();
                    qualifiedTransition.Destination = nodeStack.Pop();
                }
                
                SetCurrentNode(qualifiedTransition.Destination);
                
                currentNode.OnEnter();
            }
            
            currentNode.OnUpdate();
        }
        
        public void FixedUpdate()
        {
            currentNode.OnFixedUpdate();
        }

        public void AddSubFSM(SubFSM subFSM)
        {
            if (nodeMap.ContainsKey(subFSM))
            {
                Debug.Log($"{name}: already contains {subFSM.Name}.");
                return;
            }
            
            nodeMap.Add(subFSM, new HashSet<Transition>());
        }
        
        public override void SetEntry(INode node)
        {
            if (!nodeMap.ContainsKey(node))
            {
                Debug.Log($"{name}: entry node must be contained in this FSM.");
                return;
            }
            
            entryNode = node;

            Transition t = nodeMap[currentNode].FirstOrDefault();

            if (t != null)
            {
                t.Destination = entryNode;
                currentTransitionSet = nodeMap[currentNode];
            }
        }

        public override bool IsValidNode(INode node)
        {
            if (node == null)
            {
                Debug.Log($"{name}: node can not be NULL.");
                return false;
            }
            
            // check if node is owned by any SubFSM
            IEnumerable<SubFSM> subs = from n in Nodes 
                                       where n is SubFSM 
                                       select n as SubFSM;

            var subFSM = subs.FirstOrDefault(sub => sub.Nodes.Contains(node));

            if (subFSM != null)
            {
                Debug.Log($"{name}: node is already owned by {subFSM}.");
                return false;
            }
            
            return true;
        }
    }
}