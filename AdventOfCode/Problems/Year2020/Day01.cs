using System;
using System.Linq;

namespace AdventOfCode.Problems.Year2020;

internal static class Day01 {
	public static Int32 Part1(Int32[] array) {
		var (first, second) = array
			.SelectMany(_ => array, (first, second) => (First: first, Second: second))
			.First(tuple => tuple.First + tuple.Second == 2020);
		return first * second;
	}

	public static Int32 Part2(Int32[] array) {
		var (first, second, third) = array
			.SelectMany(_ => array, (first, second) => (First: first, Second: second))
			.SelectMany(_ => array, (tuple, third) => (First: tuple.First, Second: tuple.Second, Third: third))
			.First(tuple => tuple.First + tuple.Second + tuple.Third == 2020);
		return first * second * third;
	}
}
