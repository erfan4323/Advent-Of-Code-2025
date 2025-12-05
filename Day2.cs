namespace AdventOfCode;

public class Day2: IBase
{
	private const string Input = "67562556-67743658,62064792-62301480,4394592-4512674,3308-4582,69552998-69828126,9123-12332,1095-1358,23-48,294-400,3511416-3689352,1007333-1150296,2929221721-2929361280,309711-443410,2131524-2335082,81867-97148,9574291560-9574498524,648635477-648670391,1-18,5735-8423,58-72,538-812,698652479-698760276,727833-843820,15609927-15646018,1491-1766,53435-76187,196475-300384,852101-903928,73-97,1894-2622,58406664-58466933,6767640219-6767697605,523453-569572,7979723815-7979848548,149-216";

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