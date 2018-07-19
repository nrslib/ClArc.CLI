using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace MockUseCase.Lib.JsonResponse
{
    public class JsonResponseFile<T>
    {
        private readonly JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new PrivateSetterContractResolver()
        };

        private int now;
        private readonly string filePath;

        public JsonResponseFile(string filePath)
        {
            this.filePath = filePath;
        }

        public T Next()
        {
            if (!File.Exists(filePath))
            {
                return createAnyInstance();
            }

            var content = File.ReadAllText(filePath);
            var jsons = splitJsons(content);
            var targetJson = now < jsons.Count ? jsons[now] : jsons.LastOrDefault();
            now++;

            var instance = targetJson == null
                ? createAnyInstance()
                : JsonConvert.DeserializeObject<T>(targetJson, jsonSerializerSettings);

            return instance;
        }

        private T createAnyInstance()
        {
            var instanceType = typeof(T);
            var constructorInfo = instanceType.GetConstructors().First();
            var defaultParameters = constructorInfo.GetParameters()
                .Select(x => x.ParameterType)
                .Select(createDefaultType);
            var instance = (T)constructorInfo.Invoke(defaultParameters.ToArray());
            return instance;
        }

        private object createDefaultType(Type t)
        {
            if (t.IsValueType)
            {
                return Activator.CreateInstance(t);
            }

            return null;
        }

        private List<string> splitJsons(string content)
        {
            var acc = new List<string>();
            var sb = new StringBuilder();
            var nest = 0;
            foreach (var c in content)
            {
                sb.Append(c);
                if (c == '{')
                {
                    nest++;
                }
                else if (c == '}')
                {
                    nest--;
                    if (nest == 0)
                    {
                        acc.Add(sb.ToString());
                        sb = new StringBuilder();
                    }
                }
            }
            return acc;
        }
    }
}
