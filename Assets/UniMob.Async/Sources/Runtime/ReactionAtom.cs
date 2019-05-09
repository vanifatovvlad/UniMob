using System;

namespace UniMob.Async
{
    public class ReactionAtom : AtomBase
    {
        private readonly Action _reaction;

        public ReactionAtom(Action reaction) : base(null, null)
        {
            _reaction = reaction ?? throw new ArgumentNullException(nameof(reaction));
        }

        public void Get()
        {
            Update();
        }

        protected override void Evaluate()
        {
            State = AtomState.Actual;
            _reaction();
        }

        public override string ToString()
        {
            return "[ReactionAtom]";
        }
    }
}