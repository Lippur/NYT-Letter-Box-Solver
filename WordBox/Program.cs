using System.Text.RegularExpressions;

Console.WriteLine("~~~ NYT Games Letter Box Solver ~~~");
Console.WriteLine("Enter letters, starting from top left, moving clockwise:");

var input = Console.ReadLine()?.Replace(" ", "").Replace(",", "");

while (input is null || input.Length < 12)
{
	Console.WriteLine("Please enter all characters!");
	input = Console.ReadLine()?.Replace(" ", "").Replace(",", "");
}

var letters = input.ToLower().ToCharArray();
var words = await File.ReadAllLinesAsync("wordlist.txt");
words = words.Where(word => word.Length > 2).ToArray();

var top = string.Join("", letters[..3]);
var right = string.Join("", letters[3..6]);
var bottom = string.Join("", letters[6..9]);
var left = string.Join("", letters[9..12]);


var allowedLetters = new Regex($"^[{string.Join("", letters)}]+$");
var sameSide = new Regex($"[{top}](?=[{top}])|[{right}](?=[{right}])|[{bottom}](?=[{bottom}])|[{left}](?=[{left}])");

var matches = words
	.Where(word => allowedLetters.IsMatch(word))
	.Where(word => !sameSide.IsMatch(word))
	.OrderByDescending(word => word.ToCharArray().Distinct().Count())
	.ToArray();


Console.WriteLine("--- 1 word ---");

var oneWord = words
	.Where(word => letters.All(word.Contains));

foreach (var word in oneWord)
{
	Console.WriteLine(word);
}

Console.WriteLine("--- 2 words ---");

var wordsByStartingLetter = matches.ToLookup(word => word[0]);
var twoWordCombos = matches
	.Select(
		word =>
		{
			var missingLetters = letters.Where(letter => !word.Contains(letter));
			var secondWords = wordsByStartingLetter[word.Last()].Where(w => missingLetters.All(w.Contains));
			return new KeyValuePair<string, string[]>(word, secondWords.ToArray());
		}
	)
	.Where(pair => pair.Value.Length != 0);

foreach (var combo in twoWordCombos)
{
	Console.WriteLine($"{combo.Key} - {string.Join(", ", combo.Value)}");
}

Console.WriteLine("--- All words ---");
Console.WriteLine(string.Join(", ", matches));