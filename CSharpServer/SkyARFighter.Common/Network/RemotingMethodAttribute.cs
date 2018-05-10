using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SkyARFighter.Common.Network
{
    public class ServiceContractAttribute : Attribute
    {
        public ServiceContractAttribute()
        {
        }

        public Type Callback
        {
            get; set;
        }
    }

    public class RemotingMethodAttribute : Attribute
    {
        static RemotingMethodAttribute()
        {
            var ass = Assembly.GetCallingAssembly();
            var types = new List<Type>();
            foreach (var t in ass.GetTypes())
            {
                var attrs = t.GetCustomAttributes(typeof(ServiceContractAttribute), false) as ServiceContractAttribute[];
                if (attrs.Length > 0)
                {
                    types.Add(t);
                    types.Add(attrs[0].Callback);
                }
            }
            foreach (var t in types)
            {
                var dic = new Dictionary<string, int>();
                foreach (var mi in t.GetMethods())
                {
                    var attrRs = mi.GetCustomAttributes(typeof(RemotingMethodAttribute), false) as RemotingMethodAttribute[];
                    if (attrRs.Length > 0)
                    {
                        dic[mi.ToString()] = attrRs[0].RemoteMethodId;
                    }
                }
                ServiceRemotingMethodIds[t] = dic;
            }
        }
        public RemotingMethodAttribute(int id)
        {
            RemoteMethodId = id;
        }
        public int RemoteMethodId
        {
            get; private set;
        }

        public static Dictionary<int, MethodInfo> GetTypeMethodsMapping(params Type[] types)
        {
            var methods = new Dictionary<int, MethodInfo>();
            foreach (var type in types)
            {
                var interfaces = type.GetInterfaces();
                foreach (var method in type.GetMethods())
                {
                    foreach (var inf in interfaces)
                    {
                        if (ServiceRemotingMethodIds.TryGetValue(inf, out Dictionary<string, int> dic))
                        {
                            if (dic.TryGetValue(method.ToString(), out int remoteId))
                                methods[remoteId] = method;
                        }
                    }
                }
            }
            return methods;
        }

        private static Dictionary<Type, Dictionary<string, int>> ServiceRemotingMethodIds = new Dictionary<Type, Dictionary<string, int>>();
    }
}
