namespace AdventOfCode;

public class Day8 : IBase
{
    private readonly List<string> _input = [.. File.ReadLines("Day8_Data.txt")];

    private record Point(int X, int Y, int Z);
    
	public void Lvl1()
	{
		var pointList = _input.Select(line =>
		                      {
			                      var parts = line.Split(',');
			                      return new Point(
				                      int.Parse(parts[0]),
				                      int.Parse(parts[1]),
				                      int.Parse(parts[2])
			                      );
		                      })
		                      .ToList();

		var edges = new List<(Point a, Point b, long dist)>();

		for (int i = 0; i < pointList.Count; i++)
			for (int j = i + 1; j < pointList.Count; j++)
			{
				edges.Add((
					pointList[i],
					pointList[j],
					DistanceSquared(pointList[i], pointList[j])
				));
			}
		
		edges.Sort((a, b) => a.dist.CompareTo(b.dist));
		
		var uf = new UnionFind<Point>(pointList);

		foreach (var (a, b, _) in edges.Take(1000))
			uf.Union(a, b); 
		
		var sizes = uf.ComponentSizes()
		              .OrderByDescending(x => x)
		              .Take(3)
		              .ToList();
		
		Console.WriteLine($"Number of circuits: {sizes.Count}");
   
		long answer = (long)sizes[0] * sizes[1] * sizes[2];
		Console.WriteLine(answer); // 131150
	}

	public void Lvl2()
	{
		var pointList = _input.Select(line =>
		                      {
			                      var parts = line.Split(',');
			                      return new Point(
				                      int.Parse(parts[0]),
				                      int.Parse(parts[1]),
				                      int.Parse(parts[2])
			                      );
		                      })
		                      .ToList();

		var edges = new List<(Point a, Point b, long dist)>();

		for (int i = 0; i < pointList.Count; i++)
			for (int j = i + 1; j < pointList.Count; j++)
			{
				edges.Add((
					pointList[i],
					pointList[j],
					DistanceSquared(pointList[i], pointList[j])
				));
			}
		
		edges.Sort((a, b) => a.dist.CompareTo(b.dist));
		
		var uf = new UnionFind<Point>(pointList);
		var componentCount = pointList.Count;

		foreach (var (a, b, _) in edges)
		{
			if (!uf.Union(a, b)) continue;
			componentCount--;

			if (componentCount != 1) continue;
			long ans = (long)a.X * b.X;
			Console.WriteLine(ans);
			break;
		}
	}

	public void Run()
	{
		Lvl2();
	}

	private static long DistanceSquared(Point a, Point b)
	{
		long dx = a.X - b.X;
		long dy = a.Y - b.Y;
		long dz = a.Z - b.Z;
		return dx * dx + dy * dy + dz * dz;
	}
}

internal class UnionFind<T> where T : notnull
{
	private readonly Dictionary<T, T> _parent = new();
	private readonly Dictionary<T, int> _size = new();

	public UnionFind(IEnumerable<T> items)
	{
		foreach (var item in items)
		{
			_parent[item] = item;
			_size[item] = 1;
		}
	}

	private T Find(T x)
	{
		if (!_parent[x].Equals(x))
			_parent[x] = Find(_parent[x]);
		return _parent[x];
	}

	public bool Union(T a, T b)
	{
		var ra = Find(a);
		var rb = Find(b);
		if (ra.Equals(rb)) return false;

		_parent[rb] = ra;
		_size[ra] += _size[rb];
		return true;
	}

	public IEnumerable<int> ComponentSizes()
		=> _size.Where(kv => _parent[kv.Key].Equals(kv.Key))
		       .Select(kv => kv.Value);
}
