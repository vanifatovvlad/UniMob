namespace UniMob.ReView
{
    // ReSharper disable once InconsistentNaming
    public interface BuildContext
    {
        BuildContext Parent { get; }
        TState AncestorStateOfType<TState>() where TState : IState;
        TState RootAncestorStateOfType<TState>() where TState : IState;
    }
}