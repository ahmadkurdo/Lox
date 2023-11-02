using Lox.AST;
using Lox.AST.Abstract;
using Lox.Exceptions;
using Lox.Models;
using System.Text.Json;

namespace Lox
{
    public class Interperter : IExprVisitor<object>, IStmtVisitor
    {
        public void Interpret(List<Stmt> statements) 
        {
            try 
            {
                foreach (Stmt stmt in statements) 
                {
                    Execute(stmt);
                }
            }catch (RuntimeError error) 
            {
                Program.RuntimeError(error);
            }
        }

        public object Eval(Expr expr) => expr.Accept(this);

        public void Execute(Stmt stmt) => stmt.Accept(this);

        public object VisitGroupingExpr(Grouping expr) => Eval(expr.expr);

        public object VisitLiteralExpr(Literal expr) => expr.Value;

        public object VisitBinaryExpr(Binary expr) 
        {
            object left = Eval(expr.Left);
            object right = Eval(expr.Right);

            if (expr is { Operation.Type: TokenType.STAR }
                    or { Operation.Type: TokenType.MINUS }
                    or { Operation.Type: TokenType.SLASH }
                    or { Operation.Type: TokenType.GREATER }
                    or { Operation.Type: TokenType.GREATER_EQUAL }
                    or { Operation.Type: TokenType.LESS }
                    or { Operation.Type: TokenType.LESS_EQUAL })
                CheckNumberOperand(expr.Operation, left, right);

            return
                expr is { Operation.Type: TokenType.STAR } ? (double)left * (double)right :
                expr is { Operation.Type: TokenType.MINUS } ? (double)left - (double)right :
                expr is { Operation.Type: TokenType.SLASH } ? (double)left / (double)right :
                expr is { Operation.Type: TokenType.GREATER } ? (double)left > (double)right :
                expr is { Operation.Type: TokenType.LESS } ? (double)left < (double)right :
                expr is { Operation.Type: TokenType.GREATER_EQUAL } ? (double)left >= (double)right :
                expr is { Operation.Type: TokenType.LESS_EQUAL} ? (double)left <= (double)right :
                expr is { Operation.Type: TokenType.BANG_EQUAL } ? !IsEqual(left, right) :
                expr is { Operation.Type: TokenType.EQUAL_EQUAL } ? IsEqual(left, right) :
                expr is { Operation.Type: TokenType.PLUS } && left.GetType() == typeof(string) && right.GetType() == typeof(string) ? (string)left + (string)right :
                expr is { Operation.Type: TokenType.PLUS } && left.GetType() == typeof(double) && right.GetType() == typeof(double) ? (double)left + (double)right :
                
                expr is { Operation.Type: TokenType.PLUS } && (left.GetType() != typeof(double) || right.GetType() != typeof(double)) &&
                                                              (left.GetType() != typeof(string) || right.GetType() != typeof(string)) ? 
                                                              throw new RuntimeError(expr.Operation, "Operands must be two numbers or two strings.") :
                default;
        }

        public void VisitPrintStmt(PrintStmt printStmt)
        {
            object value = Eval(printStmt.expression);
            Console.WriteLine(JsonSerializer.Serialize(value));
        }

        public void VisitExpressionStmt(ExpressionStmt expressionStmt) => Eval(expressionStmt.expression);

        private bool IsEqual(object left, object right)
        {
            return left == null && right == null ? true :
            left == null ? false : 
            left.Equals(right);
        }

        public object VisitUnaryExpr(Unary expr)
        {
            object right = Eval(expr.right);

            if (expr is { operation.Type: TokenType.MINUS }) 
            {
                CheckNumberOperand(expr.operation, right);
            }

            return
                expr is { operation.Type: TokenType.MINUS } ? -(double)right :
                expr is { operation.Type: TokenType.BANG } ? !IsTruthy(right) : 
                default;
        }

        private bool IsTruthy(object obj) => 
            obj == null ? false : 
            obj.GetType() == typeof(bool) ? (bool)obj : 
            true;
        
        public void CheckNumberOperand(Token op, params object[] operands) 
        {
            if (operands.All(operand => operand is double))
                return;

            throw new RuntimeError(op, "Operand must be a number.");
        }

        public void VisitVarStmt(VarStmt expressionStmt)
        {
            throw new NotImplementedException();
        }
    }
}
