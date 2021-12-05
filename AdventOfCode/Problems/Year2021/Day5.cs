using System;
using System.Collections.Generic;
using System.Linq;
using z16.Core;

namespace AdventOfCode.Problems.Year2021;

internal static class Day5 {
	public static Int32 Part1(String[] array) =>
		Check(array, line => line.From.X == line.To.X || line.From.Y == line.To.Y);

	public static Int32 Part2(String[] array) =>
		Check(array, _ => true);

	private static Int32 Check(String[] array, Func<Line, Boolean> filter) =>
		array
			.Select(line => line.Split(" -> ").Select(points => points.Split(',').Select(Int32.Parse).ToArray()).Select(Vector2<Int32>.Parse).ToArray())
			.Select(vectors => new Line(vectors[0], vectors[1]))
			.Where(filter)
			.SelectMany(Expand)
			.GroupBy(point => point)
			.Where(group => group.Count() > 1)
			.Count();

	private static IEnumerable<Vector2<Int32>> Expand(Line line) {
		var distance = Math.Max(Math.Abs(line.To.X - line.From.X), Math.Abs(line.To.Y - line.From.Y));
		var direction = (line.To - line.From) / distance;
		return Enumerable.Range(0, distance + 1).Select(index => line.From + index * direction);
	}

	private record struct Line(Vector2<Int32> From, Vector2<Int32> To);
}
