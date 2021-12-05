using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using z16.Core;

namespace AdventOfCode {
	internal class Program {
		private static async Task Main(String inputs) {
			var methods = Assembly.GetExecutingAssembly().GetTypes()
				.Where(type => type.Name.StartsWith("Day") && Int32.TryParse(type.Name[3..], out var _) && type.Namespace!.StartsWith($"{nameof(AdventOfCode)}.{nameof(Problems)}.Year"))
				.OrderBy(type => type.FullName!)
				.SelectMany(type => type.GetMethods(BindingFlags.Public | BindingFlags.Static).OrderBy(method => method.Name));
			foreach (var method in methods) {
				await Solve(method, inputs);
			}

			static async Task Solve(MethodInfo method, String inputs) {
				var result = await Invoke(method, inputs);
				Console.WriteLine($"{method.DeclaringType!.FullName!.Split('.').Skip(2).JoinToString('/')}/{method.Name}: {result}");

				static async Task<Object?> Invoke(MethodInfo method, String inputs) {
					try {
						return method.Invoke(null, new[] { await GetArgument(method, inputs) });
					}
					catch (TargetInvocationException ex) {
						return $"Error during execution: {ex.InnerException!.Message}";
					}

					static async Task<Object> GetArgument(MethodInfo method, String inputs) {
						var year = method.DeclaringType!.FullName!.Split('.').ElementAt(2);
						var methodPath = Path.Combine(inputs, year, method.DeclaringType!.Name, method.Name);
						var dayPath = Path.Combine(inputs, year, method.DeclaringType!.Name, "Input");
						var inputPath = File.Exists(methodPath) ? methodPath : dayPath;
						var parameterType = method.GetParameters().Single().ParameterType;
						var enumerable = parameterType.GetInterfaces().FirstOrDefault(@interface => @interface.IsGenericType && @interface.GetGenericTypeDefinition() == typeof(IEnumerable<>));
						if (enumerable == null) {
							return Convert(await File.ReadAllTextAsync(inputPath), parameterType);
						}

						var lineType = enumerable.GetGenericArguments().Single();
						var nestedEnumerable = lineType.GetInterfaces().FirstOrDefault(@interface => @interface.IsGenericType && @interface.GetGenericTypeDefinition() == typeof(IEnumerable<>));
						if (lineType == typeof(String) || nestedEnumerable == null) {
							var lines = await File.ReadAllLinesAsync(inputPath);
							var array = Array.CreateInstance(lineType, lines.Length);
							for (var i = 0; i < lines.Length; ++i)
							{
								array.SetValue(Convert(lines[i], lineType), i);
							}

							return array;
						}

						var charType = nestedEnumerable.GetGenericArguments().Single();
						var chars = (await File.ReadAllLinesAsync(inputPath)).Select(line => line.ToArray()).ToArray();
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

						static Object Convert(Object value, Type type)
							=> type.IsEnum
								? Enum.ToObject(type, value)
								: ((IConvertible)value).ToType(type, null);
					}
				}
			}
		}
	}
}
