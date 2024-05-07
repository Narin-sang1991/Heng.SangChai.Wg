using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace Cet.Core
{
    public static class EnumTranslationExtension
    {
        public static IList<Tuple<Enum, string>> CreateTranslatedTuples<T>()
        {
            List<Tuple<Enum, string>> list = new List<Tuple<Enum, string>>();
            foreach (var x in Enum.GetValues(typeof(T)))
            {
                list.Add(new Tuple<Enum, string>((Enum)x, ((Enum)x).Translate()));
            }
            return list;
        }

        public static string Translate(this Enum value)
        {
            ResourceManager manager = new System.Resources.ResourceManager(value.GetType().Namespace + ".Resources.EnumMessages", value.GetType().GetTypeInfo().Assembly);
            string key = value.GetType().Name + "_" + value.ToString();
            var y = manager.GetString(key);
            return y == null ? key : y;
        }

    }
}
