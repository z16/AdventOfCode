using System;
using System.Collections.Generic;
using System.Linq;
using z16.Core;
using Point = z16.Core.Vector2<System.Int32>;

namespace AdventOfCode.Problems.Year2021;

internal static class Day09 {
	public static Int32 Part1(String[] array) =>
		Prepare(array).Let(CheckRisk);

	public static Int32 Part2(String[] array) =>
		Prepare(array).Let(CheckBasin);

	private static Int32[,] Prepare(String[] array) =>
		array.Select(line => line.Select(character => character - '0')).To2DArray();

	private static Int32 CheckRisk(Int32[,] array) =>
		array.Indices().Where(array.IsSink).Sum(point => array.Get(point) + 1);

	private static Int32 CheckBasin(Int32[,] array) =>
		array.Indices()
			.Where(array.IsSink)
			.Select(point => GrowBasin(array, new HashSet<Point>() { point }))
			.Select(basin => basin.Count)
			.OrderDescending()
			.Take(3)
			.Multiply();

	private static HashSet<Point> GrowBasin(Int32[,] array, HashSet<Point> basin) =>
		basin
			.SelectMany(point => array.AxisNeighbors(point).Where(neighbor => !basin.Contains(neighbor) && array.Get(neighbor) != 9))
			.Concat(basin)
			.ToHashSet()
			.Let(grown => grown.Count == basin.Count ? basin : GrowBasin(array, grown));

	private static Boolean IsSink(this Int32[,] array, Point point) =>
		array
			.AxisNeighbors(point)
			.Select(neighbor => array[neighbor.X, neighbor.Y])
			.All(value => value > array.Get(point));
}
