using LibAoc;
using static LibAoc.LogUtils;

int SolvePart1(IEnumerable<string> input) {
    var planets = GetPlanets(input);
    CountOrbits(planets);

    if (EnableLogging) {
        foreach (var planet in planets.Values) {
            Log(planet.Name, planet.OrbitCount);
        }
    }

    return planets.Values.Sum(planet => planet.OrbitCount);
}

int SolvePart2(IEnumerable<string> input) {
    var planets = GetPlanets(input);
    var start = planets["YOU"].Orbits!;
    var startAncestors = OrbitAncestors(start);
    var end = planets["SAN"].Orbits!;
    var endAncestors = OrbitAncestors(end);
    var commonAncestor = startAncestors.First(ancestor => endAncestors.Contains(ancestor));

    return startAncestors.TakeWhile(p => p != commonAncestor).Count()
        + endAncestors.TakeWhile(p => p != commonAncestor).Count();
}

List<string> OrbitAncestors(Planet planet) {
    var result = new List<string>();
    var current = planet;
    while (current != null) {
        result.Add(current.Name);
        current = current.Orbits;
    }

    return result;
}

void CountOrbits(Dictionary<string, Planet> planets) {
    var q = new Queue<Planet>();
    q.Enqueue(planets["COM"]);
    while (q.Any()) {
        var current = q.Dequeue();
        current.OrbitCount = (current.Orbits?.OrbitCount ?? -1) + 1;
        foreach (var planet in current.OrbittedBy) {
            q.Enqueue(planet);
        }
    }
}

Dictionary<string, Planet> GetPlanets(IEnumerable<string> input) {
    var result = new Dictionary<string, Planet>();
    result.Add("COM", new Planet("COM", null));

    foreach (var line in input) {
        if (line.Split(')') is [var orbitted, var orbitting]) {
            if (!result.TryGetValue(orbitted, out var orbittedPlanet)) {
                orbittedPlanet = new Planet(orbitted, null);
                result[orbitted] = orbittedPlanet;
            }

            if (!result.TryGetValue(orbitting, out var orbittingPlanet)) {
                orbittingPlanet = new Planet(orbitting, orbittedPlanet);
                result[orbitting] = orbittingPlanet;
            } else {
                orbittingPlanet.Orbits = orbittedPlanet;
            }

            orbittedPlanet.OrbittedBy.Add(orbittingPlanet);
        }
    }

    return result;
}


if (args.Length == 0) {
    EnableLogging = true;
} else {
    Utils.AocMain(args, SolvePart1);
    Utils.AocMain(args, SolvePart2);
}

public class Planet {
    public string Name { get; }
    public Planet? Orbits { get; set; }
    public List<Planet> OrbittedBy { get; } = new List<Planet>();
    public int OrbitCount { get; set; }

    public Planet(string name, Planet? orbits) {
        Name = name;
        Orbits = orbits;
    }
}
