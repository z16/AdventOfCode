using System;
using System.Linq;

namespace AdventOfCode.Problems.Year2015;

internal static class Day01 {
	public static Int32 Part1(String line) =>
		line.Count(character => character == '(') - line.Count(character => character == ')');

	public static Int32 Part2(String line) =>
		line.Aggregate((Floor: 0, Position: 0), (acc, value) => acc.Floor < 0 ? acc : (acc.Floor + (value == '(' ? 1 : -1), acc.Position + 1)).Position;
}
