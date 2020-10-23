using System;

namespace NodeCanvas
{
    public class ActionState : INode
    {
        public Action EnterAction  = () => { };
        public Action UpdateAction = () => { };
        public Action FixedUpdateAction = () => { };
        public Action ExitAction= () => { };

        public void OnEnter()  => EnterAction();
        public void OnUpdate() => UpdateAction();
        public void OnFixedUpdate() => FixedUpdateAction();
        public void OnExit() => ExitAction();
    }
}