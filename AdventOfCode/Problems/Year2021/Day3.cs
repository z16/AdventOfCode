using System;
using System.Linq;
using z16.Core;

namespace AdventOfCode.Problems.Year2021;

internal static class Day3 {
	public static Int32 Part1(String[] array) {
		var characters = array.First().Length;
		var gamma = array
			.Aggregate(Enumerable.Repeat(0, characters), (acc, line) => acc.Zip(line, (count, character) => count + (character - '0')))
			.Select(count => count > array.Length / 2)
			.Reverse()
			.Enumerate()
			.Aggregate(0, (acc, value) => acc + (value.Value ? (1 << value.Index) : 0));
		var epsilon = ~gamma & ((1 << characters) - 1);
		return gamma * epsilon;
	}

	public static Int32 Part2(String[] array) {
		var oxygen = Filter(array, 1);
		var co2 = Filter(array, 0);
		return oxygen * co2;
	}

	private static Int32 Filter(String[] array, Int32 value, Int32 depth = 0) {
		if (array.Length == 1) {
			return Convert.ToInt32(array.First(), 2);
		}

		var ones = array.Aggregate(0, (acc, line) => acc + (line[depth] - '0'));
		var half = (Decimal) array.Length / 2;
		var match =
			ones > half ? '0' + value :
			ones < half ? '1' - value :
			'0' + value;
		return Filter(array.Where(line => line[depth] == match).ToArray(), value, depth + 1);
	}
}
