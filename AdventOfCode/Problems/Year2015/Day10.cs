using System;
using System.Linq;
using z16.Core;

namespace AdventOfCode.Problems.Year2015;

internal static class Day10 {
	public static Int32 Part1(String line) =>
		Check(line, 40);

	public static Int32 Part2(String line) =>
		Check(line, 50);

	private static Int32 Check(String line, Int32 count) =>
		Enumerable.Range(0, count).Aggregate(line, (acc, _) => Expand(acc)).Length;

	private static String Expand(String line) =>
		line
			.Select(character => character - '0')
			.GroupSame()
			.SelectMany(group => new[] { group.Count, group.First() })
			.Select(number => number.ToString())
			.JoinToString();
}
