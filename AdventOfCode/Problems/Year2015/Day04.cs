using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using z16.Core;

namespace AdventOfCode.Problems.Year2015;

internal static class Day04 {
	public static Int32 Part1(String line) =>
		Check(line, 5);

	public static Int32 Part2(String line) =>
		Check(line, 6);

	private static Int32 Check(String input, Int32 zeroes) =>
		Enumerable.Range(0, Int32.MaxValue).First(number => Md5(input + number.ToString()).StartsWith(new String('0', zeroes)));

	private static String Md5(String input) =>
		MD5.Create().Use(md5 => BitConverter.ToString(md5.ComputeHash(Encoding.ASCII.GetBytes(input))).Replace("-", String.Empty).ToLowerInvariant());
}
