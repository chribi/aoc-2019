namespace LibAoc;

public record struct Point2D(long Row, long Col) {
    public Point2D Move(Direction d, long count) {
        var (dRow, dCol) = d.AsVector();
        return new Point2D(Row + dRow * count, Col + dCol * count);
    }

    public Point2D Add(Point2D p) {
        return new Point2D(Row + p.Row, Col + p.Col);
    }

    public long DistOrigin() {
        return Math.Abs(Row) + Math.Abs(Col);
    }

    public override string ToString()
    {
        return $"({Row}, {Col})";
    }

    public static implicit operator Point2D((long, long) value) {
        return new Point2D(value.Item1, value.Item2);
    }

    public static implicit operator Point2D((int, int) value) {
        return new Point2D(value.Item1, value.Item2);
    }

    public static Point2D operator +(Point2D a, Point2D b) {
        return new Point2D(a.Row + b.Row, a.Col + b.Col);
    }

    public static Point2D operator -(Point2D a, Point2D b) {
        return new Point2D(a.Row - b.Row, a.Col - b.Col);
    }

    /// <summary>
    /// Manhattan distance
    /// </summary>
    public static long Dist(Point2D p, Point2D q) {
        return Math.Abs(p.Row - q.Row) + Math.Abs(p.Col - q.Col);
    }
}

public record struct Line(Point2D P, Point2D Q) {

    public Line(Point2D p, Direction d, long count)
        : this(p, p.Move(d, count))
    { }

    public static implicit operator Line((Point2D, Point2D) value) {
        return new Line(value.Item1, value.Item2);
    }

    public long Length => Point2D.Dist(P, Q);

    public override string ToString()
    {
        return $"{P} -- {Q}";
    }

    public (Direction, long) GetMove() {
        if (P.Row == Q.Row) {
            if (Q.Col > P.Col) {
                return (Direction.R, Length);
            } else {
                return (Direction.L, Length);
            }
        }

        if (P.Col == Q.Row) {
            if (Q.Row > P.Row) {
                return (Direction.D, Length);
            } else {
                return (Direction.U, Length);
            }
        }

        throw new InvalidOperationException("Not a axis parallel line");
    }

    public Point2D GetDiff() {
        return (Q.Row - P.Row, Q.Col - P.Col);
    }

    public Point2D TopLeft => (Math.Min(P.Row, Q.Row), Math.Min(P.Col, Q.Col));
    public Point2D BottomRight => (Math.Max(P.Row, Q.Row), Math.Max(P.Col, Q.Col));

    public static Point2D? IntersectOrtho(Line a, Line b) {
        var (aTop, aLeft) = a.TopLeft;
        var (aBottom, aRight) = a.BottomRight;
        var (bTop, bLeft) = b.TopLeft;
        var (bBottom, bRight) = b.BottomRight;

        if (aTop == aBottom) {
            if (bLeft == bRight
                    && bTop <= aTop && aTop <= bBottom
                    && aLeft <= bLeft && bLeft <= aRight) {
                return (aTop, bLeft);
            }
        }

        if (bTop == bBottom) {
            if (aLeft == aRight
                    && aTop <= bTop && bTop <= aBottom
                    && bLeft <= aLeft && aLeft <= bRight) {
                return (bTop, aLeft);
            }
        }

        return null;
    }
}

public class Map2D {
    private char[,] _data;
    public int Width { get; }
    public int Height { get; }

    public Map2D(IList<string> lines) {
        Width = lines.First().Length;
        Height = lines.Count();
        _data = new char[Height, Width];
        for (var row = 0; row < Height; row++) {
            for (var col = 0; col < Width; col++) {
                _data[row, col] = lines[row][col];
            }
        }
    }

    public Map2D(int width, int height, char initial = ' ') {
        Width = width;
        Height = height;
        _data = new char[Height, Width];
        for (var row = 0; row < Height; row++) {
            for (var col = 0; col < Width; col++) {
                _data[row, col] = initial;
            }
        }
    }

    public char this[int row, int col] {
        get => _data[row, col];
        set => _data[row, col] = value;
    }

    public char[] GetRow(int row) {
        var result = new char[Width];
        for (var i = 0; i < Width; i++) result[i] = _data[row, i];
        return result;
    }

    public char[] GetCol(int col) {
        var result = new char[Height];
        for (var i = 0; i < Height; i++) result[i] = _data[i, col];
        return result;
    }

    public IEnumerable<char> Data() {
        foreach (var (row, col) in Positions()) {
            yield return _data[row, col];
        }
    }

    public IEnumerable<(int Row, int Col)> Positions() {
        for (var row = 0; row < Height; row++) {
            for (var col = 0; col < Width; col++) {
                yield return (row, col);
            }
        }
    }

    public void Print() {
        for (var row = 0; row < Height; row++) {
            Console.WriteLine(new string(GetRow(row)));
        }
    }

    public void PrintColored(Func<char, ConsoleColor?> coloring) {
        for (var row = 0; row < Height; row++) {
            for (var col = 0; col < Width; col++) {
                var c = _data[row, col];
                var color = coloring(c);
                if (color is null) {
                    Console.ResetColor();
                } else {
                    Console.BackgroundColor = color.Value;
                }
                Console.Write(c);
            }
            Console.ResetColor();
            Console.WriteLine();
        }
    }
}

public enum Direction { U, R, D, L }

public static class Map2DExtensions {
    public static Direction AsDirection(this char c) {
        return c switch {
            'U' => Direction.U,
            'R' => Direction.R,
            'D' => Direction.D,
            'L' => Direction.L,
            _ => throw new ArgumentOutOfRangeException(nameof(c), c, $"Invalid Direction: {c}"),
        };
    }

    public static (long DRow, long DCol) AsVector(this Direction d) {
        return d switch {
            Direction.U => (-1, 0),
            Direction.R => (0, 1),
            Direction.D => (1, 0),
            Direction.L => (0, -1),
            _ => throw new ArgumentOutOfRangeException(nameof(d)),
        };
    }
}
