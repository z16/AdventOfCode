using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Problems.Year2020;

internal static class Day2 {
	private static readonly Regex LinePattern = new Regex(@"^(?<first>\d+)-(?<second>\d+) (?<char>\w): (?<password>\w+)$", RegexOptions.Compiled);

	public static Int32 Part1(String[] lines) =>
		lines
			.Select(line => LinePattern.Match(line))
			.Select(match => new {
				Password = match.Groups["password"].Value,
				Min = Int32.Parse(match.Groups["first"].Value),
				Max = Int32.Parse(match.Groups["second"].Value),
				Char = match.Groups["char"].Value.Single(),
			})
			.Select(parsed => new {
				Occurrences = parsed.Password.Count(@char => @char == parsed.Char),
				Min = parsed.Min,
				Max = parsed.Max,
			})
			.Where(counted => counted.Occurrences >= counted.Min && counted.Occurrences <= counted.Max)
			.Count();

	public static Int32 Part2(String[] lines) =>
		lines
			.Select(line => LinePattern.Match(line))
			.Select(match => new {
				Password = match.Groups["password"].Value,
				Index1 = Int32.Parse(match.Groups["first"].Value) - 1,
				Index2 = Int32.Parse(match.Groups["second"].Value) - 1,
				Char = match.Groups["char"].Value.Single(),
			})
			.Select(parsed => new {
				Char1 = parsed.Password[parsed.Index1],
				Char2 = parsed.Password[parsed.Index2],
				Compare = parsed.Char,
			})
			.Where(chars => (chars.Char1 == chars.Compare || chars.Char2 == chars.Compare) && chars.Char1 != chars.Char2)
			.Count();
}
