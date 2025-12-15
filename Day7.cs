namespace AdventOfCode;

class Day7 : IBase
{
    private readonly List<string> _input = [.. File.ReadLines("Day7_Data.txt")];

    public void Lvl1()
    {
        var charSet = _input
            .Select(line => line.ToCharArray())
            .ToArray();

        long count = 0L;

        for (int y = 0; y < charSet.Length; y++)
        {
            for (int x = 0; x < charSet[y].Length; x++)
            {

                if (TryGet(y - 1, x, charSet, out char up) && up is 'S' or '|')
                {
                    switch (charSet[y][x])
                    {
                        case '^':
                        {
                            if (TryGet(y, x - 1, charSet, out char left) && left == '.')
                                charSet[y][x - 1] = '|';

                            if (TryGet(y, x + 1, charSet, out char right) && right == '.')
                                charSet[y][x + 1] = '|';

                            count++;
                            break;
                        }
                        case '.':
                            charSet[y][x] = '|';
                            break;
                    }
                }
            }
        }
        Console.WriteLine(count); // 1543
    }

    public void Lvl2()
    {
        var countExits = 0L;
        var height = _input.Count;
        var width = _input[0].Length;
        var start = (0, _input[0].IndexOf('S'));
        
        var beamsQueue =  new Queue<(int Y, int X)>();
        beamsQueue.Enqueue(start);
        var beams = new Dictionary<(int Y, int X), long>
        {
            [start] = 1
        };

        while (beamsQueue.Count > 0)
        {
            var beam = beamsQueue.Dequeue();
            var count = beams[beam];
            beams.Remove(beam);
            var (y, x) =  beam;
            var by = y + 1;
            if (by >= height)
            {
                countExits += count;
                continue;
            }

            switch (_input[by][x]) 
            {
                case '.': EnqueueBeam((by, x), count); break;
                case '^': 
                    var lx = x - 1; if (lx >= 0 && _input[by][lx] == '.') EnqueueBeam((by, lx), count);
                    var rx = x + 1; if (rx  < width && _input[by][rx] == '.') EnqueueBeam((by, rx), count);
                    break;
            }
        }

        Console.WriteLine(countExits); // 3223365367809

        return; 
        
        void EnqueueBeam((int, int) p, long count)
        {
            if (!beams.TryAdd(p, count))
                beams[p] += count;
            if (!beamsQueue.Contains(p)) 
                beamsQueue.Enqueue(p);
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

        c = '\0';
        return false;
    }

}