using System;

namespace AdventOfCode {
	[AttributeUsage(AttributeTargets.Method)]
	internal class SolverAttribute : Attribute {
		public String Filename { get; }
		public Boolean SplitLines { get; set; } = false;
		public Type ItemType { get; set; } = typeof(String);

		public SolverAttribute(String filename) {
			Filename = filename;
		}
	}
}
