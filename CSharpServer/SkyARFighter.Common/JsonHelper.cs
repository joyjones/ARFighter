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
            var argVals = new Dictionary<string, JToken>();
            var jobj = JsonConvert.DeserializeObject(content) as JObject;
            foreach (var t in jobj.Children())
            {
                if (t is JProperty tp)
                    argVals[tp.Name] = tp.Value;
            }
            var ps = mi.GetParameters();
            if (argVals.Count != ps.Length)
                throw new Exception($"方法'{mi.Name}'的参数数量({ps.Length})与数据解析参数数量({argVals.Count})不一致！");
            var mtdVals = new Dictionary<string, ParameterInfo>();
            foreach (var p in ps)
                mtdVals[p.Name] = p;

            List<object> args = new List<object>();
            foreach (var kt in argVals)
            {
                var pi = mtdVals[kt.Key];
                object obj = ParseJsonObject(kt.Value, pi.ParameterType);
                args.Add(obj);
            }
            return args.ToArray();
        }
    }
}
