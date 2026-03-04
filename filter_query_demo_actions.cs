using System;
using System.Collections.Generic;

namespace FilterQueryDemo
{
    public class FilterQueryDemoUserActions : FilterQueryDemoActions
    {
        private readonly Dictionary<string, object> _context;
        private Query? _query;
        private bool? _lastResult;

        public FilterQueryDemoUserActions()
        {
            _context = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
            {
                ["status"] = "open",
                ["priority"] = 2d,
                ["owner"] = "sam",
                ["archived"] = false,
                ["score"] = 91.5d,
            };
        }

        public bool EvaluateParsedQuery()
        {
            if (_query is null)
            {
                throw new InvalidOperationException("No parsed query available. Call parser before evaluation.");
            }

            _lastResult = FilterQueryEvaluator.Evaluate(_query, _context);
            return _lastResult.Value;
        }

        public override void OnQuery(Query arg)
        {
            _query = arg;
        }

        public override string ToString()
        {
            return _lastResult is bool result
                ? $"Evaluation result: {result.ToString().ToLowerInvariant()}"
                : "Grammar parsed successfully";
        }
    }
}
