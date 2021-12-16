using System;
using System.Collections.Generic;
using System.Linq;
using z16.Core;

namespace AdventOfCode.Problems.Year2021;

internal static class Day16 {
	public static Decimal Part1(String line) =>
		Parse(line)
			.Recurse(packet => packet.Children)
			.Sum(packet => packet.Version);

	public static Decimal Part2(String line) =>
		Parse(line).Value;

	private static Packet Parse(String line) =>
		line
			.Select(character => character >= 'A' ? character - 'A' + 10 : character - '0')
			.SelectMany(number => 4.Repeat(index => number >> (3 - index) & 1))
			.Let(Parse);

	private static Packet Parse(IEnumerable<Int32> bits) =>
		bits
			.GetEnumerator()
			.Enumerate()
			.Let(enumerable => (
				 Version: enumerable.ToInteger(3),
				 Type: enumerable.ToInteger(3)
			).Let(header => GetChildren(enumerable, header.Type)
				 .Let(childrenResult => GetValue(enumerable, header.Type, childrenResult.Packets)
					 .Let(valueResult => new Packet(header.Version, valueResult.Value, childrenResult.Packets, 6 + childrenResult.Size + valueResult.Size))
				 )
			));

	private static (Decimal Value, Int32 Size) GetValue(IEnumerable<Int32> enumerable, Int32 type, Packet[] children) =>
		type == 4
			? enumerable
				.Chunk(5)
				.TakeUntil(chunk => chunk.First() == 0)
				.ToArray()
				.Let(chunks => (chunks.SelectMany(chunk => chunk.Skip(1)).ToType<Decimal>(), chunks.Length * 5))
			: (type switch {
				0 => children.Sum(packet => packet.Value),
				1 => children.Multiply(packet => packet.Value),
				2 => children.Min(packet => packet.Value),
				3 => children.Max(packet => packet.Value),
				5 => children[0].Value > children[1].Value ? 1L : 0L,
				6 => children[0].Value < children[1].Value ? 1L : 0L,
				7 => children[0].Value == children[1].Value ? 1L : 0L,
			}, 0);

	private static (Packet[] Packets, Int32 Size) GetChildren(IEnumerable<Int32> enumerable, Int32 type) =>
		type == 4
			? (Array.Empty<Packet>(), default)
			: enumerable.ToInteger(1).Let(lengthType => lengthType == 1
				? enumerable
					.ToInteger(11)
					.Repeat(() => Parse(enumerable))
					.ToArray()
					.Let(packets => (packets, 1 + 11 + packets.Sum(packet => packet.Size)))
				: enumerable
					.ToInteger(15)
					.Let(size => new List<Packet>()
						.Do(list => list.Add(Parse(enumerable)), list => list.Sum(packet => packet.Size) < size)
						.Let(list => (list.ToArray(), 1 + 15 + size))
					)
			);

	private static Int32 ToInteger(this IEnumerable<Int32> bits, Int32 size) =>
		bits.Take(size).ToType<Int32>();

	private static T ToType<T>(this IEnumerable<Int32> bits) where T : INumber<T> =>
		bits.Aggregate(default(T)!, (acc, value) => acc * T.Create(2)! + T.Create(value)!);

	private readonly record struct Packet(Int32 Version, Decimal Value, Packet[] Children, Int32 Size);
}
