namespace Songer.Core.Services
{
    public interface ICasheService
    {
        void Delete(string key);
        T Get<T>(string key);
        void Put(string key, object entity);

        void ClearCashe();
    }
}