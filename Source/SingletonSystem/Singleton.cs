namespace Vain.Singleton
{
    public class Singleton <T>
    {
        public Singleton(T instance)
        {
            Reference = instance;
        }
        public T Reference {get; internal set;}


    }


}