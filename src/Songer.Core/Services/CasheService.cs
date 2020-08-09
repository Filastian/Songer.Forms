using System.Collections.Generic;

namespace Songer.Core.Services
{
    public class CasheService : ICasheService
    {
        private readonly IDictionary<string, object> _cashe;

        public CasheService()
        {
            _cashe = new Dictionary<string, object>();
        }

        public void Put(string key, object entity)
        {
            if (_cashe.ContainsKey(key))
                _cashe[key] = entity;
            else
                _cashe.Add(key, entity);
        }

        public T Get<T>(string key)
        {
            if (!_cashe.ContainsKey(key)) return default(T);

            if (_cashe[key] is T value) return value;

            return default(T);
        }

        public void Delete(string key)
        {
            if (_cashe.ContainsKey(key))
                _cashe.Remove(key);
        }

        public void ClearCashe()
        {
            _cashe.Clear();
        }
    }
}
