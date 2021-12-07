using System;
using System.Linq;

namespace AdventOfCode.Problems.Year2020;

internal static class Day03 {
	public static Int32 Part1(Tile[][] field) => CountSlope(field, 3, 1);

	public static Decimal Part2(Tile[][] field) => new[] {
		(Right: 1, Down: 1),
		(Right: 3, Down: 1),
		(Right: 5, Down: 1),
		(Right: 7, Down: 1),
		(Right: 1, Down: 2),
	}.Aggregate(1m, (acc, tuple) => acc * CountSlope(field, tuple.Right, tuple.Down));

	private static Int32 CountSlope(Tile[][] field, Int32 right, Int32 down) =>
		field
			.Select((row, index) => index % down == 0 ? row[right * index / down % row.Length] : Tile.Open)
			.Count(tile => tile == Tile.Tree);

	public enum Tile {
		Open = '.',
		Tree = '#',
	}
}
