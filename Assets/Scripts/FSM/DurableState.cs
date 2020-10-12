using UnityEngine;

namespace Node
{
    public class DurableState : State
    {
        public float Duration { get; protected set; }
        public float Elapsed  { get; protected set; }
        public bool  IsDone   { get; protected set; }

        protected DurableState(float duration)
        {
            this.Duration = duration;
        }
    
        public override void OnEnter()
        {
            Elapsed = 0;
            IsDone  = false;
        }
    
        public override void OnUpdate()
        {
            if (Elapsed > Duration)
                IsDone = true;

            Elapsed += Time.deltaTime;
        }
    }
}