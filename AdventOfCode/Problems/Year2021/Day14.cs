using System;
using System.Collections.Generic;
using System.Linq;
using z16.Core;
using static MoreLinq.Extensions.WindowExtension;

namespace AdventOfCode.Problems.Year2021;

internal static class Day14 {
	public static Decimal Part1(String[] array) =>
		Check(array, 10);

	public static Decimal Part2(String[] array) =>
		Check(array, 40);

	private static Decimal Check(String[] array, Int32 steps) =>
		array
			.Let(lines => array
				.Skip(2)
				.Select(line => line.Split(" -> "))
				.ToDictionary(split => split[0], split => new (String Combination, Decimal Count)[] {
					(split[0].First() + split[1], 1m),
					(split[1] + split[0].Last(), 1m),
					(split[0], -1m),
				})
				.Let(rules => array[0]
					.Window(2)
					.Select(window => window.JoinToString())
					.Group()
					.ToDictionary(group => group.First(), group => (Decimal) group.Count())
					.With(" " + array[0].First(), 1m)
					.With(array[0].Last() + " ", 1m)
					.Let(pairs => rules
						.SelectMany(rule => pairs.GetValueOrDefault(rule.Key).Let(count => rule.Value.Select(kvp => KeyValuePair.Create(kvp.Combination, count * kvp.Count))))
						.Concat(pairs)
						.GroupBy(kvp => kvp.Key)
						.ToDictionary(group => group.Key, group => group.Sum(pair => pair.Value))
					, steps)
					.Let(pairs => pairs.Keys
						.SelectMany()
						.Distinct()
						.Where(element => element != ' ')
						.Select(element => pairs.Select(pair => pair.Key.Count(element) * pair.Value).Sum() / 2)
						.ToArray()
						.Let(counts => counts.Max() - counts.Min())
					)
				)
			);
}
