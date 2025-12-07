using System.Reflection.Emit;

namespace AdventOfCode;

class Day7 : IBase
{
    private readonly List<string> input = [.. File.ReadLines("Day7_Data.txt")];

    public void Lvl1()
    {
        var charSet = input
            .Select(line => line.ToCharArray())
            .ToArray();

        long count = 0L;

        for (int y = 0; y < charSet.Length; y++)
        {
            for (int x = 0; x < charSet[y].Length; x++)
            {

                if (TryGet(y - 1, x, charSet, out char up) && (up == 'S' || up == '|'))
                {
                    if (charSet[y][x] == '^')
                    {
                        if (TryGet(y, x - 1, charSet, out char left) && left == '.')
                            charSet[y][x - 1] = '|';

                        if (TryGet(y, x + 1, charSet, out char right) && right == '.')
                            charSet[y][x + 1] = '|';

                        count++;
                    }
                    else if (charSet[y][x] == '.')
                    {
                        charSet[y][x] = '|';
                    }
                }
            }
        }
        Console.WriteLine(count); // 1543
    }

    public void Lvl2()
    {
        var grid = input.Select(line => line.ToCharArray()).ToArray();

        Console.WriteLine(CountTimelines(0, 0));

        long CountTimelines(int y, int x)
        {
            // out of bounds or blocked
            if (y < 0 || y >= grid.Length || x < 0 || x >= grid[y].Length)
                return 0;

            char c = grid[y][x];

            if (c == '|') return 1;  // already counted

            // splitter
            if (c == '^')
            {
                long left = CountTimelines(y, x - 1);
                long right = CountTimelines(y, x + 1);
                return left + right;
            }

            // normal empty space
            if (c == '.')
            {
                grid[y][x] = '|'; // mark visited
                return CountTimelines(y + 1, x); // flow down
            }

            return 0;
        }
    }

    public void Run()
    {
        Lvl2();
    }

    bool TryGet(int y, int x, char[][] charSet, out char c)
    {
        if (y >= 0 && y < charSet.Length &&
            x >= 0 && x < charSet[y].Length)
        {
            c = charSet[y][x];
            return true;
        }

        c = default;
        return false;
    }

}