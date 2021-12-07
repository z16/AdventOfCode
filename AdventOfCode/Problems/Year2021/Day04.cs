using System;
using System.Collections.Generic;
using System.Linq;
using z16.Core;

namespace AdventOfCode.Problems.Year2021;

internal static class Day04 {
	public static Int32 Part1(String[] array) =>
		Check(array, CheckFirst);

	public static Int32 Part2(String[] array) =>
		Check(array, CheckLast);

	private static Int32 Check(String[] array, Func<Int32[][][], Int32[], Int32> checker) {
		var numbers = array.First().Split(',').Select(Int32.Parse).ToArray();

		var boards = array
			.Skip(1)
			.Chunk(6)
			.Select(chunk => chunk.Skip(1).Select(line => line.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(Int32.Parse).ToArray()).ToArray())
			.ToArray();

		return checker(boards, numbers);
	}

	private static Int32 CheckLast(Int32[][][] boards, Int32[] numbers) {
		var (_, board, last, _) = Enumerable.Range(0, numbers.Length)
			.Reverse()
			.Select(index => Selector(boards, numbers, index))
			.First(results => results.Any(result => !result.Win))
			.First(result => !result.Win);
		var index = numbers.IndexOf(last)!.Value + 1;
		return CheckBoard(board, new HashSet<Int32>(numbers.Take(index + 1)), numbers[index]).Score;
	}

	private static Int32 CheckFirst(Int32[][][] boards, Int32[] numbers) =>
		Enumerable.Range(0, numbers.Length)
			.Select(index => Selector(boards, numbers, index))
			.First(results => results.Any(result => result.Win))
			.First(result => result.Win)
			.Score;

	private static Result[] Selector(Int32[][][] boards, Int32[] numbers, Int32 index) =>
		boards.Select(board => CheckBoard(board, new HashSet<Int32>(numbers.Take(index + 1)), numbers[index])).ToArray();

	private static Result CheckBoard(Int32[][] board, HashSet<Int32> numbers, Int32 last) {
		var win = board.Any(row => row.All(numbers.Contains)) || Enumerable.Range(0, board.Length).Any(index => board.All(row => numbers.Contains(row[index])));
		return new(win, board, last, !win
			? default
			: last * board
				.SelectMany(row => row)
				.Where(number => !numbers.Contains(number))
				.Aggregate(0, (acc, value) => acc + value));
	}

	private record struct Result(Boolean Win, Int32[][] Board, Int32 Last, Int32 Score);
}
