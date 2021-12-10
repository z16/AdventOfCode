using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using z16.Core;

namespace AdventOfCode {
	internal class Program {
		private static async Task Main(String data) {
			Console.SetCursorPosition(10, Console.GetCursorPosition().Top);
			Console.Write("Part 1".PadLeft(16));
			Console.Write("Part 2".PadLeft(16));
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

			static async Task<Object> GetArgument(MethodInfo method, String inputs) {
				var parameterType = method.GetParameters().Single().ParameterType;
				if (parameterType == typeof(String)) {
					return (await File.ReadAllTextAsync(inputs)).Trim();
				}

				var enumerable = parameterType.GetInterfaces().FirstOrDefault(@interface => @interface.IsGenericType && @interface.GetGenericTypeDefinition() == typeof(IEnumerable<>));
				if (enumerable == null) {
					return Convert((await File.ReadAllTextAsync(inputs)).Trim(), parameterType);
				}

				var lineType = enumerable.GetGenericArguments().Single();
				var nestedEnumerable = lineType.GetInterfaces().FirstOrDefault(@interface => @interface.IsGenericType && @interface.GetGenericTypeDefinition() == typeof(IEnumerable<>));
				if (lineType == typeof(String) || nestedEnumerable == null) {
					var lines = await File.ReadAllLinesAsync(inputs);
					var array = Array.CreateInstance(lineType, lines.Length);
					for (var i = 0; i < lines.Length; ++i)
					{
						array.SetValue(Convert(lines[i], lineType), i);
					}

					return array;
				}

				var charType = nestedEnumerable.GetGenericArguments().Single();
				var chars = (await File.ReadAllLinesAsync(inputs)).Select(line => line.ToArray()).ToArray();
				var lineLength = chars.First().Length;
				var matrix = Array.CreateInstance(lineType, chars.Length);
				for (var i = 0; i < chars.Length; ++i) {
					var array = Array.CreateInstance(charType, lineLength);
					for (var j = 0; j < lineLength; ++j)
					{
						array.SetValue(Convert(chars[i][j], charType), j);
					}
					matrix.SetValue(array, i);
				}
				return matrix;

				static Object Convert(Object value, Type type) =>
					type.IsEnum
						? Enum.ToObject(type, value)
						: ((IConvertible)value).ToType(type, null);
			}

			static async Task<Object?> GetResult(MethodInfo method, String outputs) {
				var resultString = (await File.ReadAllTextAsync(outputs)).Trim();
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
					Console.Write(new String(' ', 16));
					return;
				}

				var result = method.Invoke(null, new[] { input })!;

				var backup = Console.ForegroundColor;
				if (expected != null) {
					Console.ForegroundColor = result.Equals(expected) ? ConsoleColor.Green : ConsoleColor.Red;
				}
				Console.Write($"{result,16}");
				Console.ForegroundColor = backup;
			}
		}
	}
}
