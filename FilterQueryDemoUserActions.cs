using System;
using System.Collections.Generic;

namespace FilterQueryDemo
{
    public class FilterQueryDemoUserActions : FilterQueryDemoActions
    {
        // Stores the start-symbol value so it can be easily used for grammar processing.
        private Query? _parseResult;

        private readonly Dictionary<string, object> _context;
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
            if (_parseResult is null)
            {
                throw new InvalidOperationException("No parsed query available. Call parser before evaluation.");
            }

            _lastResult = FilterQueryEvaluator.Evaluate(_parseResult, _context);
            return _lastResult.Value;
        }

        // Called when the start symbol has been parsed. Contains the processed input.
        public override void OnQuery(Query arg)
        {
            _parseResult = arg;
        }

        // Print the evaluated parse result if available.
        public override string ToString()
        {
            return _lastResult is bool result
                ? $"Evaluation result: {result.ToString().ToLowerInvariant()}"
                : "Grammar parsed successfully";
        }
    }
}
