// See https://aka.ms/new-console-template for more information
using Lox.Models;
using Lox;
using System.Text;
using Lox.Parser;

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

        var parserState = new ParserState(0, tokens);
        var parser = new Parser(parserState);
        var interpreter = new Interperter();
        var res = interpreter.Eval(parser.Parse());
        Console.WriteLine(res);
        if (tokens.Any(token => token.Type == TokenType.Error)) 
        {
            var error = tokens.First(toke => toke.Type == TokenType.Error);
            Error(error.Line, error.Literal.ToString());
            return;
        }
    }

    static void RunPrompt()
    {
        using var input = new StreamReader(Console.OpenStandardInput());
        for (; ; )
        {
            Console.Write("> ");
            string? line = input.ReadLine();
            if (line == null) break;
            Run(line);
        }
    }

    public static void Error(int line, string message)
    {
        Report(line, "", message);
    }

    public static void Error(Token token, string message)
    {
        if (token.Type == TokenType.EOF)
        {
            Report(token.Line, " at end", message);
        }
        else
        {
            Report(token.Line, " at '" + token.Lexeme + "'", message);
        }
    }

    static void Report(int line, string where, string message)
    {
        Console.Error.WriteLine($"[line {line}] Error{where}: {message}");
        hadError = true;
    }
}

