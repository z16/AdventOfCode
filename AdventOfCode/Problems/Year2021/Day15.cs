using System;
using System.Linq;
using System.Collections.Generic;
using z16.Core;
using Point = z16.Core.Vector2<System.Int32>;

namespace AdventOfCode.Problems.Year2021;

internal static class Day15 {
	public static Int32 Part1(Int32[,] array) =>
		Check(array);

	public static Int32 Part2(Int32[,] array) =>
		new Point(array.GetLength(0), array.GetLength(1))
			.Let(max => Check(new Int32[max.X * 5, max.Y * 5].Apply((point, _) => (array[point.X % max.X, point.Y % max.Y] + point.X / max.X + point.Y / max.Y).Let(value => value > 9 ? value - 9 : value))));

	private static Int32 Check(Int32[,] array) =>
		(
			Graph: array.Apply((point, value) => (
				Risk: value,
				Total: point == (0, 0) ? 0 : Int32.MaxValue
			)),
			Visited: new HashSet<Point>(),
			Next: new PriorityQueue<Point, Int32>(new (Point, Int32)[] { ((0, 0), 0) }),
			End: array.Indices().Last()
		)
		.Let(state => state.Next.Dequeue()
			.Do(currentIndex => state.Visited.Add(currentIndex))
			.Let(currentIndex => state.Graph.Get(currentIndex).Total
				.Let(total => state.Graph.AxisNeighbors(currentIndex)
					.Where(neighbor => !state.Visited.Contains(neighbor))
					.ToArray()
					.Do(neighbors => neighbors
						.Where(index => state.Graph.Get(index).Let(neighbor => total + neighbor.Risk < neighbor.Total))
						.DoEach(index => state.Graph.Set(index, state.Graph.Get(index).Let(neighbor => neighbor with {
							Total = total + neighbor.Risk
						})))
					)
					.Let(neighbors => neighbors
						.Select(neighborIndex => (Element: neighborIndex, Priority: state.Graph.Get(neighborIndex).Total))
						.Concat(state.Next.UnorderedItems)
						.KeepFirstBy(entry => entry.Element)
						.Let(nexts => state with {
							Next = new(nexts)
						})
					)
				)
			), state => state.Next.Peek() != state.End
		)
		.Graph.Get(array.Indices().Last()).Total;
}
