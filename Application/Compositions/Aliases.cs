using Microsoft.Practices.Unity;

namespace Application.Compositions
{
    internal class Singleton : ContainerControlledLifetimeManager {}

    internal class Resolve : PerResolveLifetimeManager {}

    internal class Constructor : InjectionConstructor
    {
        public Constructor(params object[] parameterValues) : base(parameterValues) {}
    }

    internal class Method : InjectionMethod
    {
        public Method(string methodName, params object[] methodParameters) : base(methodName, methodParameters) {}
    }

    internal class Parameter<T> : ResolvedParameter<T>
    {
        public Parameter() {}
        public Parameter(string name) : base(name) {}
    }

    internal class Array<T> : ResolvedArrayParameter<T>
    {
        public Array(params object[] elementValues) : base(elementValues) {}
    }
}