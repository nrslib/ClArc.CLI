using System;

namespace WebApplication.ClArc.Lib.Bus
{
    public interface IInjector {
        TInstance Resolve<TInstance>();
        object Resolve(Type type);
    }
}
