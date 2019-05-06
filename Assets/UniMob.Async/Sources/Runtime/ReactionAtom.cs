using System;

namespace UniMob.Async
{
    public class ReactionAtom : AtomBase
    {
        private readonly Action m_reaction;

        public ReactionAtom(Action reaction) : base(null, null)
        {
            m_reaction = reaction ?? throw new ArgumentNullException(nameof(reaction));
        }

        public void Get()
        {
            Update();
        }

        protected override void Evaluate()
        {
            State = AtomState.Actual;
            m_reaction();
        }

        public override string ToString()
        {
            return "[ReactionAtom]";
        }
    }
}