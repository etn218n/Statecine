namespace NodeCanvas
{
    public abstract class State : INode
    {
        public virtual void OnEnter() { }
        public virtual void OnUpdate() { }
        public virtual void OnFixedUpdate() { }
        public virtual void OnExit() { }
    }
}