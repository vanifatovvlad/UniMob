namespace UniMob.Async
{
    public sealed class AsyncUnit
    {
        public static readonly AsyncUnit Unit = new AsyncUnit();

        private AsyncUnit()
        {
        }
    }
}