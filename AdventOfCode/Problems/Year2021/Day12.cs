using System;
using System.Collections.Generic;
using System.Linq;
using z16.Core;

namespace AdventOfCode.Problems.Year2021;

internal static class Day12 {
	public static Int32 Part1(String[] array) =>
		Count(array, 0);

	public static Int32 Part2(String[] array) =>
		Count(array, 1);

	private static Int32 Count(String[] array, Int32 revisits) =>
		array
			.Select(line => line.Split('-').Let(split => (From: (Name: split[0], Big: split[0].All(Char.IsUpper)), To: (Name: split[1], Big: split[1].All(Char.IsUpper)))))
			.Let(Graph.FromEdgesUndirected)
			.Let(graph => (
				Count: 0,
				Start: graph.Nodes.First(node => node.Data.Name == "start"),
				End: graph.Nodes.First(node => node.Data.Name == "end"),
				Paths: new[] {(
					Current: graph.Nodes.First(node => node.Data.Name == "start"),
					Visited: new HashSet<(String, Boolean)>(),
					Revisited: 0
				)}
			))
			.Let(state => state.Paths
				.SelectMany(path => path.Current.Outgoing
					.Where(edge => edge.To != state.Start && (edge.To.Data.Big || !path.Visited.Contains(edge.To.Data) || path.Revisited < revisits))
					.Select(edge => path with {
						Current = edge.To,
						Visited = new HashSet<(String, Boolean)>(path.Visited.Concat(edge.From.Data.Yield())),
						Revisited = path.Revisited + (!edge.To.Data.Big && path.Visited.Contains(edge.To.Data) ? 1 : 0),
					})
				)
				.ToArray()
				.Let(total => total
					.Where(path => path.Current != state.End)
					.ToArray()
					.Let(unfinished => state with {
						Count = state.Count + (total.Length - unfinished.Length),
						Paths = unfinished
					})
				),
				state => state.Paths.Any()
			)
			.Count;
}
