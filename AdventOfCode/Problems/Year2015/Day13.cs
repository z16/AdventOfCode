using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using z16.Core;

namespace AdventOfCode.Problems.Year2015;

internal static class Day13 {
	public static Int32 Part1(String[] array) =>
		Check(array, false);

	public static Int32 Part2(String[] array) =>
		Check(array, true);

	private static Int32 Check(String[] array, Boolean addSelf) {
		var lines = array.Select(Line.Parse).ToArray();
		var persons = lines.Select(line => line.From).Distinct();
		if (addSelf) {
			lines = lines.Concat(persons.SelectMany(person => new[] { new Line(person, "z16", 0), new Line("z16", person, 0) })).ToArray();
			persons = persons.Concat("z16".Yield()).ToArray();
		}
		return persons.Permutations().Max(permutation => Check(permutation, lines));
	}

	private static Int32 Check(IList<String> persons, Line[] lines) =>
		persons
			.Concat(persons.First().Yield())
			.Window(2)
			.Sum(window => lines.First(line => line.From == window[0] && line.To == window[1]).Happiness + lines.First(line => line.From == window[1] && line.To == window[0]).Happiness);

	private readonly record struct Line(String From, String To, Int32 Happiness) {
		public static Line Parse(String line) {
			var split1 = line.Split(" happiness units by sitting next to ");
			var split2 = split1[0].Split(" would ");
			var split3 = split2[1].Split(' ');
			return new(split2[0], split1[1][..^1], (split3[0] == "gain" ? 1 : -1) * Int32.Parse(split3[1]));
		}
	}
}
