using System;
using System.Reflection;
using UseCase.Core;

namespace WebApplication.ClArc.Lib.Bus
{
    public class UseCaseInvoker
    {
        private readonly Type usecaseType;
        private readonly IInjector injector;
        private readonly MethodInfo handleMethod;

        public UseCaseInvoker(Type usecaseType, Type implementsType, IInjector injector)
        {
            this.usecaseType = usecaseType;
            this.injector = injector;

            handleMethod = implementsType.GetMethod("Handle");
        }

        public TResponse Invoke<TResponse>(object request)
            where TResponse : IResponse
        {
            var instance = injector.Resolve(usecaseType);

            object responseObject;
            try
            {
                responseObject = handleMethod.Invoke(instance, new[] {request});
            } 
            catch (TargetInvocationException e)
            {
                throw e.InnerException;
            }

            var response = (TResponse) responseObject;

            return response;
        }
    }
}
