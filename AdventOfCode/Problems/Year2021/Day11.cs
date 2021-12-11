using System;
using System.Linq;
using z16.Core;

namespace AdventOfCode.Problems.Year2021;

internal static class Day11 {
	public static Int32 Part1(Int32[,] array) =>
		new State(array, 0, 0).Let(state => Step(state), 100).Flashes;

	public static Int32 Part2(Int32[,] array) =>
		new State(array, 0, 0).Let(state => Step(state), state => state.Array.Enumerate().Any(value => value != 0)).Round;

	private static State Step(State state) =>
		state.Array
			.Let(array => array.Apply(value => value + 1))
			.Let(Flash, array => array.Enumerate().Any(value => value >= 10))
			.Let(array => state with { Flashes = state.Flashes + array.Enumerate().Count(value => value == 0), Round = state.Round + 1 });

	private static Int32[,] Flash(Int32[,] array) =>
		array.Apply((point, value) => value == 0 ? 0 : value >= 10 ? 0 : value + array.Neighbors(point).Select(array.Get).Count(value => value >= 10));

	private readonly record struct State(Int32[,] Array, Int32 Flashes, Int32 Round);
}
