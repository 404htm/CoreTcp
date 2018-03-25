namespace CoreTcp
{
    public class ClientFactory
    {
        public T CreateProxy<T>()
        {
            return default(T);
        }
    }
}