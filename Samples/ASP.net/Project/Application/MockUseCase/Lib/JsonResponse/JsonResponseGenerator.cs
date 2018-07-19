using System;
using System.Collections.Concurrent;
using System.IO;

namespace MockUseCase.Lib.JsonResponse
{
    public class JsonResponseGenerator
    {
        private readonly ConcurrentDictionary<Type, object> files = new ConcurrentDictionary<Type, object>();
        private readonly string fileDirectory;

        public JsonResponseGenerator(string fileDirectory)
        {
            this.fileDirectory = fileDirectory;
        }

        public T Generate<T>()
        {
            var type = typeof(T);
            var jsonFile = (JsonResponseFile<T>) files.GetOrAdd(type, t =>
            {
                var path = Path.Combine(fileDirectory, $"{type.Name}.jsons");
                return new JsonResponseFile<T>(path);
            });
            return jsonFile.Next();
        }
    }
}
