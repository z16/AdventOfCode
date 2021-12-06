using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Problems.Year2015;

internal static class Day7 {
	public static UInt16 Part1(String[] array) =>
		Check(Terms(array), new Dictionary<String, UInt16>());

	public static UInt16 Part2(String[] array) {
		var terms = Terms(array);
		return Check(terms, new Dictionary<String, UInt16>() { ["b"] = Check(terms, new Dictionary<String, UInt16>()) });
	}

	private static IReadOnlyDictionary<String, String> Terms(String[] array) =>
		array.Select(line => line.Split(" -> ")).ToDictionary(assignment => assignment[1], assignment => assignment[0]);

	private static UInt16 Check(IReadOnlyDictionary<String, String> terms, IDictionary<String, UInt16> values) =>
		Evaluate("a", terms, values);

	private static UInt16 Evaluate(String term, IReadOnlyDictionary<String, String> terms, IDictionary<String, UInt16> values) {
		var tokens = term.Split(' ');
		return (UInt16) (tokens.Length switch {
			1 => GetValue(tokens[0]),
			2 => tokens[0] switch {
				"NOT" => ~GetValue(tokens[1]),
			},
			3 => tokens[1] switch {
				"AND" => GetValue(tokens[0]) & GetValue(tokens[2]),
				"OR" => GetValue(tokens[0]) | GetValue(tokens[2]),
				"LSHIFT" => GetValue(tokens[0]) << GetValue(tokens[2]),
				"RSHIFT" => GetValue(tokens[0]) >> GetValue(tokens[2]),
			},
		});

		UInt16 GetValue(String token) {
			if (UInt16.TryParse(token, out var integer)) {
				return integer;
			}

			if (!values.TryGetValue(token, out var value)) {
				value = Evaluate(terms[token], terms, values);
				values[token] = value;
			}

			return value;
		}
	}

	private static UInt16 Apply(String line, IReadOnlyDictionary<String, UInt16> variables) {
		var tokens = line.Split(' ');
		return (UInt16) (tokens.Length switch {
			1 => GetValue(tokens[0], variables),
			2 => tokens[0] switch {
				"NOT" => ~GetValue(tokens[1], variables),
			},
			3 => tokens[1] switch {
				"AND" => GetValue(tokens[0], variables) & GetValue(tokens[2], variables),
				"OR" => GetValue(tokens[0], variables) | GetValue(tokens[2], variables),
				"LSHIFT" => GetValue(tokens[0], variables) << GetValue(tokens[2], variables),
				"RSHIFT" => GetValue(tokens[0], variables) >> GetValue(tokens[2], variables),
			},
		});

		static UInt16 GetValue(String token, IReadOnlyDictionary<String, UInt16> variables) =>
			UInt16.TryParse(token, out var integer)
				? integer
				: variables[token];
	}
}
