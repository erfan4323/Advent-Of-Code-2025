namespace AdventOfCode;

public class Day1: IBase
{
	private readonly List<string> _instructions = File.ReadAllLines("Day1_Data.txt").ToList();
	
	public void Lvl1()
	{
		var currIdx = 50;
		const int size = 100;
		int steps = 0;
		int pass = 0;
		foreach (var input in _instructions) {
			var dir = input[0];
			var step = int.Parse(input[1..]);
			if (dir == 'R') {
				currIdx += step;
			}
			else {
				currIdx -= step;
			}
			currIdx = (currIdx % size + size) % size;
			if (currIdx == 0) {
				pass++;
			}
			steps++;
		}

		Console.WriteLine(pass); // 1074
		Console.WriteLine(steps); // 4408
	}

	public void Lvl2()
	{
		var currIdx = 50;
		const int size = 100;
		int steps = 0;
		long pass = 0;
		foreach (var input in _instructions) {
			var dir = input[0];
			var step = int.Parse(input[1..]);
			int before = currIdx;
			long hitsThisMove = 0;
			if (dir == 'R') {
				int firstHit = (size - (before % size)) % size;
				if (firstHit == 0) firstHit = size; // if before == 0, need a full turn

				if (step >= firstHit)
				{
					hitsThisMove = 1 + (step - firstHit) / size;
				}

				int afterRaw = before + step;
				currIdx = afterRaw % size; // >= 0 
			}
			else {
				int firstHit = before % size;
				if (firstHit == 0) firstHit = size;

				if (step >= firstHit)
				{
					hitsThisMove = 1 + (step - firstHit) / size;
				}

				int afterRaw = before - step;
				currIdx = afterRaw % size;
				if (currIdx < 0)
					currIdx += size;
			}

			pass += hitsThisMove;
			steps++;
		}

		Console.WriteLine(pass); //6254
		Console.WriteLine(steps); //4408
	}

	public void Run()
	{
		Lvl2();
	}
}