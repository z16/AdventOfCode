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
			.Select(line => line.Split('-').Let(split => (From: ParseNode(split[0]), To: ParseNode(split[1]))))
			.Let(Graph.FromEdgesUndirected)
			.Let(graph => (
				Graph: graph,
				Start: graph.Nodes.First(node => node.Data.Name == "start"),
				Current: graph.Nodes.First(node => node.Data.Name == "start"),
				End: graph.Nodes.First(node => node.Data.Name == "end"),
				Visited: new List<Node>(),
				Revisited: 0,
				Finished: false
			))
			.Yield()
			.ToArray()
			.Let(states => states
				.Where(state => state.Finished)
				.Concat(states.Where(state => !state.Finished).SelectMany(state =>
					state.Current.Outgoing
						.Where(edge => edge.To != state.Start && (edge.To.Data.Big || !state.Visited.Contains(edge.To.Data) || state.Revisited < revisits))
						.Select(edge => state with {
							Current = edge.To,
							Visited = new List<Node>(state.Visited.Concat(edge.From.Data.Yield())),
							Revisited = state.Revisited + (!edge.To.Data.Big && state.Visited.Contains(edge.To.Data) ? 1 : 0),
							Finished = edge.To == state.End
						})
				)).ToArray(), states => states.Any(state => !state.Finished))
			.Length;

	private static Node ParseNode(String name) =>
		new(name, name.All(Char.IsUpper));

	private readonly record struct Node(String Name, Boolean Big);
}
