using Microsoft.AspNetCore.Hosting;

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
            
        public IDILauncher InstantiateLauncher(IHostingEnvironment env)
        {
            if (IsDevelop)
            {
                return new DebugDiLauncher(env);
            }
            else
            {
                return new ProductDiLauncher();
            }
        }
    }
}
