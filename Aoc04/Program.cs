using LibAoc;

bool InBounds(string pw) {
    var n = int.Parse(pw);
    return 171309 <= n && n <= 643603;
}

bool CheckPart1(string pw) => pw.Pairs().Any(pair => pair.Item1 == pair.Item2);
long CountPasswordsPart1() => Gen(6, 0).Count(pw => InBounds(pw) && CheckPart1(pw));

bool CheckPart2(string pw) => pw.GroupBy(c => c).Select(g => g.Count()).Contains(2);
long CountPasswordsPart2() => Gen(6, 0).Count(pw => InBounds(pw) && CheckPart2(pw));

// Generate ascending digit sequences
IEnumerable<string> Gen(int length, int minDigit) {
    if (length == 0) {
        yield return "";
        yield break;
    }

    for (var i = minDigit; i <= 9; i++) {
        foreach (var pw in Gen(length - 1, i)) {
            yield return i.ToString() + pw;
        }
    }
}

Console.WriteLine(CountPasswordsPart1());
Console.WriteLine(CountPasswordsPart2());
