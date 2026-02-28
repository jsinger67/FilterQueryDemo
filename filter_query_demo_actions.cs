using System;
using System.Collections.Generic;
using Parol.Runtime;

namespace FilterQueryDemo
{
    public partial class FilterQueryDemoActions : IFilterQueryDemoActions
    {
        private readonly Dictionary<string, object> _context;
        private bool? _lastResult;

        public FilterQueryDemoActions()
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

        public bool EvaluateInput(string input)
        {
            _lastResult = FilterQueryEvaluator.Evaluate(input, _context);
            return _lastResult.Value;
        }

        public override string ToString()
        {
            return _lastResult is bool result
                ? $"Evaluation result: {result.ToString().ToLowerInvariant()}"
                : "Grammar parsed successfully";
        }
    }
}
