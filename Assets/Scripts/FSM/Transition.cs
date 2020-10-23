using System;

namespace NodeCanvas
{
    public class Transition
    {
        public INode Source;
        public INode Destination;
        public Func<bool> Condition;

        public Transition()
        {
            this.Source      = null;
            this.Destination = null;
            this.Condition   = null;
        }
        
        public Transition(INode source, INode destination, Func<bool> condition)
        {
            this.Source      = source;
            this.Destination = destination;
            this.Condition   = condition;
        }

        public bool Contain(INode node)
        {
            if (Source == node || Destination == node)
                return true;

            return false;
        }
    }
}
