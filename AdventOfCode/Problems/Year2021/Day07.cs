using System;
using System.Linq;
using z16.Core;

namespace AdventOfCode.Problems.Year2021;

internal static class Day07 {
	public static Int32 Part1(String line) =>
		Check(line, (crab, target) => Math.Abs(target - crab));

	public static Int32 Part2(String line) =>
		Check(line, (crab, target) => Math.Abs(target - crab).Let(distance => (distance * distance + distance) / 2));

	private static Int32 Check(String line, Func<Int32, Int32, Int32> distance) =>
		line.Split(',').Select(Int32.Parse).ToArray().Let(array => Check(array, array.Min(), array.Max(), distance));

	private static Int32 Check(Int32[] array, Int32 min, Int32 max, Func<Int32, Int32, Int32> distance) =>
		Enumerable.Range(min, max - min + 1).Min(target => array.Sum(crab => distance(crab, target)));
}
