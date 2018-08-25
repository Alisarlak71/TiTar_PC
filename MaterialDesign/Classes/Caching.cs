using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Caching;
namespace MaterialDesign2.Classes
{
    public class Caching
    {
        static readonly ObjectCache Cache = MemoryCache.Default;
        public void Store_single_to_cache(object content,string key)
        {

            ObjectCache cache = MemoryCache.Default;
            cache.Add("singlecontent"+key , content, DateTime.Now.AddDays(30));
        }
        public static Content Get_single_from_cache<Content>(string key) where Content : class
        {
            try
            {
                return (Content)Cache[key];
            }
            catch
            {
                return null;
            }
        }
    }
}
