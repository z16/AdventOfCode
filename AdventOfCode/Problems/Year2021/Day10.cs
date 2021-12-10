using System;
using System.Collections.Generic;
using System.Linq;
using z16.Core;

namespace AdventOfCode.Problems.Year2021;

internal static class Day10 {
	public static Int64 Part1(String[] array) =>
		array.Let(Check).Sum(line => line.Score);

	public static Int64 Part2(String[] array) =>
		array.Let(Check)
			.Select(acc => acc.Stack.Aggregate(0L, (acc, value) => acc * 5L + value switch {
				'(' => 1L,
				'[' => 2L,
				'{' => 3L,
				'<' => 4L,
			}))
			.Where(score => score > 0L)
			.Order()
			.ToArray().Let(scores => scores[scores.Length / 2]);

	private static IEnumerable<Line> Check(String[] array) =>
		array.Select(line => line.Aggregate(new Line(), (acc, value) => acc.Score != 0L ? acc : value switch {
			')' => acc.Stack.Peek() == '(' ? new(acc.Stack.Popped()) : new(3L),
			']' => acc.Stack.Peek() == '[' ? new(acc.Stack.Popped()) : new(57L),
			'}' => acc.Stack.Peek() == '{' ? new(acc.Stack.Popped()) : new(1197L),
			'>' => acc.Stack.Peek() == '<' ? new(acc.Stack.Popped()) : new(25137L),
			_ => new(acc.Stack.Pushed(value)),
		}));

	private static Stack<T> Pushed<T>(this Stack<T> stack, T value) {
		stack.Push(value);
		return stack;
	}

	private static Stack<T> Popped<T>(this Stack<T> stack) {
		stack.Pop();
		return stack;
	}

	private readonly record struct Line(Stack<Char> Stack, Int64 Score) {
		public Line() : this(new Stack<Char>(), 0L) { }
		public Line(Stack<Char> stack) : this(stack, 0L) { }
		public Line(Int64 score) : this(new Stack<Char>(), score) { }
	}
}
