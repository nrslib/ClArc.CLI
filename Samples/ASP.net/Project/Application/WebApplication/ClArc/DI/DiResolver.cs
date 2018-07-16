namespace WebApplication.ClArc.DI
{
    class DiResolver
    {
        private DiResolver()
        {
        }

        static DiResolver()
        {
            Instance = new DiResolver();
        }

        public static DiResolver Instance { get; }

        public IDILauncher InstantiateLauncher()
        {
            return new DebugDiLauncher();
        }
    }
}
