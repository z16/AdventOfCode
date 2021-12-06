using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;

namespace AdventOfCode.Problems.Year2015;

internal static class Day5 {
	public static Int32 Part1(String[] array) =>
		array.Count(line => line.Count(Vowels.Contains) >= 3 && line.Window(2).Any(characters => characters[0] == characters[1]) && !Forbidden.Any(line.Contains));

	public static Int32 Part2(String[] array) =>
		array.Count(line =>
			line.Window(2).Select((window, index) => (Index: index, Substring: String.Join(String.Empty, window))).Any(match => line[(match.Index + 2)..].Contains(match.Substring)) &&
			line.Window(3).Any(window => window[0] == window[2])
		);

	private readonly static HashSet<Char> Vowels = new(new[] { 'a', 'e', 'i', 'o', 'u' });
	private readonly static HashSet<String> Forbidden = new(new[] { "ab", "cd", "pq", "xy" });
}
