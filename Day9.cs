namespace AdventOfCode;

public class Day9 : IBase
{
	private readonly List<string> _input = [.. File.ReadLines("Day9_Data.txt")];
	
    private record Point(long X, long Y);
    private record Interval(long Start, long End);
    private record Slab(double YMid, List<Interval> Intervals);
    
	public void Lvl1()
	{
		var points = ParsePoints(_input);


		long maxArea = points
			.Aggregate<Point, long>(0,
				(current, a) => points
				                .Select(b => Area(a, b))
				                .Prepend(current)
				                .Max()
			);

		Console.WriteLine(maxArea); // 4769758290
	}

	public void Lvl2()
	{
		var points = ParsePoints(_input);
		var polygon = ClosePolygon(points);
		var slabs = BuildSlabs(polygon);
		
		long maxArea = 0;

		for (int i = 0; i < points.Count; i++)
		{
			for (int j = i + 1; j < points.Count; j++)
			{
				var p1 = points[i];
				var p2 = points[j];
            
				var area = Area(p1, p2);
				if (area <= maxArea)
					continue;
            
				if (IsRectangleInsidePolygon(p1, p2, slabs))
				{
					maxArea = area;
				}
			}
		}
    
		Console.WriteLine(maxArea);
	}
	
	public void Run()
	{
		Lvl2();
	}
	
	private static List<Point> ClosePolygon(List<Point> points)
	{
		if (points[0] == points[^1])
			return points;
    
		var closed = new List<Point>(points) { points[0] };
		return closed;
	}

	private static List<Slab> BuildSlabs(List<Point> polygon)
	{
		var ys = polygon
		         .Select(p => p.Y)
		         .Distinct()
		         .OrderBy(y => y)
		         .ToArray();
		if (ys.Length < 2) 
			return [];
		
		var slabs = new List<Slab>();

		for (int i = 0; i < ys.Length - 1; i++)
		{
			var y0 = ys[i];
			var y1 = ys[i + 1];
			if (y0 == y1)
				continue;
			
			var yMid = (y0 + y1) / 2;
			var xs = new List<long>();


			for (int j = 0; j < polygon.Count - 1; j++)
			{
				var p = polygon[j];
				var b = polygon[j + 1];
				
				if (p.X != b.X)
					continue;
				
				var yMin = Math.Min(p.Y, b.Y);
				var yMax = Math.Max(p.Y, b.Y);
				
				if (yMid > yMin && yMid <= yMax)
				{
					xs.Add(p.X); 
				}
			}
			
			xs.Sort();
			
			var intervals = new List<Interval>();
			for (int xi = 0; xi < xs.Count; xi += 2)
			{
				var xA = xs[xi];
				var xB = xs[xi + 1];
				intervals.Add(xA <= xB ? new(xA, xB) : new(xB, xA));
			}

			slabs.Add(new Slab(yMid, intervals));
		}
		
		return slabs;
	}

	private static bool IsRectangleInsidePolygon(Point p1, Point p2, List<Slab> slabs)
	{
		var xLow = Math.Min(p1.X, p2.X);
		var xHigh = Math.Max(p1.X, p2.X);
		var yLow = Math.Min(p1.Y, p2.Y);
		var yHigh = Math.Max(p1.Y, p2.Y);

		foreach (var slab in slabs)
		{
			if (slab.YMid <= yLow || slab.YMid >= yHigh)
				continue; // Slab outside rectangle's Y range

			bool ok = false;
			foreach (var (start, end) in slab.Intervals)
			{
				if (xLow >= start && xHigh <= end)
				{
					ok = true; // Rectangle fits in this interval
					break;
				}
			}
			
			if (!ok)
				return false; // Rectangle extends outside polygon
		}
		
		return true;
	}
	
	private static long Area(Point p1, Point p2) => (Math.Abs(p1.X - p2.X) + 1) * (Math.Abs(p1.Y - p2.Y) + 1);
	
	private static List<Point> ParsePoints(IEnumerable<string> input)
	{
		return input
		       .Select(line => line.Split(','))
		       .Select(parts => new Point(long.Parse(parts[0]), long.Parse(parts[1])))
		       .ToList();
	}
}