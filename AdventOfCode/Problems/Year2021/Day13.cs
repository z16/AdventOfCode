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

	public static String Part2(String[] array) =>
		Check(array, false)
			.OrderBy(dot => dot.Y)
			.ThenBy(dot => dot.X)
			.GroupBy(dot => dot.X / 5, dot => new Point(dot.X % 5, dot.Y))
			.Select(dots => Letters.First(entry => entry.Dots.SequenceEqual(dots)).Letter)
			.JoinToString();

	private static Point[] Check(String[] array, Boolean first) =>
		array
			.Split(String.Empty)
			.ToArray()
			.Let(split => (
				Dots: split[0].Select(line => line.Split(',').Select(Int32.Parse).ToArray()).Select(coords => new Point(coords[0], coords[1])).ToArray(),
				Instructions: (first ? split[1].First().Yield() : split[1]).Select(instruction => instruction.Split(' ').Let(tokens => tokens[2].Split('=').Let(fold => (
					Axis: fold[0] == "x" ? 0 : 1,
					Index: Int32.Parse(fold[1])
				)))).ToArray()
			))
			.Let(state => (
				Dots: state.Instructions.First().Let(instruction => state.Dots
					.Partition(dot => dot[instruction.Axis] < instruction.Index)
					.Let(split => split.True.Concat(split.False.Select(dot => instruction.Axis == 0
						? dot with { X = 2 * instruction.Index - dot.X }
						: dot with { Y = 2 * instruction.Index - dot.Y }
					))))
					.Distinct()
					.ToArray(),
				Instructions: state.Instructions.Skip(1).ToArray()
			), state => state.Instructions.Any())
			.Dots;

	private static readonly (Char Letter, Point[] Dots)[] Letters = new[] {
		('A', new Point[] {
			new(1, 0),
			new(2, 0),
			new(0, 1),
			new(3, 1),
			new(0, 2),
			new(3, 2),
			new(0, 3),
			new(1, 3),
			new(2, 3),
			new(3, 3),
			new(0, 4),
			new(3, 4),
			new(0, 5),
			new(3, 5),
		}),
		('B', new Point[] {
			new(0, 0),
			new(1, 0),
			new(2, 0),
			new(0, 1),
			new(3, 1),
			new(0, 2),
			new(1, 2),
			new(2, 2),
			new(0, 3),
			new(3, 3),
			new(0, 4),
			new(3, 4),
			new(0, 5),
			new(1, 5),
			new(2, 5),
		}),
		('C', new Point[] {
			new(1, 0),
			new(2, 0),
			new(0, 1),
			new(3, 1),
			new(0, 2),
			new(0, 3),
			new(0, 4),
			new(3, 4),
			new(1, 5),
			new(2, 5),
		}),
		//('D', new Point[] {
		//}),
		('E', new Point[] {
			new(0, 0),
			new(1, 0),
			new(2, 0),
			new(3, 0),
			new(0, 1),
			new(0, 2),
			new(1, 2),
			new(2, 2),
			new(0, 3),
			new(0, 4),
			new(0, 5),
			new(1, 5),
			new(2, 5),
			new(3, 5),
		}),
		('F', new Point[] {
			new(0, 0),
			new(1, 0),
			new(2, 0),
			new(3, 0),
			new(0, 1),
			new(0, 2),
			new(1, 2),
			new(2, 2),
			new(0, 3),
			new(0, 4),
			new(0, 5),
		}),
		('G', new Point[] {
			new(1, 0),
			new(2, 0),
			new(0, 1),
			new(3, 1),
			new(0, 2),
			new(0, 3),
			new(2, 3),
			new(3, 3),
			new(0, 4),
			new(3, 4),
			new(1, 5),
			new(2, 5),
			new(3, 5),
		}),
		('H', new Point[] {
			new(0, 0),
			new(3, 0),
			new(0, 1),
			new(3, 1),
			new(0, 2),
			new(1, 2),
			new(2, 2),
			new(3, 2),
			new(0, 3),
			new(3, 3),
			new(0, 4),
			new(3, 4),
			new(0, 5),
			new(3, 5),
		}),
		//('I', new Point[] {
		//}),
		('J', new Point[] {
			new(2, 0),
			new(3, 1),
			new(3, 2),
			new(3, 3),
			new(0, 4),
			new(3, 4),
			new(1, 5),
			new(2, 5),
		}),
		('K', new Point[] {
			new(0, 0),
			new(3, 0),
			new(0, 1),
			new(2, 1),
			new(0, 2),
			new(1, 2),
			new(0, 3),
			new(2, 3),
			new(0, 4),
			new(2, 4),
			new(0, 5),
			new(3, 5),
		}),
		('L', new Point[] {
			new(0, 0),
			new(0, 1),
			new(0, 2),
			new(0, 3),
			new(0, 4),
			new(0, 5),
			new(1, 5),
			new(2, 5),
			new(3, 5),
		}),
		//('M', new Point[] {
		//}),
		//('N', new Point[] {
		//}),
		('O', new Point[] { //
			new(1, 0),
			new(2, 0),
			new(0, 1),
			new(3, 1),
			new(0, 2),
			new(3, 2),
			new(0, 3),
			new(3, 3),
			new(0, 4),
			new(3, 4),
			new(1, 5),
			new(2, 5),
		}),
		('P', new Point[] {
			new(0, 0),
			new(1, 0),
			new(2, 0),
			new(0, 1),
			new(3, 1),
			new(0, 2),
			new(3, 2),
			new(0, 3),
			new(1, 3),
			new(2, 3),
			new(0, 4),
			new(0, 5),
		}),
		//('Q', new Point[] {
		//}),
		('R', new Point[] {
			new(0, 0),
			new(1, 0),
			new(2, 0),
			new(0, 1),
			new(3, 1),
			new(0, 2),
			new(3, 2),
			new(0, 3),
			new(1, 3),
			new(2, 3),
			new(0, 4),
			new(2, 4),
			new(0, 5),
			new(3, 5),
		}),
		//('S', new Point[] {
		//}),
		//('T', new Point[] {
		//}),
		('U', new Point[] {
			new(0, 0),
			new(3, 0),
			new(0, 1),
			new(3, 1),
			new(0, 2),
			new(3, 2),
			new(0, 3),
			new(3, 3),
			new(0, 4),
			new(3, 4),
			new(1, 5),
			new(2, 5),
		}),
		//('V', new Point[] {
		//}),
		//('W', new Point[] {
		//}),
		//('X', new Point[] {
		//}),
		//('Y', new Point[] {
		//}),
		('Z', new Point[] {
			new(0, 0),
			new(1, 0),
			new(2, 0),
			new(3, 0),
			new(3, 1),
			new(2, 2),
			new(1, 3),
			new(0, 4),
			new(0, 5),
			new(1, 5),
			new(2, 5),
			new(3, 5),
		}),
	};
}
