// See https://aka.ms/new-console-template for more information
using Lox;
using System.Text;

class Program
{
    private static bool hadError = false;

    static void Main(string[] args)
    {
        if (args.Length > 1)
            Console.WriteLine("Usage: jlox [script]\");");

        if (args.Length == 1)
            runFile(args[0]);

        else
            RunPrompt();
    }

    static void runFile(string path)
    {
        if (hadError) Environment.Exit(0);
        byte[] bytes = File.ReadAllBytes(path);
        string source = Encoding.Default.GetString(bytes);
    }

    static void Run(string source)
    {
        Scanner scanner = new Scanner(source);
        List<Token> tokens = scanner.Scan();
        foreach (Token token in tokens)
        {
            Console.WriteLine(token);
        }
        if (tokens.Any(token => token.Type == TokenType.Error)) 
        {
            var error = tokens.First(toke => toke.Type == TokenType.Error);
            Error(error.Line, error.Literal.ToString());
            return;
        }
    }

    static void RunPrompt()
    {
        using (var input = new StreamReader(Console.OpenStandardInput()))
        {
            for (; ; )
            {
                Console.Write("> ");
                string? line = input.ReadLine();
                if (line == null) break;
                Run(line);
            }
        }
    }

    public static void Error(int line, string message)
    {
        Report(line, "", message);
    }

    static void Report(int line, string where, string message)
    {
        Console.Error.WriteLine($"[line {line}] Error{where}: {message}");
        hadError = true;
    }
}

