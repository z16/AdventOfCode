using System;
using System.Collections.Generic;
using System.Linq;
using z16.Core;

namespace AdventOfCode.Problems.Year2015;

internal static class Day15 {
	public static Int32 Part1(String[] array) =>
		Check(array, _ => true);

	public static Int32 Part2(String[] array) =>
		Check(array, partition => partition.Sum(ingredient => ingredient.Count * ingredient.Value.Properties["calories"]) == 500);

	private static Int32 Check(String[] array, Func<(Line Value, Int32 Count)[], Boolean> filter) =>
		array
			.Select(line => (Value: Line.Parse(line), Count: 0))
			.Let(partition => Iterate(partition.ToArray(), 100))
			.Where(filter)
			.Max(partition => partition[0].Value.Properties.Keys
				.Where(property => property != "calories")
				.Multiply(property => Math.Max(0, partition.Sum(ingredient => ingredient.Count * ingredient.Value.Properties[property])))
			);

	private static IEnumerable<(Line Value, Int32 Count)[]> Iterate((Line Value, Int32 Count)[] partition, Int32 total, Int32 start = 0) {
		var next = start + 1;
		if (next == partition.Length) {
			partition[start].Count = total;
			yield return partition;
			yield break;
		}

		partition[start].Count = total;
		for (var i = start + 1; i < partition.Length; ++i) {
			partition[i].Count = 0;
		}

		while (partition[start].Count >= 0) {
			foreach (var inner in Iterate(partition, total - partition[start].Count, next)) {
				yield return inner;
			}
			--partition[start].Count;
		}

		partition[start].Count = 0;
	}

	private readonly record struct Line(String Name, IReadOnlyDictionary<String, Int32> Properties) {
		public static Line Parse(String line) =>
			line.Split(": ").Let(split => new Line(split[0], split[1].Split(", ").Select(term => term.Split(' ')).ToDictionary(tokens => tokens[0], tokens => Int32.Parse(tokens[1]))));
	}
}
