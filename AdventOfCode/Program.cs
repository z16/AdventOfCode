using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using z16.Core;

namespace AdventOfCode;

internal class Program {
	private static async Task Main(String data) {
		Console.SetCursorPosition(10, Console.GetCursorPosition().Top);
		Console.Write("Part 1".PadLeft(20).PadRight(24));
		Console.Write("Part 2".PadLeft(20).PadRight(24));
		Console.WriteLine();

		Console.SetCursorPosition(10, Console.GetCursorPosition().Top);
		Console.Write("Result".PadLeft(16));
		Console.Write("Time".PadLeft(8));
		Console.Write("Result".PadLeft(16));
		Console.Write("Time".PadLeft(8));
		Console.WriteLine();

		var lastYear = String.Empty;
		var days = Assembly.GetExecutingAssembly().GetTypes()
			.Where(type => type.Name.StartsWith("Day") && Int32.TryParse(type.Name[3..], out var _) && type.Namespace!.StartsWith($"{nameof(AdventOfCode)}.{nameof(Problems)}.Year"))
			.OrderBy(type => type.FullName!);
		foreach (var day in days) {
			var year = day.FullName!.SplitEnumerable('.').ElementAt(2);
			if (year != lastYear) {
				Console.WriteLine();
				Console.WriteLine($" {year[4..]}");
				lastYear = year;
			}

			Console.Write($"       {day.Name[3..]} ");
			var parts = new[] { day.GetMethod("Part1"), day.GetMethod("Part2") };
			var input = await GetArgument(parts.First(part => part != null)!, Path.Combine(data, "Inputs", year, day.Name));
			var expected = (await Task.WhenAll(parts.Select(async part => part == null ? null : await GetResult(part, Path.Combine(data, "Outputs", year, day.Name, part.Name))))).ToArray();
			RunPart(parts[0], input, expected[0]);
			RunPart(parts[1], input, expected[1]);

			Console.WriteLine();
		}

		static async Task<Object> GetArgument(MethodInfo method, String input) {
			var parameterType = method.GetParameters().Single().ParameterType;
			if (parameterType == typeof(String)) {
				return await ReadString(input);
			}

			if (parameterType.IsArray && parameterType.GetArrayRank() == 2) {
				return await Read2dArray(parameterType, input);
			}

			var enumerable = GetEnumerable(parameterType);
			if (enumerable == null) {
				return await ReadObject(parameterType, input);
			}

			var lineType = enumerable.GetGenericArguments().Single();
			var nestedEnumerable = GetEnumerable(lineType);
			if (lineType == typeof(String) || nestedEnumerable == null) {
				return await ReadArray(lineType, input);
			}

			return await ReadJaggedArray(nestedEnumerable, lineType, input);

			static async Task<Object> ReadString(String input) =>
				(await File.ReadAllTextAsync(input)).Trim();

			static async Task<Object> Read2dArray(Type parameterType, String input) {
				var valueType = parameterType.GetElementType()!;
				var chars = (await File.ReadAllLinesAsync(input)).To2DArray();
				var length1 = chars.GetLength(0);
				var length2 = chars.GetLength(1);
				var array = Array.CreateInstance(valueType, new[] { length1, length2 });
				for (var i = 0; i < length1; ++i) {
					for (var j = 0; j < length2; ++j) {
						array.SetValue(Convert(chars[i, j], valueType), i, j);
					}
				}
				return array;
			}

			static async Task<Object> ReadObject(Type parameterType, String input) =>
				Convert((await File.ReadAllTextAsync(input)).Trim(), parameterType);

			static async Task<Object> ReadArray(Type lineType, String input) {
				var lines = await File.ReadAllLinesAsync(input);
				var array = Array.CreateInstance(lineType, lines.Length);
				for (var i = 0; i < lines.Length; ++i) {
					array.SetValue(Convert(lines[i], lineType), i);
				}

				return array;
			}

			static async Task<Object> ReadJaggedArray(Type nestedEnumerable, Type lineType, String input) {
				var charType = nestedEnumerable.GetGenericArguments().Single();
				var chars = (await File.ReadAllLinesAsync(input)).Select(line => line.ToArray()).ToArray();
				var lineLength = chars.First().Length;
				var matrix = Array.CreateInstance(lineType, chars.Length);
				for (var i = 0; i < chars.Length; ++i) {
					var array = Array.CreateInstance(charType, lineLength);
					for (var j = 0; j < lineLength; ++j) {
						array.SetValue(Convert(chars[i][j], charType), j);
					}
					matrix.SetValue(array, i);
				}
				return matrix;
			}

			static Object Convert(Object value, Type type) =>
				type.IsEnum
					? Enum.ToObject(type, value)
					: type != typeof(Char) && value.GetType() == typeof(Char) && type.GetMethod("Parse", new[] { typeof(String) }) is MethodInfo parse
						? parse.Invoke(null, new[] { value.ToString() })!
						: type == value.GetType()
							? value
							: ((IConvertible) value).ToType(type, null);

			static Type? GetEnumerable(Type type) =>
				type.GetInterfaces().FirstOrDefault(compare => compare.IsGenericType && compare.GetGenericTypeDefinition() == typeof(IEnumerable<>));
		}

		static async Task<Object?> GetResult(MethodInfo method, String output) {
			var resultString = (await File.ReadAllTextAsync(output)).Trim();
			if (resultString == String.Empty) {
				return null;
			}

			var returnType = method.ReturnType;
			if (returnType == typeof(String)) {
				return resultString;
			}

			var parse = returnType.GetMethod("Parse", new[] { typeof(String) });
			return parse!.Invoke(null, new[] { resultString });
		}

		static void RunPart(MethodInfo? method, Object input, Object? expected) {
			if (method == null) {
				Console.Write(new String(' ', 24));
				return;
			}

			var sw = Stopwatch.StartNew();
			var result = method.Invoke(null, new[] { input })!;
			var time = sw.Elapsed;

			var backup = Console.ForegroundColor;
			if (expected != null) {
				Console.ForegroundColor = result.Equals(expected) ? ConsoleColor.Green : ConsoleColor.Red;
			}
			Console.Write($"{result,16}");
			Console.ForegroundColor = backup;
			Console.Write($"{time,8:ss\\.fff}");
		}
	}
}
