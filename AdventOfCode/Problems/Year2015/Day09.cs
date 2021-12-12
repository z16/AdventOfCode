using System;
using System.Collections.Generic;
using System.Linq;
using z16.Core;
using Node = z16.Core.Graph<System.String, System.Int32>.Node;

namespace AdventOfCode.Problems.Year2015;

internal static class Day09 {
	public static Int32 Part1(String[] array) =>
		Parse(array).HamiltonianPath(Enumerable.Min);

	public static Int32 Part2(String[] array) =>
		Parse(array).HamiltonianPath(Enumerable.Max);

	public static Graph<String, Int32> Parse(String[] array) =>
		Graph.FromEdgesUndirected(array
			.Select(line => line.Split(" = "))
			.SelectMany(data => data[0].Split(" to ").Let(connection => new[] {(
				From: connection[0],
				To: connection[1],
				Distance: Int32.Parse(data[1])
			)}))
		);

	public static Int32 HamiltonianPath(this Graph<String, Int32> graph, Func<IEnumerable<Node>, Func<Node, Int32>, Int32> minmax) {
		var nodes = graph.Nodes;
		var edges = graph.Edges;
		return minmax(nodes, node => PathFromNode(node, nodes.Except(node.Yield()).ToArray()));

		Int32 PathFromNode(Node start, Node[] remaining) =>
			remaining.Any()
				? minmax(remaining, node => graph.EdgeMap[(start, node)].Data + PathFromNode(node, remaining.Except(node.Yield()).ToArray()))
				: 0;
	}
}
