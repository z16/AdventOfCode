using System;
using System.Linq;
using z16.Core;
using static MoreLinq.Extensions.SplitExtension;
using static MoreLinq.Extensions.PartitionExtension;
using Point = z16.Core.Vector2<System.Int32>;

namespace AdventOfCode.Problems.Year2021;

internal static class Day13 {
	public static Int32 Part1(String[] array) =>
		Check(array, true).Length;

	public static Int32 Part2(String[] array) =>
		Check(array, false).Aggregate(0x55555555, (acc, value) => acc ^ value.GetHashCode());

	//public static Int32 Part2(String[] array) {
	//	var dots = Check(array, false);
	//	var maxX = dots.Max(dot => dot.X);
	//	var maxY = dots.Max(dot => dot.Y);
	//	var set = dots.ToHashSet();
	//	Console.WriteLine();
	//	for (var x = 0; x <= maxY; ++x) {
	//		for (var y = 0; y <= maxX; ++y) {
	//			Console.Write(dots.Contains((y, x)) ? '#' : '.');
	//		}
	//		Console.WriteLine();
	//	}
	//	return 0;
	//}

	private static Point[] Check(String[] array, Boolean first) =>
		array
			.Split(String.Empty)
			.ToArray()
			.Let(split => (
				Dots: split[0].Select(line => line.Split(',').Select(Int32.Parse).ToArray()).Select(coords => new Point(coords[0], coords[1])).ToArray(),
				Instructions: (first ? split[1].First().Yield() : split[1]).Select(instruction => instruction.Split(' ').Let(tokens => tokens[2].Split('=').Let(fold => (
					Axis: fold[0] == "x" ? 0 : 1,
					Index: Int32.Parse(fold[1])
				))))
			))
			.Let(state => (
				Dots: state.Instructions.First().Let(instruction => state.Dots
					.Partition(dot => dot[instruction.Axis] < instruction.Index)
					.Let(split => split.True.Concat(split.False.Select(dot => new Point(
						instruction.Axis == 0 ? 2 * instruction.Index - dot.X : dot.X,
						instruction.Axis == 1 ? 2 * instruction.Index - dot.Y : dot.Y
					)))))
					.Distinct()
					.ToArray(),
				Instructions: state.Instructions.Skip(1).ToArray()
			), state => state.Instructions.Any())
			.Dots;
}
