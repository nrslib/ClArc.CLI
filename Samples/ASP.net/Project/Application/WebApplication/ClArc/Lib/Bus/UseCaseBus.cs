using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using UseCase.Core;

namespace WebApplication.ClArc.Lib.Bus
{
    public class UseCaseBus
    {
        private IInjector injector;
        private readonly Dictionary<Type, Type> handlerTypes = new Dictionary<Type, Type>();
        private readonly ConcurrentDictionary<Type, UseCaseInvoker> invokers = new ConcurrentDictionary<Type, UseCaseInvoker>();

        public void Setup(IInjector injector)
        {
            this.injector = injector;
        }

        public void Register<TRequest, TUseCase>()
            where TRequest : IRequest<IResponse>
            where TUseCase : IUseCase<TRequest, IResponse> {
            handlerTypes.Add(typeof(TRequest), typeof(TUseCase));
        }

        public TResponse Handle<TResponse>(IRequest<TResponse> query)
            where TResponse : IResponse
        {
            var invoker = Invoker(query);
            return invoker.Invoke<TResponse>(query);
        }

        public async Task<TResponse> HandleAync<TResponse>(IRequest<TResponse> query)
            where TResponse : IResponse {
            var invoker = Invoker(query);
            var result = await Task.Run(() => invoker.Invoke<TResponse>(query));
            return result;
        }

        private UseCaseInvoker Invoker<TResponse>(IRequest<TResponse> request)
            where TResponse : IResponse
        {
            var requestType = request.GetType();
            if (invokers.TryGetValue(requestType, out var searchedInvoker))
            {
                return searchedInvoker;
            }

            if (!handlerTypes.TryGetValue(requestType, out var handlerType))
            {
                throw new Exception($"No registered any usecase for this request(RequestType : {request.GetType().Name}");
            }

            var invoker = invokers.GetOrAdd(requestType, _ =>
            {
                var handlerInstance = injector.Resolve(handlerType);
                return new UseCaseInvoker(handlerType, handlerInstance.GetType(), injector);
            });

            return invoker;
        }
    }
}
