using System.Text.RegularExpressions;

namespace LibAoc;

public static class Input {

    public static int GetIntNumber(string line) {
        var num = new Regex(@"-?\d+").Match(line);
        return int.Parse(num.Value);
    }

    public static List<int> GetIntNumbers(string line) {
        var nums = new Regex(@"-?\d+").Matches(line);
        return nums.Select(m => int.Parse(m.Value)).ToList();
    }

    public static long GetNumber(string line) {
        var num = new Regex(@"-?\d+").Match(line);
        return long.Parse(num.Value);
    }

    public static List<long> GetNumbers(string line) {
        var nums = new Regex(@"-?\d+").Matches(line);
        return nums.Select(m => long.Parse(m.Value)).ToList();
    }
}
