using Microsoft.Extensions.DependencyInjection;
using WebApplication.ClArc.Lib.Bus;

namespace WebApplication.ClArc.DI
{
    public class DebugDiLauncher : IDILauncher
    {
        public void Run(IServiceCollection services)
        {
            RegisterUseCaseBus(services);
        }

        private void RegisterUseCaseBus(IServiceCollection services)
        {
            var busBuilder = new UseCaseBusBuilder(services);

            RegisterUseCase(busBuilder);

            var usecaseBus = busBuilder.Build();
            services.AddSingleton(usecaseBus);
        }

        private void RegisterUseCase(UseCaseBusBuilder builder)
        {
            #region ClArc START
            #endregion ClArc END
        }
    }
}
