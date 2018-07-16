using System;

namespace WebApplication.ClArc.Lib.Bus
{
    public class UseCaseProvider : IInjector
    {
        private readonly IServiceProvider serviceProvider;

        public UseCaseProvider(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public TInstance Resolve<TInstance>()
        {
            return (TInstance)serviceProvider.GetService(typeof(TInstance));
        }

        public object Resolve(Type type)
        {
            return serviceProvider.GetService(type);
        }
    }
}
