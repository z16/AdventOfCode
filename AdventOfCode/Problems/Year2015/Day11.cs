using System;
using System.Linq;
using z16.Core;
using static MoreLinq.Extensions.WindowExtension;

namespace AdventOfCode.Problems.Year2015;

internal static class Day11 {
	public static String Part1(String line) =>
		Check(line);

	public static String Part2(String line) =>
		Check(Check(line));

	private static String Check(String line) {
		var array = line.Select(character => character - 'a').ToArray();
		Increment(array);
		while (!Check(array)) {
			Increment(array);
		}

		return array.Select(number => (Char) ('a' + number)).JoinToString();
	}

	private static Boolean Check(Int32[] array) =>
		array.Window(3).Any(window => window[0] + 1 == window[1] && window[1] + 1 == window[2]) &&
		array.All(number => !number.OneOf('i' - 'a', 'o' - 'a', 'l' - 'a')) &&
		Enumerable.Range(0, 26).AtLeast(2, value => array.Window(2).Any(window => window[0] == value && window[1] == value));


	private static void Increment(Int32[] array) {
		for (var i = 7; i > 0; --i) {
			++array[i];
			if (array[i] < 26) {
				break;
			}
			array[i] = 0;
		}
	}
}
