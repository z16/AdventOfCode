using System;
using System.Collections.Generic;
using System.Linq;
using z16.Core;

namespace AdventOfCode.Problems.Year2021;

internal static class Day10 {
	public static Int64 Part1(String[] array) =>
		array.Sum(line => Check(line).Score);

	public static Int64 Part2(String[] array) =>
		array.Select(Check)
			.Where(line => line.Score == 0L)
			.Select(line => line.Stack.Aggregate(0L, (acc, value) => acc * 5L + value switch {
				'(' => 1L,
				'[' => 2L,
				'{' => 3L,
				'<' => 4L,
			}))
			.Order()
			.Median();

	private static Line Check(String line) =>
		line.Aggregate(new Line(new Stack<Char>(), 0L), (acc, value) => acc.Score != 0L ? acc : value switch {
			')' => acc.Stack.Pop().Let(match => match == '(' ? acc : acc with { Score = 3L }),
			']' => acc.Stack.Pop().Let(match => match == '[' ? acc : acc with { Score = 57L }),
			'}' => acc.Stack.Pop().Let(match => match == '{' ? acc : acc with { Score = 1197L }),
			'>' => acc.Stack.Pop().Let(match => match == '<' ? acc : acc with { Score = 25137L }),
			_ => acc.Do(line => line.Stack.Push(value)),
		});

	private readonly record struct Line(Stack<Char> Stack, Int64 Score);
}
