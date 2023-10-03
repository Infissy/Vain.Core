using Vain.Core.ComponentSystem;

namespace Vain.Singleton
{
    public class SingletonComponent<T> : Singleton<T> where T : Component
    {


        public SingletonComponent(T instance) : base(instance) {}
    }
}