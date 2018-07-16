using System;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using MockUseCase.Lib.JsonResponse;
using WebApplication.ClArc.Lib.Bus;

namespace WebApplication.ClArc.DI
{
    public class DebugDiLauncher : IDILauncher
    {
        public void Run(IServiceCollection services)
        {
            RegisterJsonGenerator(services);
            RegisterUseCaseBus(services);
        }

        private void RegisterJsonGenerator(IServiceCollection services)
        {
            var webProjectFullPath = AppDomain.CurrentDomain.BaseDirectory;
            var jsonResponseFullPath = Path.Combine(webProjectFullPath, "Debug", "JsonResponses");
            var generator = new JsonResponseGenerator(jsonResponseFullPath);
            services.AddSingleton(generator);
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
