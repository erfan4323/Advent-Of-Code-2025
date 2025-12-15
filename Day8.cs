namespace AdventOfCode;

public class Day8 : IBase
{
    private readonly List<string> _input = [.. File.ReadLines("Day8_Data.txt")];

    private record Point(int X, int Y, int Z);
    
	public void Lvl1()
	{
		var (pointList, edges) = PopulateEdges();
		
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
		var (pointList, edges) = PopulateEdges();

		var uf = new UnionFind<Point>(pointList);
		var componentCount = pointList.Count;

		foreach (var (a, b, _) in edges)
		{
			if (!uf.Union(a, b)) continue;
			componentCount--;

			if (componentCount != 1) continue;
			long ans = (long)a.X * b.X;
			Console.WriteLine(ans); // 2497445
			break;
		}
	}

	public void Run()
	{
		Lvl2();
	}
	
	private (List<Point>, List<(Point a, Point b, long dist)>) PopulateEdges()
	{
		var points = ParsePoints(_input);
		var edges = BuildEdges(points);

		return (points, edges);
	}

	private static List<Point> ParsePoints(IEnumerable<string> input)
	{
		return input
		       .Select(line => line.Split(','))
		       .Select(parts => new Point(int.Parse(parts[0]), int.Parse(parts[1]), int.Parse(parts[2])))
		       .ToList();
	}

	private static List<(Point a, Point b, long dist)> BuildEdges(IReadOnlyList<Point> points)
	{
		int edgeCount = points.Count * (points.Count - 1) / 2;
		var edges = new List<(Point a, Point b, long dist)>(edgeCount);

		for (int i = 0; i < points.Count; i++)
			for (int j = i + 1; j < points.Count; j++)
			{
				var a = points[i];
				var b = points[j];

				edges.Add((a, b, DistanceSquared(a, b)));
			}

		edges.Sort((x, y) => x.dist.CompareTo(y.dist));
		return edges;
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
