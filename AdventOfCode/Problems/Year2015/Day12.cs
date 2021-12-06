using System;
using System.Linq;
using System.Text.Json;

namespace AdventOfCode.Problems.Year2015;

internal static class Day12 {
	public static Int32 Part1(String line) =>
		Count(line, CountAll);

	public static Int32 Part2(String line) =>
		Count(line, CountNoReds);

	private static Int32 Count(String line, Func<JsonElement, Int32> count) =>
		count(JsonSerializer.Deserialize<JsonElement>(line));

	private static Int32 CountAll(JsonElement element) =>
		element.ValueKind switch {
			JsonValueKind.Number => element.GetInt32(),
			JsonValueKind.Array => element.EnumerateArray().Sum(CountAll),
			JsonValueKind.Object => element.EnumerateObject().Select(property => property.Value).Sum(CountAll),
			_ => 0,
		};

	private static Int32 CountNoReds(JsonElement element) =>
		element.ValueKind switch {
			JsonValueKind.Number => element.GetInt32(),
			JsonValueKind.Array => element.EnumerateArray().Sum(CountNoReds),
			JsonValueKind.Object => element.EnumerateObject().Any(property => property.Value.ValueKind == JsonValueKind.String && property.Value.GetString() == "red")
				? 0
				: element.EnumerateObject().Select(property => property.Value).Sum(CountNoReds),
			_ => 0,
		};
}
