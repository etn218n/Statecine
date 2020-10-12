using System;

namespace Node
{
    public class Transition
    {
        public State Source;
        public State Destination;
        public Func<bool> Condition;
        
        public Transition() { }
        
        public Transition(State source, State destination, Func<bool> condition)
        {
            this.Source      = source;
            this.Destination = destination;
            this.Condition   = condition;
        }
    }
}