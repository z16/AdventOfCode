using System;
using System.Collections.Generic;
using System.Linq;
using z16.Core;
using static MoreLinq.Extensions.TakeEveryExtension;
using Point = z16.Core.Vector2<System.Int32>;

namespace AdventOfCode.Problems.Year2015;

internal static class Day3 {
	public static Int32 Part1(String line) =>
		Check(line).Distinct().Count();

	public static Int32 Part2(String line) =>
		Check(line.TakeEvery(2)).Concat(Check(line.Skip(1).TakeEvery(2))).Distinct().Count();

	private static IEnumerable<Point> Check(IEnumerable<Char> characters) =>
		characters
			.Select(character => character switch {
				'^' => new Point(0, 1),
				'v' => new Point(0, -1),
				'>' => new Point(1, 0),
				'<' => new Point(-1, 0),
			})
			.Aggregate((Position: new Point(), Visited: new Point(0, 0).Yield()), (acc, direction) => (
				Position: acc.Position + direction,
				Visited: acc.Visited.Append(acc.Position + direction))
			).Visited;
}
