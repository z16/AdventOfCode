using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Problems.Year2021;

internal static class Day01 {
	public static Int32 Part1(Int32[] array) =>
		CountDecreases(array);

	public static Int32 Part2(Int32[] array) =>
		CountDecreases(Enumerable.Range(0, array.Length - 2).Select(position => array.Skip(position).Take(3).Sum()));

	private static Int32 CountDecreases(IEnumerable<Int32> enumerable) =>
		enumerable
			.Skip(1)
			.Aggregate((
				Count: 0,
				Last: enumerable.First()
			), (acc, value) => (
				Count: value > acc.Last ? acc.Count + 1 : acc.Count,
				Last: value)
			)
			.Count;
}
