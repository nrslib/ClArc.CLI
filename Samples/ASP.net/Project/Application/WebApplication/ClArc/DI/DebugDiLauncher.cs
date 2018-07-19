using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using MockUseCase.Lib.JsonResponse;
using WebApplication.ClArc.Lib.Bus;

namespace WebApplication.ClArc.DI
{
    public class DebugDiLauncher : IDILauncher
    {
        private readonly IHostingEnvironment env;

        public DebugDiLauncher(IHostingEnvironment env)
        {
            this.env = env;
        }

        public void Run(IServiceCollection services)
        {
            RegisterJsonGenerator(services);
            RegisterUseCaseBus(services);
        }

        private void RegisterJsonGenerator(IServiceCollection services)
        {
            var jsonResponseFullPath = Path.Combine(env.ContentRootPath, "Debug", "JsonResponses");
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
