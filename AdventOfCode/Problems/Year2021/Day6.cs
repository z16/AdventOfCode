using System;
using System.Linq;
using z16.Core;

namespace AdventOfCode.Problems.Year2021;

internal static class Day6 {
	public static Int64 Part1(String line) =>
		CountAll(line, 80);

	public static Int64 Part2(String line) =>
		CountAll(line, 256);

	private static Int64 CountAll(String line, Int64 days) =>
		line.Split(',').Select(Int64.Parse).Sum(remaining => 1 + Count.Invoke(remaining + 1, days));

	private static Int64 CountSpawns(Int64 remaining, Int64 total, Int32 spawns) =>
		spawns + Enumerable.Range(0, spawns).Sum(spawn => Count.Invoke(9, total - remaining - spawn * 7));

	private static readonly Memoize<Int64, Int64, Int64> Count = new((Int64 remaining, Int64 total) =>
		total < remaining
			? 0
			: CountSpawns(remaining, total, (Int32) (total - remaining) / 7 + 1)
	);
}
