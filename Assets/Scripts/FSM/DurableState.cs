using UnityEngine;

namespace NodeCanvas
{
    public class DurableState : State
    {
        public float Duration { get; protected set; } = 0;
        public float Elapsed  { get; protected set; } = 0;
        public bool IsDone    { get; protected set; } = true;

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

        public override void OnExit()
        {
            IsDone = true;
        }
    }
}