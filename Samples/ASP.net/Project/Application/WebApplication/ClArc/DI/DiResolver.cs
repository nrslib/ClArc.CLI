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

        public bool IsDevelop { get; set; }
            
        public IDILauncher InstantiateLauncher()
        {
            if (IsDevelop)
            {
                return new DebugDiLauncher();
            }
            else
            {
                return new ProductDiLauncher();
            }
        }
    }
}
