namespace AdventOfCode;

public class Day2: IBase
{
	private static readonly string Input = File.ReadLines("Day2_Data.txt").First();

	public void Lvl1()
	{
		long sum = 0;
		var ranges = Input.Split(',').ToList();
		var listRange = ranges.Select(s => {
			var parts = s.Split('-');
			return (begin: long.Parse(parts[0]), end: long.Parse(parts[1]));
		}).ToList();
		foreach (var range in listRange)
		{
			for (long i = range.begin; i < range.end; i++)
			{
				var curr = i.ToString();
				if (curr.Length % 2 != 0) continue;
				var idx = curr.Length / 2;
				var firstHalf = curr[..idx];
				var secondHalf = curr[idx..];
				if (firstHalf == secondHalf)
				{
					sum += i;
				}
			}
		}
		Console.WriteLine(sum); // 54641809925
	}

	public void Lvl2()
	{
		long sum = 0;
		var ranges = Input.Split(',').ToList();
		var listRange = ranges.Select(s => {
			var parts = s.Split('-');
			return (begin: long.Parse(parts[0]), end: long.Parse(parts[1]));
		}).ToList();
		
		foreach (var range in listRange)
		{
			for (long i = range.begin; i < range.end; i++)
			{
				var curr = i.ToString();
				for (int chunkSize = 1; chunkSize <= curr.Length / 2; chunkSize++)
				{
					if (curr.Length % chunkSize != 0) continue;
					var chunks = curr.Chunk(chunkSize).Select(c => new string(c)).ToList();

					if (chunks.Any(c => c != chunks[0])) continue;
					Console.WriteLine($"Repeats '{i}' (chunk size {chunkSize})");
					sum += i;
					break;
				}
			}
		}
		Console.WriteLine(sum); // 73694270688
	}

	public void Run()
	{
		Lvl2();
	}
}