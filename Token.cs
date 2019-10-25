using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JASONParser
{
    public enum Token_Class
    {
        End, Repeat, Else, If, Integer, ElseIf, main, Program,
        Parameters, FunctionName, Read, Float, Then, Until, Write,
        Dot, Semicolon, Comma, LParanthesis, RParanthesis, EqualOp, LessThanOp,
        GreaterThanOp, NotEqualOp, PlusOp, MinusOp, MultiplyOp, DivideOp, Comment, String,
        Idenifier, Constant, And, OR, LCurly, RCurly, Endline, Colon, Assignment, Return, OrSingle, AndSingle, DoubleQoutes, UnknownType
    }

    public class Token
    {
        public string lex;
        public Token_Class token_type;

    }
    public class TokenHelper
    {
        public static Dictionary<string, Token_Class> TokenMap = new Dictionary<string, Token_Class>();
        static void Initialize()
        {

            TokenMap = new Dictionary<string, Token_Class>();
            TokenMap.Add("Main", Token_Class.main);
            TokenMap.Add("if", Token_Class.If);
            TokenMap.Add("elseif", Token_Class.ElseIf);
            TokenMap.Add("end", Token_Class.End);
            TokenMap.Add("else", Token_Class.Else);
            TokenMap.Add("int", Token_Class.Integer);
            TokenMap.Add("parameters", Token_Class.Parameters);
            TokenMap.Add("read", Token_Class.Read);
            TokenMap.Add("repeat", Token_Class.Repeat);
            TokenMap.Add("Program", Token_Class.Program);
            TokenMap.Add("then", Token_Class.Then);
            TokenMap.Add("until", Token_Class.Until);
            TokenMap.Add("write", Token_Class.Write);
            TokenMap.Add("endl", Token_Class.Endline);
            TokenMap.Add("return", Token_Class.Return);
            TokenMap.Add("float", Token_Class.Float);
            TokenMap.Add("string", Token_Class.String);
            TokenMap.Add("Idenifier", Token_Class.Idenifier);


            TokenMap.Add(".", Token_Class.Dot);
            TokenMap.Add(";", Token_Class.Semicolon);
            TokenMap.Add(",", Token_Class.Comma);
            TokenMap.Add("(", Token_Class.LParanthesis);
            TokenMap.Add(")", Token_Class.RParanthesis);
            TokenMap.Add("=", Token_Class.EqualOp);
            TokenMap.Add(":", Token_Class.Colon);
            TokenMap.Add(":=", Token_Class.Assignment);
            TokenMap.Add("<", Token_Class.LessThanOp);
            TokenMap.Add(">", Token_Class.GreaterThanOp);
            TokenMap.Add("<>", Token_Class.NotEqualOp);
            TokenMap.Add("+", Token_Class.PlusOp);
            TokenMap.Add("-", Token_Class.MinusOp);
            TokenMap.Add("*", Token_Class.MultiplyOp);
            TokenMap.Add("/", Token_Class.DivideOp);
            TokenMap.Add("&&", Token_Class.And);
            TokenMap.Add("||", Token_Class.OR);
            TokenMap.Add("{", Token_Class.LCurly);
            TokenMap.Add("}", Token_Class.RCurly);
            TokenMap.Add("|", Token_Class.OrSingle);
            TokenMap.Add("&", Token_Class.AndSingle);
            TokenMap.Add('"'.ToString(), Token_Class.DoubleQoutes);
        }
        public static List<Token> ReadTokens(string filePath)
        {
            Initialize();
            List<Token> listOfTokens = new List<Token>();
            StreamReader sr = new StreamReader(filePath);
            string file = sr.ReadToEnd();
            string removeCharacter = "\n";
            file = file.Replace(removeCharacter, string.Empty);
            string[] lines = file.Split('\r');
            Token temp;
            for (int i = 0; i < lines.Length; i++)
            {

                string[] token = lines[i].Split('\t');
                temp = new Token();
                if (token[0] == "")
                {
                    continue;
                }
                temp.lex = token[0];
                bool z = TokenMap.ContainsKey(token[0]);
                if (z == true)
                {
                    temp.token_type = TokenMap[token[0]];
                }
                else if (temp.lex[0] >= '0' && temp.lex[0] <= '9')
                {
                    temp.token_type = Token_Class.Constant;
                }
                else
                {
                    temp.token_type = Token_Class.Idenifier;
                }
                listOfTokens.Add(temp);
            }

            return listOfTokens;
        }
    }
}
