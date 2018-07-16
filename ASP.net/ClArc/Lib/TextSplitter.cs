using System;
using System.Collections.Generic;

namespace ClArc.Lib {
    public static class TextSplitter
    {
        public static (List<string>, List<string>) DivideInto2(
            IEnumerable<string> source,
            Func<string, bool> selector,
            bool containToFront = true)
        {
            var front = new List<string>();
            var rear = new List<string>();
            var current = front;

            foreach (var line in source)
            {
                if (selector(line))
                {
                    if (containToFront)
                    {
                        current.Add(line);
                        current = rear;
                    }
                    else
                    {
                        current = rear;
                        current.Add(line);
                    }
                }
                else
                {
                    current.Add(line);
                }
            }

            return (front, rear);
        }

        public static (List<string>, List<string>, List<string>) DivideIntoThree(
            IEnumerable<string> source,
            Func<string, bool> startSelector,
            Func<string, bool> endSelector,
            bool isContainsSelect = true
        )
        {
            var fronts = new List<string>();
            var middle = new List<string>();
            var rear = new List<string>();
            var current = fronts;

            var isPushing = false;
            foreach (var line in source)
            {
                if (!isPushing && startSelector(line))
                {
                    isPushing = true;
                    if (isContainsSelect)
                    {
                        current = middle;
                        current.Add(line);
                    }
                    else
                    {
                        current.Add(line);
                        current = middle;
                    }

                }
                else if (isPushing && endSelector(line))
                {
                    if (isContainsSelect)
                    {
                        current.Add(line);
                        current = rear;
                    }
                    else
                    {
                        current = rear;
                        current.Add(line);
                    }
                }
                else
                {
                    current.Add(line);
                }
            }

            return (fronts, middle, rear);
        }

        public static List<List<string>> Divide(IEnumerable<string> source,
            Func<string, bool> divineSelector,
            bool isContainsToFront = true,
            bool ignoreUntilFirstSelected = false
        )
        {
            var results = new List<List<string>>();
            var current = new List<string>();
            if (!ignoreUntilFirstSelected)
            {
                results.Add(current);
            }

            foreach (var line in source)
            {
                if (divineSelector(line))
                {
                    if (isContainsToFront)
                    {
                        current.Add(line);
                    }
                    current = new List<string>();
                    results.Add(current);
                    if (!isContainsToFront)
                    {
                        current.Add(line);
                    }
                }
                else
                {
                    current.Add(line);
                }
            }

            return results;
        }
    }
}
