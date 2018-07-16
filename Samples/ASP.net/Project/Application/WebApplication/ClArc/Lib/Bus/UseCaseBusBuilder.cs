using Microsoft.Extensions.DependencyInjection;
using UseCase.Core;

namespace WebApplication.ClArc.Lib.Bus
{
    public class UseCaseBusBuilder
    {
        private readonly IServiceCollection services;
        private readonly UseCaseBus bus;

        public UseCaseBusBuilder(IServiceCollection services) {
            this.services = services;
            bus = new UseCaseBus();
        }

        public UseCaseBus Build() {
            var provider = ServiceCollectionContainerBuilderExtensions.BuildServiceProvider(services);
            var usecaseProvider = new UseCaseProvider(provider);
            bus.Setup(usecaseProvider);

            return bus;
        }

        public void RegisterUseCase<TRequest, TUseCase, TImplement>()
            where TUseCase : class, IUseCase<TRequest, IResponse>
            where TRequest : IRequest<IResponse>
            where TImplement : class, TUseCase {
            ServiceCollectionServiceExtensions.AddSingleton<TUseCase, TImplement>(services);
            bus.Register<TRequest, TUseCase>();
        }
    }
}
