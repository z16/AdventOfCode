using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using z16.Core;

namespace AdventOfCode.Problems.Year2020;

internal static class Day4 {
	public static Int32 Part1(String[] lines) => ParseCredentials(lines).Count(credentials => credentials.HasPassportFields);

	public static Int32 Part2(String[] lines) => ParseCredentials(lines).Count(credentials => credentials.HasValidPassportFields);

	private static IEnumerable<Credentials> ParseCredentials(String[] lines)
		=> lines
			.Split(String.Empty)
			.Select(lines => lines.SelectMany(line => line.Split(' ')).Select(item => item.Split(':')).ToDictionary(item => item[0], item => item[1]).AsReadOnly())
			.Select(entry => new Credentials() {
				BirthYear = entry.GetValueOrNull("byr")?.To<Int32>(),
				IssueYear = entry.GetValueOrNull("iyr")?.To<Int32>(),
				ExpirationYear = entry.GetValueOrNull("eyr")?.To<Int32>(),
				HeightAmount = entry.GetValueOrNull("hgt")?[0..^2].ToOrNull<Int32>(),
				HeightUnit = entry.GetValueOrNull("hgt")?[^2..],
				HairColor = entry.GetValueOrNull("hcl"),
				EyeColor = entry.GetValueOrNull("ecl"),
				PassportId = entry.GetValueOrNull("pid"),
				CountryId = entry.GetValueOrNull("cid")
			});

	private record Credentials {
		public Int32? BirthYear { get; init; }
		public Int32? IssueYear { get; init; }
		public Int32? ExpirationYear { get; init; }
		public Int32? HeightAmount { get; init; }
		public String? HeightUnit { get; init; }
		public String? HairColor { get; init; }
		public String? EyeColor { get; init; }
		public String? PassportId { get; init; }
		public String? CountryId { get; init; }

		public Boolean HasPassportFields =>
			BirthYear != null &&
			IssueYear != null &&
			ExpirationYear != null &&
			HeightAmount != null &&
			HeightUnit != null &&
			HairColor != null &&
			EyeColor != null &&
			PassportId != null;

		public Boolean HasValidPassportFields =>
			BirthYear >= 1920 && BirthYear <= 2002 &&
			IssueYear >= 2010 && IssueYear <= 2020 &&
			ExpirationYear >= 2020 && ExpirationYear <= 2030 &&
			(HeightUnit == "cm" && HeightAmount >= 150 && HeightAmount <= 193 || HeightUnit == "in" && HeightAmount >= 59 && HeightAmount <= 76) &&
			HairColor != null && HairColor.Length == 7 && HairColor[0] == '#' && HairColor[1..].All(@char => Char.IsDigit(@char) || @char >= 'a' && @char <= 'f') &&
			EyeColor.OneOf("amb", "blu", "brn", "gry", "grn", "hzl", "oth") &&
			PassportId != null && PassportId.Length == 9 && PassportId.All(@char => Char.IsDigit(@char));
	}
}
