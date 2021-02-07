using System.Linq;
using UnityEngine;

namespace NodeCanvas
{
    public class SubFSM : BaseFSM, INode
    {
        private readonly FSM ownerFSM;

        public bool IsFinished { get; private set; }

        public SubFSM(FSM ownerFSM, string name = "Untitled") : base(name)
        {
            this.ownerFSM = ownerFSM;
            this.ownerFSM.AddSubFSM(this);

            this.exitNode = new ActionState()
            {
                EnterAction = () =>
                {
                    this.IsFinished = true;
                    Debug.Log($"Exit {name}");
                }
            };
        }
        
        public void OnEnter()
        {
            entryNode.OnEnter();
            entryNode.OnUpdate();

            SetCurrentNode(entryNode);

            IsFinished = false;
            
            OnUpdate(); // advance one update
        }

        public void OnUpdate()
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
                
                CurrentNode.OnEnter();
            }
            
            currentNode.OnUpdate();
        }

        public void OnFixedUpdate()
        {
            currentNode.OnFixedUpdate();
        }

        public void OnExit()
        {
            IsFinished = true;
        }
        
        public override void SetEntry(INode node)
        {
            if (!nodeMap.ContainsKey(node))
            {
                Debug.Log($"{name}: entry node must be contained in this SubFSM.");
                return;
            }
            
            entryNode = node;
        }

        public override bool IsValidNode(INode node)
        {
            if (node == null)
            {
                Debug.Log($"{name}: node can not be NULL.");
                return false;
            }

            if (node == this)
            {
                Debug.Log($"{name}: can not reference itself.");
                return false;
            }
            
            if (node is SubFSM)
            {
                Debug.Log($"{name}: can not contain another SubFSM.");
                return false;
            }

            if (ownerFSM.Nodes.Contains(node))
            {
                Debug.Log($"{name}: node is already owned by OwnerFSM.");
                return false;
            }
            
            return true;
        }
    }
}