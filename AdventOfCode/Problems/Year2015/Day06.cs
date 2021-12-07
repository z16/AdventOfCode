using System;
using System.Collections.Generic;
using System.Linq;
using Point = z16.Core.Vector2<System.Int32>;

namespace AdventOfCode.Problems.Year2015;

internal static class Day06 {
	public static Int32 Part1(String[] array) =>
		Check(array, BooleanActions).Count(value => value);

	public static Int32 Part2(String[] array) =>
		Check(array, NumberActions).Sum();

	private static IEnumerable<T> Check<T>(String[] array, IReadOnlyDictionary<String, Func<T, T>> actions) =>
		array
			.Select(Line.Parse)
			.Aggregate(new T[1000, 1000], (board, line) => board.Update(line, actions[line.Action]))
			.OfType<T>();

	private static T[,] Update<T>(this T[,] board, Line line, Func<T, T> action) {
		for (var x = line.From.X; x <= line.To.X; ++x) {
			for (var y = line.From.Y; y <= line.To.Y; ++y) {
				board[x, y] = action(board[x, y]);
			}
		}
		return board;
	}

	private readonly record struct Line(String Action, Point From, Point To) {
		public static Line Parse(String line) {
			var through = line.Split(" through ");
			var actionFrom = through[0].Split(' ');
			return new(String.Join(' ', actionFrom.SkipLast(1)), Point.Parse(actionFrom.Last()), Point.Parse(through[1]));
		}
	}

	private readonly static IReadOnlyDictionary<String, Func<Boolean, Boolean>> BooleanActions = new Dictionary<String, Func<Boolean, Boolean>>() {
		["turn on"] = boolean => true,
		["turn off"] = boolean => false,
		["toggle"] = boolean => !boolean,
	};

	private readonly static IReadOnlyDictionary<String, Func<Int32, Int32>> NumberActions = new Dictionary<String, Func<Int32, Int32>>() {
		["turn on"] = number => number + 1,
		["turn off"] = number => Math.Max(number - 1, 0),
		["toggle"] = number => number + 2,
	};
}
