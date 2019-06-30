using System;

namespace UniMob.Async
{
    public class ReactionAtom : AtomBase
    {
        private readonly Action _reaction;
        private readonly Action<Exception> _exceptionHandler;

        public ReactionAtom(Action reaction, Action<Exception> exceptionHandler = null)
            : base(null, null)
        {
            _reaction = reaction ?? throw new ArgumentNullException(nameof(reaction));
            _exceptionHandler = exceptionHandler ?? Zone.Current.HandleUncaughtException;
        }

        public void Get()
        {
            Update();
        }

        protected override void Evaluate()
        {
            State = AtomState.Actual;

            try
            {
                _reaction();
            }
            catch (Exception exception)
            {
                _exceptionHandler(exception);
            }
        }

        public override string ToString()
        {
            return "[ReactionAtom]";
        }
    }
}