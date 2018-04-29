using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SkyARFighter.Common
{
    public sealed class JsonHelper
    {
        public static object ParseJsonObject(JToken token, Type objType)
        {
            object obj = null;
            if (token is JObject)
                obj = JsonConvert.DeserializeObject(token.ToString(), objType);
            else if (token is JValue)
            {
                var v = ((JValue)token).Value;
                if (v != null)
                    obj = Convert.ChangeType(((JValue)token).Value.ToString(), objType);
            }
            else if (token is JArray)
            {
                var arr = Array.CreateInstance(objType.GetElementType(), token.Children().Count());
                int i = 0;
                foreach (var t in token.Children())
                    arr.SetValue(ParseJsonObject(t, objType.GetElementType()), i++);
                obj = arr;
            }
            return obj;
        }

        public static object[] ParseMethodParameters(MethodInfo mi, string content)
        {
            var argVals = new List<JToken>();
            var jarray = JsonConvert.DeserializeObject(content) as JArray;
            foreach (JToken t in jarray.Children())
                argVals.Add(t);
            var ps = mi.GetParameters();
            if (argVals.Count != ps.Length)
                throw new Exception($"方法'{mi.Name}'的参数数量({ps.Length})与数据解析参数数量({argVals.Count})不一致！");

            List<object> args = new List<object>();
            for (int i = 0; i < argVals.Count; ++i)
            {
                var pi = ps[i];
                object obj = ParseJsonObject(argVals[i], pi.ParameterType);
                args.Add(obj);
            }
            return args.ToArray();
        }
    }
}
