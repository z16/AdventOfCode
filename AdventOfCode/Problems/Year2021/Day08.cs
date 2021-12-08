using System;
using System.Collections.Generic;
using System.Linq;
using z16.Core;

namespace AdventOfCode.Problems.Year2021;

internal static class Day08 {
	public static Int32 Part1(String[] array) =>
		array.Select(line => line.Split("| ")[1]).SelectMany(output => output.Split(' ')).Count(token => token.Length.OneOf(2, 3, 4, 7));

	public static Int32 Part2(String[] array) =>
		array.Select(line => line.Split(" | ")).Sum(Parse);

	private static Int32 Parse(String[] array) {
		var input = array[0].Split(' ');
		var output = array[1].Split(' ');

		var mapping = new Dictionary<Int32, String>() {
			[1] = input.First(token => token.Length == 2),
			[7] = input.First(token => token.Length == 3),
			[4] = input.First(token => token.Length == 4),
			[8] = input.First(token => token.Length == 7),
		};
		mapping[3] = Find(5, mapping[1], 2);
		mapping[2] = Find(5, mapping[4], 2);
		mapping[5] = Find(5, mapping[4], 3);
		mapping[9] = Find(6, mapping[3], 5);
		mapping[0] = Find(6, mapping[1], 2);
		mapping[6] = Find(6, mapping[1], 1);

		return output.Reverse().Enumerate().Aggregate(0, (acc, value) => acc + (Int32) Math.Pow(10, value.Index) * mapping.First(kvp => kvp.Value.SequenceEqual(value.Value.Order())).Key);

		String Find(Int32 length, String substring, Int32 matches) =>
			input!.First(token => !mapping.ContainsValue(token) && token.Length == length && token.Intersect(substring).Count() == matches);
	}
}
