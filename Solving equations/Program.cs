using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices.Marshalling;
// الكود ده ان شاء الله هيكون مصمم انه يحل المعادلات أو بشكل أصح : التعبيرات العددية , زي:
// (9+3)/4 => 3
// (8-0)/8 => 1
// ((9+3)/4)/3 => 1
// 1/0 => Undefined 
// 0/0 => Indeterminated

// في الكود ده ان شاء الله هستخدم خوارزمية اسمها Shunting Yard 

// يوجد شرح مفصل في الREADME 


namespace Solving_equations
{
    internal class Program
    {
        static bool IsOp(string s) { return s == "+" || s == "-" || s == "*" || s == "/"; }
        static bool IsValidInput(string s)
        {
            String Valids = "0123456789+-*/ ()";
            foreach (char c in s)
            {
                if (!Valids.Contains(c)) return false;
            }
            return true;
        }

        static string GetAValidInput()
        {
            string Input;
            do
            {
                Console.WriteLine("Enter your Expression: ");
                Input = Console.ReadLine();
            } while (!IsValidInput(Input));

            return Input;
        }

        static int VerySimpleExpression(int num1, int num2, string op)
        {
            switch (op)
            {
                // لاحظ اننا مش محتاجين بريك عشان احنا بنعمل ريتيرن اصلا 
                case "+": return num1 + num2;
                case "-": return num1 - num2;
                case "*": return num1 * num2;
                case "/":
                    if (num1 == 0 && num2 == 0) { throw new Exception("Indeterminated (0/0)"); }
                    if (num2 == 0) { throw new Exception("Undefined (" + num1 + "/0)"); }

                    return num1 / num2;
            }
            return 0;
        }

        static int SimpleExpressionSolver(List<string> Expression)
        {
            List<int> output = new List<int>();
            List<string> operators = new List<string>();

            foreach (string Exp in Expression)
            {
                if (!IsOp(Exp)) output.Add(Convert.ToInt32(Exp));
                else operators.Add(Exp);
            }

            int i = 0;
            int num1, num2, ans;
            string op;
            while (i < operators.Count && output.Count!=1)
            {
                if (operators[i] == "+" || operators[i] == "-") { i++; continue; }

                num1 = output[i];
                num2 = output[i + 1];
                op = operators[i];

                ans = VerySimpleExpression(num1, num2, op);

                output.RemoveAt(i);
                output.RemoveAt(i);
                output.Insert(i, ans);
                operators.RemoveAt(i);

            }
            i = 0;
            while (i < operators.Count && output.Count != 1)
            {
                num1 = output[i];
                num2 = output[i + 1];
                op = operators[i];

                ans = VerySimpleExpression(num1, num2, op);

                output.RemoveAt(i);
                output.RemoveAt(i);
                output.Insert(i, ans);
                operators.RemoveAt(i);
            }
            return output[0];
        }

        static int ExpressionSolver(List<string> Expression)
        {
            if (!Expression.Contains("("))
                return SimpleExpressionSolver(Expression);

            int close = Expression.IndexOf(")");

            int open = close;
            while (open >= 0 && Expression[open] != "(")
                open--;

            List<string> SubExpression = new List<string>();

            for (int i = open + 1; i < close; i++)
                SubExpression.Add(Expression[i]);

            int result = ExpressionSolver(SubExpression);

            Expression.RemoveRange(open, close - open + 1);

            Expression.Insert(open, result.ToString());

            return ExpressionSolver(Expression);
     
        }

        static List<string> TransformStringIntoExpression(string s)
        {
            List<string> Expression = new List<string>();
            Expression.Add("");
            foreach (char c in s)
            {
                if (c == '(' || c == ')')
                {
                    if (Expression[Expression.Count - 1] == "")
                        Expression.RemoveAt(Expression.Count - 1);

                    Expression.Add(c.ToString());
                    Expression.Add("");
                }
                else if (!IsOp(c.ToString()))
                {
                    Expression[Expression.Count - 1] += c;
                }
                else
                {
                    Expression.Add(c.ToString());
                    Expression.Add("");
                }
            }

            if (Expression[Expression.Count - 1] == "")
                Expression.RemoveAt(Expression.Count - 1);

            Expression.RemoveAll(x => x == "");

            return Expression;
        }

        static void Main(string[] args)
        {
            string Expression = GetAValidInput();
            Console.WriteLine("your answer is : " + ExpressionSolver(TransformStringIntoExpression(Expression)));
        }
    }
}
