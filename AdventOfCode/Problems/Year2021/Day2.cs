using System;
using System.Linq;
using z16.Core;

namespace AdventOfCode.Problems.Year2021;

internal static class Day2 {
	public static Int32 Part1(String[] array) {
		var (x, y) = array
			.Select(line => line.Split(' '))
			.Select(tokens => (
				Direction: tokens[0],
				Distance: Int32.Parse(tokens[1])
			))
			.Aggregate(new Vector2<Int32>(0, 0), (acc, value) => acc + value.Distance * value.Direction switch {
				"forward" => new Vector2<Int32>(1, 0),
				"down" => new Vector2<Int32>(0, 1),
				"up" => new Vector2<Int32>(0, -1),
			});
		return x * y;
	}

	public static Int32 Part2(String[] array) {
		var (x, y) = array
			.Select(line => line.Split(' '))
			.Select(tokens => (
				Direction: tokens[0],
				Distance: Int32.Parse(tokens[1])
			))
			.Aggregate((
				Vector: new Vector2<Int32>(0, 0),
				Aim: 0
			), (acc, value) => value.Direction switch {
				"forward" => (acc.Vector + (value.Distance, acc.Aim * value.Distance), acc.Aim),
				"down" => (acc.Vector, acc.Aim + value.Distance),
				"up" => (acc.Vector, acc.Aim - value.Distance),
			}).Vector;
		return x * y;
	}
}
