using System.Collections.Generic;
using System.Linq;

namespace ClArc.Lib {
    public static class TextJoiner
    {
        public static List<string> Join(IEnumerable<IEnumerable<string>> source, string sep = null)
        {
            if (source.Any())
            {
                var results = new List<string>(source.First());
                foreach (var lines in source.Skip(1))
                {
                    if (sep != null)
                    {
                        results.Add(sep);
                    }
                    results.AddRange(lines);
                }

                return results;
            }
            else
            {
                return new List<string>();
            }
        }
    }
}
