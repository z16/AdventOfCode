using System;
using System.Linq;

namespace AdventOfCode.Problems.Year2015;

internal static class Day02 {
	public static Int32 Part1(String[] array) =>
		array
			.Select(line => line.Split('x').Select(Int32.Parse).ToArray())
			.Select(sides => new[] { sides[0] * sides[1], sides[1] * sides[2], sides[2] * sides[0] })
			.Sum(areas => 2 * areas.Sum() + areas.Min());

	public static Int32 Part2(String[] array) =>
		array
			.Select(line => line.Split('x').Select(Int32.Parse).ToArray())
			.Sum(sides => 2 * sides.OrderBy(side => side).Take(2).Sum() + sides.Aggregate(1, (acc, value) => acc * value));
}
