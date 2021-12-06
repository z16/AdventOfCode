using System;
using System.Collections.Generic;
using System.Linq;
using z16.Core;

namespace AdventOfCode.Problems.Year2015;

internal static class Day9 {
	public static Int32 Part1(String[] array) =>
		Graph.Parse(array).ShortestHamiltonianPath();

	public static Int32 Part2(String[] array) =>
		Graph.Parse(array).LongestHamiltonianPath();

	private readonly record struct Edge(Node From, Node To, Int32 Distance = 0) {
		public Boolean Connects(Node node) =>
			From == node || To == node;

		public Node Other(Node end) =>
			end == From ? To : From;

		public Boolean Compare(Edge other) =>
			From == other.From && To == other.To || From == other.To && To == other.From;
	}

	private readonly record struct Node(String Name);

	private readonly record struct Graph(Node[] Nodes, Edge[] Edges) {
		public static Graph Parse(String[] array) {
			var edgeData = array.Select(line => line.Split(" = ")).Select(data => (Connection: data[0].Split(" to "), Distance: Int32.Parse(data[1]))).ToArray();
			var nodes = edgeData.SelectMany(data => data.Connection).Distinct().Select(name => new Node(name)).ToArray();
			var nodeMap = nodes.ToDictionary(node => node.Name, node => node);
			var edges = edgeData.Select(data => new Edge(nodeMap[data.Connection[0]], nodeMap[data.Connection[1]], data.Distance)).ToArray();
			return new(nodes, edges);
		}

		public Int32 ShortestHamiltonianPath() =>
			HamiltonianPath(Enumerable.Min);

		public Int32 LongestHamiltonianPath() =>
			HamiltonianPath(Enumerable.Max);

		public Int32 HamiltonianPath(Func<IEnumerable<Node>, Func<Node, Int32>, Int32> minmax) {
			var nodes = Nodes;
			var edges = Edges;
			return minmax(nodes, node => PathFromNode(node, nodes.Except(node.Yield()).ToArray()));

			Int32 PathFromNode(Node start, IEnumerable<Node> remaining) =>
				remaining.Any()
					? minmax(remaining, node => edges.First(edge => edge.Compare(new(start, node, 0))).Distance + PathFromNode(node, remaining.Except(node.Yield())))
					: 0;
		}
	}
}
