using Microsoft.Extensions.DependencyInjection;

namespace WebApplication.ClArc.DI
{
    interface IDILauncher
    {
        void Run(IServiceCollection services);
    }
}
