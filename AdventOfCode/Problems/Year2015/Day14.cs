using System;
using System.Linq;

namespace AdventOfCode.Problems.Year2015;

internal static class Day14 {
	public static Int32 Part1(String[] array) =>
		array.Select(Line.Parse).Max(line => Calculate(line, 2503));

	public static Int32 Part2(String[] array) =>
		CountPoints(array.Select(Line.Parse).ToArray());

	private static Int32 Calculate(Line line, Int32 duration) {
		var cycleDuration = line.FlyDuration + line.RestDuration;
		var laps = duration / cycleDuration;
		var remaining = duration % cycleDuration;
		return laps * line.Speed * line.FlyDuration + line.Speed * Math.Min(remaining, line.FlyDuration);
	}

	private static Int32 CountPoints(Line[] lines) =>
		Enumerable.Range(1, 2503)
			.Aggregate(new Int32[lines.Length], (acc, value) => {
				var score = lines.Select(line => Calculate(line, value)).ToArray();
				var highest = score.Max();
				for (var i = 0; i < lines.Length; ++i) {
					if (score[i] == highest) {
						++acc[i];
					}
				}
				return acc;
			}).Max();

	private readonly record struct Line(String Name, Int32 Speed, Int32 FlyDuration, Int32 RestDuration) {
		public static Line Parse(String line) {
			var split1 = line.Split(" can fly ");
			var split2 = split1[1].Split(" km/s for ");
			var split3 = split2[1].Split(" seconds, but then must rest for ");
			return new(split1[0], Int32.Parse(split2[0]), Int32.Parse(split3[0]), Int32.Parse(split3[1][..^9]));
		}
	}
}
