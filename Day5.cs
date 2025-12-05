namespace AdventOfCode;


public class Day5 : IBase
{
	private record Range(long Start, long End);

	private List<Range> _ranges = [];
	private List<long> _ids = [];
	
	public void Lvl1()
	{
		GetInputs(out _ranges, out _ids);
		int cnt = _ids.Count(id => _ranges.Any(r => id >= r.Start && id <= r.End));
		Console.WriteLine(cnt); // 615
	}

	public void Lvl2()
	{
		GetInputs(out _ranges, out _ids);

		var sorted = _ranges
			.OrderBy(r => r.Start)
			.ToList();
		
		var nonOverlapping = new List<Range>();

		foreach (var r in sorted)
		{
			if (nonOverlapping.Count == 0)
			{
				nonOverlapping.Add(r);
				continue;
			}

			var last = nonOverlapping[^1];
			if (r.Start <= last.End)
			{
				nonOverlapping[^1] = last with { End = Math.Max(r.End, last.End) };
			}
			else
			{
				nonOverlapping.Add(r);
			}
		}
		
		var sum = nonOverlapping.Sum(GetRangeCount);

		Console.WriteLine(sum); // 353716783056994
	}

	public void Run()	
	{
		Lvl2();
	}

	private static void GetInputs(out List<Range> ranges, out List<long> ids)
	{
		ranges = [];
		ids = [];

		foreach (var raw in File.ReadLines("Day5_Data.txt"))
		{
			var line = raw.Trim();
			if (string.IsNullOrWhiteSpace(line)) continue;
			if (line.Contains('-'))
			{
				var parts = line.Split('-', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
				if (!long.TryParse(parts[0], out var first))
				{
					Console.WriteLine($"There were errors parsing the first line. {line}");
					continue;
				}

				if (!long.TryParse(parts[1], out var second))
				{
					Console.WriteLine($"There were errors parsing the second line. {line}");
					continue;
				}
				ranges.Add(new Range(first, second));
			}
			else
			{
				if (!long.TryParse(line, out var num))
				{
					Console.WriteLine($"There were errors parsing the line. {line}");
					continue;
				}
				ids.Add(num);
			}
		}
	}

	private static long GetRangeCount(Range range) => range.End - range.Start + 1;
}