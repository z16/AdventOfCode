using System;
using System.Collections.Generic;
using System.Linq;
using z16.Core;
using static MoreLinq.Extensions.WindowExtension;

namespace AdventOfCode.Problems.Year2015;

internal static class Day08 {
	public static Int32 Part1(String[] array) =>
		array.Select(line => line.Skip(1).SkipLast(1).Window(2).Select(CheckWindow).Aggregate((Total: 2, Skip: 0), (acc, value) => acc.Skip > 0
			? (Total: acc.Total, Skip: acc.Skip - 1)
			: (Total: acc.Total + value, Skip: value)
		).Total).Sum();

	public static Int32 Part2(String[] array) =>
		array.Sum(line => 2 + line.Count(character => character.OneOf('"', '\\')));

	private static Int32 CheckWindow(IEnumerable<Char> window) =>
		window.SequenceEqual("\\\\") || window.SequenceEqual("\\\"") ? 1 :
		window.SequenceEqual("\\x") ? 3 :
		0;
}
