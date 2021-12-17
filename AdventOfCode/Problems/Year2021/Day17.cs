using System;
using System.Collections.Generic;
using System.Linq;
using z16.Core;
using Point = z16.Core.Vector2<System.Int32>;

namespace AdventOfCode.Problems.Year2021;

internal static class Day17 {
	public static Int32 Part1(String line) =>
		Check(line).Max(vector => vector.Y).Let(y => y * (y + 1) / 2);

	public static Int32 Part2(String line) =>
		Check(line).Count();

	private static IEnumerable<Point> Check(String line) =>
		Parse(line)
			.Let(target => (target.To.X + 1, Math.Max(-target.From.Y, target.To.Y) + 1).RangeToFrom(((Int32) Math.Round(Math.Sqrt(0.25 + 2 * target.From.X) - 0.5), target.From.Y))
				.Where(vector => (Vector: vector, Position: new Point(0, 0))
					.Let(state => (
						Vector: (Math.Max(state.Vector.X - 1, 0), state.Vector.Y - 1),
						Position: state.Position + state.Vector
					), state => !state.Position.InBounds(target.From, target.To) && state.Position.X <= target.To.X && state.Position.Y >= target.From.Y)
					.Position.InBounds(target.From, target.To)
				).Select(tuple => (Point) tuple)
			);

	private static (Point From, Point To) Parse(String line) =>
		line.Split(": ")
			.Let(split => split[1]
				.Split(", ")
				.SelectMany(coordinate => coordinate[2..].Split("..").Select(Int32.Parse))
				.ToArray()
			)
			.Let(data => ((data[0], data[2]), (data[1], data[3])));
}
