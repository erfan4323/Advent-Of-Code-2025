namespace AdventOfCode;

public class Day4: IBase
{
	private static readonly char[][] Grid = File
	                                        .ReadAllLines("Day4_Data.txt")
	                                        .Select(line => line.ToCharArray())
	                                        .ToArray();

	private static readonly int Rows = Grid.Length;
	private static readonly int Cols = Grid[0].Length;
	
	public void Lvl1()
	{
		int count = 0;
		
		for (int y = 0; y < Rows; y++)
		{
			for (int x = 0; x < Cols; x++)
			{
				if (Grid[y][x] == '@' && CountAtSigns(x, y) < 4)
				{
					count++;
				}
			}
		}
		
		Console.WriteLine(count); // 1553
	}

	public void Lvl2()
	{
		int outerCount = 0;

		while (true)
		{
			int innerCount = 0;
			for (int y = 0; y < Rows; y++)
			{
				for (int x = 0; x < Cols; x++)
				{
					if (Grid[y][x] == '@' && CountAtSigns(x, y) < 4)
					{
						Grid[y][x] = 'x';
						innerCount++;
					}
				}
			}
			if (innerCount == 0) break;
			outerCount += innerCount;
		}
		
		Console.WriteLine(outerCount); // 8442
	}

	public void Run()
	{
		Lvl2();
	}
	
	private static int CountAtSigns(int x, int y)
	{
		(int, int)[] directions = [
			(-1,-1), (-1,0), (-1,1),
			(0,-1),          (0,1),
			(1,-1),  (1,0),  (1,1)
		];
		int count = 0;
		foreach (var (dx, dy) in directions)
		{
			int nx = x + dx;
			int ny = y + dy;
			if (nx >= 0 && nx < Cols && ny >= 0 && ny < Rows && Grid[ny][nx] == '@')
				count++;
		}
		return count;
	}
}