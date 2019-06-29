namespace UniMob.ReView
{
    public abstract partial class State
    {
        public virtual TState AncestorStateOfType<TState>()
            where TState : IState
        {
            var ancestor = Context;

            while (ancestor != null)
            {
                if (ancestor is TState state)
                {
                    return state;
                }

                ancestor = ancestor.Parent;
            }

            return default;
        }

        public virtual TState RootAncestorStateOfType<TState>()
            where TState : IState
        {
            var ancestor = Context;

            TState root = default;
            while (ancestor != null)
            {
                if (ancestor is TState state)
                {
                    root = state;
                }

                ancestor = ancestor.Parent;
            }

            return root;
        }
    }
}