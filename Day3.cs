namespace AdventOfCode;

public class Day3: IBase
{
	private readonly List<string> _inputs = File.ReadAllLines("Day3_Data.txt").ToList();

	public void Lvl1()
	{
		long sum = 0;
		
		foreach (var input in _inputs)
		{
			var max = 0;
			for (int idx = 0; idx < input.Length - 1; idx++)
			{
				for (int after = 0; after < input.Length; after++)
				{
					if (idx >= after) continue;
					int currJolt = int.Parse(input[idx] + input[after].ToString());
					if (max < currJolt)
					{
						max = currJolt;	
					}
				}
			}
			sum += max;
		}
		
		Console.WriteLine(sum); // 17430
	}

	public void Lvl2()
	{
		long sum = 0;

		foreach (var input in _inputs)
		{
			var stack =  new Stack<int>();
			const int targetLen = 12;
			for (int idx = 0; idx < input.Length; idx++)
			{
				int remainingDigits = input.Length - idx;
				int num = int.Parse(input[idx].ToString());
				while (stack.Count != 0 && stack.Peek() < num &&(stack.Count - 1 + remainingDigits) >= targetLen)
				{
					stack.Pop();
				}
				if (stack.Count < targetLen)
				{
					stack.Push(num);
				}
			}

			var result = string.Join("", stack.Reverse());
			sum += long.Parse(result);
		}
		
		Console.WriteLine(sum); // 171975854269367
	}

	public void Run()
	{
		Lvl2();
	}
}