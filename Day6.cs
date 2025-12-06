namespace AdventOfCode;

public class Day6 : IBase
{
	private readonly List<string> _input = File.ReadLines("Day6_Data.txt").ToList();
	
	public void Lvl1()
	{
		var row0 = _input[0].Split(' ', StringSplitOptions.RemoveEmptyEntries);
		var row1 = _input[1].Split(' ', StringSplitOptions.RemoveEmptyEntries);
		var row2 = _input[2].Split(' ', StringSplitOptions.RemoveEmptyEntries);
		var row3 = _input[3].Split(' ', StringSplitOptions.RemoveEmptyEntries);
		var ops  = _input[4].Split(' ', StringSplitOptions.RemoveEmptyEntries);

		long sum = 0;

		for (int i = 0; i < ops.Length; i++)
		{
			long n0 = long.Parse(row0[i]);
			long n1 = long.Parse(row1[i]);
			long n2 = long.Parse(row2[i]);
			long n3 = long.Parse(row3[i]);
			char op = ops[i][0];

			long result = op switch
			              {
				              '+' => n0 + n1 + n2 + n3,
				              '*' => n0 * n1 * n2 * n3,
				              _ => throw new InvalidOperationException()
			              };

			sum += result;
		}

		Console.WriteLine(sum); // 3785892992137
	}

	// I used AI for this, I had a crash out figuring this out. Sorry... 
	public void Lvl2()
	{
		var rows = _input.ToArray();
	    int rowCount = rows.Length;
	    if (rowCount < 2) throw new InvalidOperationException("Input must have at least one digit-rows and one operator row.");

	    // Last row contains operators (and spaces)
	    string opRow = rows[rowCount - 1];

	    // Number of columns (width) is the longest row length
	    int cols = rows.Max(r => r.Length);

	    // Problems: each problem holds the parsed numbers and the operator char
	    var problems = new List<(List<long> values, char op)>();

	    // temporary list of column indices that belong to the current problem
	    var currentCols = new List<int>();

	    // scan columns left -> right, grouping columns into problems by blank columns
	    for (int c = 0; c < cols; c++)
	    {
	        if (IsBlankCol(c))
	        {
		        if (currentCols.Count <= 0) continue;
		        // finalize current problem
		        var values = new List<long>(currentCols.Count);
		        foreach (var colIndex in currentCols)
		        {
			        // build number from top rows (excluding operator row)
			        var sb = new System.Text.StringBuilder();
			        for (int r = 0; r < rowCount - 1; r++)
			        {
				        if (colIndex < rows[r].Length)
				        {
					        char ch = rows[r][colIndex];
					        if (ch != ' ') sb.Append(ch);
				        }
			        }

			        if (sb.Length == 0) throw new FormatException($"Empty number in column {colIndex}.");
			        values.Add(long.Parse(sb.ToString()));
		        }

		        // determine operator: look at operator row at the rightmost column of the group,
		        // or any non-space operator inside the group's columns.
		        char op = '+';
		        char found = '\0';
		        for (int k = currentCols.Count - 1; k >= 0; k--)
		        {
			        int ci = currentCols[k];
			        if (ci < opRow.Length && (opRow[ci] == '+' || opRow[ci] == '*'))
			        {
				        found = opRow[ci];
				        break;
			        }
		        }
		        op = found == '\0' ? '+' : found;

		        problems.Add((values, op));
		        currentCols.Clear();
		        // continue (skip blank column)
	        }
	        else
	        {
	            // non-blank column -> belongs to current problem
	            currentCols.Add(c);
	        }
	    }

	    // finalize last group if any
	    if (currentCols.Count > 0)
	    {
	        var values = new List<long>(currentCols.Count);
	        foreach (var colIndex in currentCols)
	        {
	            var sb = new System.Text.StringBuilder();
	            for (int r = 0; r < rowCount - 1; r++)
	            {
	                if (colIndex < rows[r].Length)
	                {
	                    char ch = rows[r][colIndex];
	                    if (ch != ' ') sb.Append(ch);
	                }
	            }

	            if (sb.Length == 0) throw new FormatException($"Empty number in column {colIndex}.");
	            values.Add(long.Parse(sb.ToString()));
	        }

	        char found = '\0';
	        for (int k = currentCols.Count - 1; k >= 0; k--)
	        {
	            int ci = currentCols[k];
	            if (ci < opRow.Length && (opRow[ci] == '+' || opRow[ci] == '*'))
	            {
	                found = opRow[ci];
	                break;
	            }
	        }
	        char op = found == '\0' ? '+' : found;

	        problems.Add((values, op));
	        currentCols.Clear();
	    }

	    // Evaluate each problem and sum results.
	    // Cephalopods described problems right-to-left conceptually, but since addition and multiplication are
	    // commutative, and we only need the sum of each problem result, order doesn't affect the final grand total.
	    long grandTotal = 0;
	    foreach (var p in problems)
	    {
	        if (p.op == '*')
	        {
	            long prod = p.values.Aggregate<long, long>(1, (current, v) => current * v);
	            grandTotal += prod;
	        }
	        else
	        {
	            long s = p.values.Sum();
	            grandTotal += s;
	        }
	    }

	    Console.WriteLine(grandTotal); // 7669802156452
	    return;

	    bool IsBlankCol(int c)
	    {
		    for (int r = 0; r < rowCount; r++)
		    {
			    if (c < rows[r].Length && rows[r][c] != ' ') return false;
		    }
		    return true;
	    }
	}

	public void Run()
	{
		Lvl2();
	}
}