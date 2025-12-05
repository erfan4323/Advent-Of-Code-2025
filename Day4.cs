using System.Text;

namespace AdventOfCode;

public class Day4: IBase
{
	private static readonly List<string> Inputs = File.ReadAllLines("Day4_Data.txt").ToList();
	
	private static readonly List<StringBuilder> Grid = Inputs
	                                                   .Select(line => new StringBuilder(line))
	                                                   .ToList();

	private static readonly int Rows = Grid.Count;
	private static readonly int Cols = Grid[0].Length;
	private readonly int _maxX = Rows - 1;
	private readonly int _maxY = Cols - 1;

	private readonly int[] _dxs = [-1, -1, -1, 0, 0, 1, 1, 1];
	private readonly int[] _dys = [-1, 0, 1, -1, 1, -1, 0, 1];
	
	public void Lvl1()
	{
		int count = 0;
		
		for (int y = 0; y < Rows; y++)
		{
			for (int x = 0; x < Cols; x++)
			{
				if (Grid[y][x] == '@')
				{
					var neighbors = GetNeighbors(x, y);
					var atSigns = neighbors.FindAll(c => c == "@");
					if (atSigns.Count < 4)
					{
						Console.WriteLine($"{x},{y}\t=> {atSigns.Count} / {neighbors.Count}");
						count++;
					}
				}
			}
		}
		
		Console.WriteLine(count); // 1553
	}

	public void Lvl2()
	{
		int outerCount = 0;
		int innerCount = 0;

		while (true)
		{
			for (int y = 0; y < Rows; y++)
			{
				for (int x = 0; x < Cols; x++)
				{
					if (Grid[y][x] == '@')
					{
						if (CountAtSigns(x, y) < 4)
						{
							// Console.WriteLine($"{x},{y}\t=> {atSigns.Count} / {neighbors.Count}");
							Grid[y][x] = 'x';
							innerCount++;
						}
					}
				}
			}
			if (innerCount == 0) break;
			outerCount += innerCount;
		}
		
		Console.WriteLine(outerCount); 
	}

	public void Run()
	{
		Lvl2();
	}
	
	private int CountAtSigns(int x, int y)
	{
		int count = 0;
		for (int k = 0; k < 8; k++)
		{
			int nx = x + _dxs[k];
			int ny = y + _dys[k];
			if (nx >= 0 && nx <= _maxX && ny >= 0 && ny <= _maxY)
			{
				if (Grid[ny][nx] == '@') count++;
			}
		}
		return count;
	}


	private List<string> GetNeighbors(int x, int y)
	{
		var res = new List<string>(8);
		for (int k = 0; k < 8; k++)
		{
			int nx = x + _dxs[k];
			int ny = y + _dys[k];
			if (nx >= 0 && nx <= _maxX && ny >= 0 && ny <= _maxY)
			{
				res.Add(Grid[ny][nx].ToString()); 
			}
		}
		return res;
	}
}