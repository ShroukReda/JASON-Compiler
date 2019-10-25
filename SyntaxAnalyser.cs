using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JASONParser
{
    public class Node
    {
        public List<Node> children = new List<Node>();
        public Token token;
        public Node()
        {
            token = new Token();
        }
        public Node(string emptyToken)
        {
            token = new Token();
            token.lex = emptyToken;
        }
    }
    class SyntaxAnalyser
    {
        static List<Token> currenttokens;
        static int i = 0;
        public static Node Parse(List<Token> Tokens)
        {
            Node root = new Node();
            currenttokens = Tokens;
            root = Program();
            //write your parser code

            return root;
        }
        public static Node match(Token_Class ExpectedToken)
        {
            Node node = new Node();

            //write your pars
            if (currenttokens[i].token_type == ExpectedToken)
            {
                node.token = currenttokens[i];
                i++;
            }
            else
            {
                string s = "Expected" + ExpectedToken + "found" + currenttokens[i].lex;
                node = new Node(s);
            }
            return node;
        }
        public static Node Datatype()
        {
            Node n = new Node("Datatype");
            if (currenttokens[i].token_type == Token_Class.Integer)
            {
                n.children.Add(match(Token_Class.Integer));
            }
            else if (currenttokens[i].token_type == Token_Class.String)
            {
                n.children.Add(match(Token_Class.String));
            }
            else if (currenttokens[i].token_type == Token_Class.Float)
            {
                n.children.Add(match(Token_Class.Float));
            }
            return n;
        }
        public static Node Program()
        {
            Node n = new Node("Program");

            n.children.Add(FuncStatments());

            return n;

        }
        public static Node FunctionCall()
        {
            Node n = new Node("FunctionCall");
            n.children.Add(match(Token_Class.Idenifier));
            n.children.Add(FunctionPart());
            return n;
        }
        public static Node FunctionPart()
        {
            Node n = new Node("FunctionPart");
            if (currenttokens[i].token_type == Token_Class.LParanthesis && currenttokens[i + 1].token_type == Token_Class.Idenifier)
            {
                n.children.Add(match(Token_Class.LParanthesis));
                n.children.Add(match(Token_Class.Idenifier));
                n.children.Add(Part());
                n.children.Add(match(Token_Class.RParanthesis));
            }
            else if (currenttokens[i].token_type == Token_Class.LParanthesis && currenttokens[i + 1].token_type == Token_Class.Constant)
            {
                n.children.Add(match(Token_Class.LParanthesis));
                n.children.Add(match(Token_Class.Constant));
                n.children.Add(Part());
                n.children.Add(match(Token_Class.RParanthesis));
            }
            else if (currenttokens[i].token_type == Token_Class.LParanthesis && currenttokens[i + 1].token_type == Token_Class.RParanthesis)
            {
                n.children.Add(match(Token_Class.LParanthesis));
                n.children.Add(match(Token_Class.RParanthesis));
            }
            return n;
        }
        public static Node Part()
        {
            Node n = new Node("Part");
            if (currenttokens[i].token_type == Token_Class.Comma)
            {
                n.children.Add(match(Token_Class.Comma));
                n.children.Add(match(Token_Class.Idenifier));
                n.children.Add(Part());
            }
            return n;
        }
        public static Node FuncStatments()
        {

            Node n = new Node("FuncStatments");
            if (currenttokens[i].token_type == Token_Class.Integer || currenttokens[i].token_type == Token_Class.String || currenttokens[i].token_type == Token_Class.Float)
            {
                n.children.Add(FuncStat());
                n.children.Add(FuncStatmentsDash());
            }
            return n;

        }
        public static Node FuncStatmentsDash()
        {
            Node n = new Node("FuncStatmentsDash");
            if ((currenttokens[i].token_type == Token_Class.Integer) || (currenttokens[i].token_type == Token_Class.String) || (currenttokens[i].token_type == Token_Class.Float))
            {
                n.children.Add(FuncStat());
                n.children.Add(FuncStatmentsDash());
            }
            return n;

        }
        public static Node FuncStat()
        {
            Node n = new Node("FuncStat");
            if (currenttokens[i].token_type == Token_Class.Integer || currenttokens[i].token_type == Token_Class.String || currenttokens[i].token_type == Token_Class.Float)
            {
                n.children.Add(FuncDec());
                n.children.Add(FuncBody());
            }
            return n;
        }

        public static Node FuncDec()
        {
            Node n = new Node("FuncDec");
            if (currenttokens[i].token_type == Token_Class.Integer || currenttokens[i].token_type == Token_Class.String || currenttokens[i].token_type == Token_Class.Float)
            {
                n.children.Add(Datatype());
                n.children.Add(FuncName());
            }
            return n;
        }
        public static Node FuncName()
        {
            Node n = new Node("FuncName");
            if (currenttokens[i].token_type == Token_Class.Idenifier)
            {
                n.children.Add(match(Token_Class.Idenifier));
                n.children.Add(match(Token_Class.LParanthesis));
                n.children.Add(ParmList());
                n.children.Add(match(Token_Class.RParanthesis));
            }
            return n;
        }
        public static Node FuncBody()
        {
            Node n = new Node("FuncBody");
            if (currenttokens[i].token_type == Token_Class.LCurly)
            {
                n.children.Add(match(Token_Class.LCurly));
                n.children.Add(Statments());
                n.children.Add(ReturnStat());
                n.children.Add(match(Token_Class.RCurly));
            }
            return n;
        }
        public static Node ParmList()
        {
            Node n = new Node("ParmList");
            if (currenttokens[i].token_type == Token_Class.Integer || currenttokens[i].token_type == Token_Class.String || currenttokens[i].token_type == Token_Class.Float)
            {
                n.children.Add(Parm());
                n.children.Add(ParmListDash());
            }
            return n;
        }
        public static Node ParmListDash()
        {
            Node n = new Node("ParmListDash");
            if (currenttokens[i].token_type == Token_Class.Integer || currenttokens[i].token_type == Token_Class.String || currenttokens[i].token_type == Token_Class.Float)
            {
                n.children.Add(Parm());
                n.children.Add(ParmListDash());
                return n;
            }
            else
            {
                return n;
            }
        }
        public static Node Parm()
        {
            Node n = new Node("Parm");
            if (currenttokens[i].token_type == Token_Class.Integer || currenttokens[i].token_type == Token_Class.String || currenttokens[i].token_type == Token_Class.Float)
            {
                n.children.Add(Datatype());
                n.children.Add(ParmDash());
            }
            return n;
        }
        public static Node ParmDash()
        {
            Node n = new Node("ParmDash");
            if (currenttokens[i].token_type == Token_Class.Idenifier)
            {
                n.children.Add(match(Token_Class.Idenifier));
                n.children.Add(ParmDash2());
            }
            return n;
        }
        public static Node ParmDash2()
        {
            Node n = new Node("ParmDash2");

            if (currenttokens[i].token_type == Token_Class.Comma)
            {
                n.children.Add(match(Token_Class.Comma));
                return n;
            }
            else
            {
                return n;
            }
        }
        public static Node Statments()
        {
            Node n = new Node("Statments");
            n.children.Add(Statment());
            if (currenttokens[i].token_type == Token_Class.Read || currenttokens[i].token_type == Token_Class.Write || currenttokens[i].token_type == Token_Class.Repeat || currenttokens[i].token_type == Token_Class.Idenifier || currenttokens[i].token_type == Token_Class.If || (currenttokens[i].token_type == Token_Class.Integer || currenttokens[i].token_type == Token_Class.String || currenttokens[i].token_type == Token_Class.Float))
            {
            n.children.Add(Statments());
            }
            return n;
        }

        public static Node Statment()
        {
            Node nn = new Node("Statment");
            if (currenttokens[i].token_type == Token_Class.Read)
            {
                Node n = new Node("ReadStat");
                n.children.Add(ReadStat());
                n.children.Add(match(Token_Class.Semicolon));
                return n;
            }
            else if (currenttokens[i].token_type == Token_Class.Write)
            {
                Node n = new Node("WriteStat");
                n.children.Add(WriteStat());
                n.children.Add(match(Token_Class.Semicolon));
                return n;
            }
            else if (currenttokens[i].token_type == Token_Class.Repeat)
            {
                Node n = new Node("RepeatStat");
                n.children.Add(RepeatStat());
                return n;
            }
            else if (currenttokens[i].token_type == Token_Class.Idenifier)
            {
                Node n = new Node("AssigStatment");
                n.children.Add(AssigStatement());
                n.children.Add(match(Token_Class.Semicolon));
                return n;
            }
            else if (currenttokens[i].token_type == Token_Class.If)
            {
                Node n = new Node("IfStat");
                n.children.Add(IfStat());
                n.children.Add(match(Token_Class.Semicolon));
                return n;
            }
            else if (currenttokens[i].token_type == Token_Class.Integer || currenttokens[i].token_type == Token_Class.String || currenttokens[i].token_type == Token_Class.Float)
            {
                Node n = new Node("DecStat");
                n.children.Add(Datatype());
                n.children.Add(DecList());
                n.children.Add(match(Token_Class.Semicolon));
                return n;
            }

            else
            {
                return nn;
            }

        }
        public static Node DecList()
        {
            Node n = new Node("DecList");
            n.children.Add(match(Token_Class.Idenifier));
            if (currenttokens[i].token_type == Token_Class.Assignment)
            {
                n.children.Add(match(Token_Class.Assignment));
                n.children.Add(Exp());
            }
            n.children.Add(DecListDash());
            return n;
        }
        public static Node DecListDash()
        {
            Node n = new Node("DecListDash");
            if (currenttokens[i].token_type == Token_Class.Comma)
            {

                n.children.Add(match(Token_Class.Comma));
                n.children.Add(match(Token_Class.Idenifier));
                if (currenttokens[i].token_type == Token_Class.Assignment)
                {
                    n.children.Add(match(Token_Class.Assignment));
                    n.children.Add(Exp());
                }
                n.children.Add(DecListDash());
                return n;
            }
            else
            {
                return n;
            }
        }
        public static Node AssigStatement()
        {
            Node n = new Node("AssigStatement");
            n.children.Add(match(Token_Class.Idenifier));
            n.children.Add(match(Token_Class.Assignment));
            n.children.Add(Exp());

            return n;
        }
        public static Node ReadStat()
        {
            Node n = new Node("ReadStat");
            n.children.Add(match(Token_Class.Read));
            n.children.Add(match(Token_Class.Idenifier));
            return n;
        }
  
        public static Node IfStat()
        {
            Node n = new Node("IfStat");
            if (currenttokens[i].token_type == Token_Class.If)
            {
                n.children.Add(match(Token_Class.If));
                n.children.Add(ConditionStat());
                n.children.Add(match(Token_Class.Then));
                n.children.Add(Statments());
                n.children.Add(IfStatdash());
            }
            return n;

        }
        public static Node IfStatdash()
        {
            Node n = new Node("IfStatdash");

            if (currenttokens[i].token_type == Token_Class.ElseIf)
            {
                n.children.Add(ElseIf());
            }
            else if (currenttokens[i].token_type == Token_Class.Else)
            {
                n.children.Add(ElseStat());

            }
            else if (currenttokens[i].token_type == Token_Class.End)
            {
                n.children.Add(match(Token_Class.End));
            }
            return n;
        }
        public static Node ElseIf()
        {
            Node n = new Node("ElseIf");
            if (currenttokens[i].token_type == Token_Class.ElseIf)
            {
                n.children.Add(match(Token_Class.ElseIf));
                n.children.Add(ConditionStat());
                n.children.Add(match(Token_Class.Then));
                n.children.Add(Statments());
                n.children.Add(ElseIfDash());
            }
            return n;

        }
        public static Node ElseIfDash()
        {
            Node n = new Node("ElseIfDash");
            if (currenttokens[i].token_type == Token_Class.ElseIf)
            {
                n.children.Add(ElseIf());

            }
            else if (currenttokens[i].token_type == Token_Class.Else)
            {
                n.children.Add(ElseStat());
            }
            else if (currenttokens[i].token_type == Token_Class.End)
            {
                n.children.Add(match(Token_Class.End));
            }
            return n;

        }
        public static Node ElseStat()
        {
            Node n = new Node("ElseStat");
            if (currenttokens[i].token_type == Token_Class.Else)
            {
                n.children.Add(match(Token_Class.Else));
                n.children.Add(Statments());
                n.children.Add(match(Token_Class.End));
            }
            else if (currenttokens[i].token_type == Token_Class.End)
            {
                n.children.Add(match(Token_Class.End));
            }

            return n;
        }

       
        public static Node BoolOp()
        {
            Node n = new Node("BoolOp");
            if (currenttokens[i].token_type == Token_Class.And)
            {
                n.children.Add(match(Token_Class.And));
            }
            else if (currenttokens[i].token_type == Token_Class.OR)
            {
                n.children.Add(match(Token_Class.OR));
            }
            return n;
        }
        public static Node Condition()
        {
            Node n = new Node("Condition");
            if (currenttokens[i].token_type == Token_Class.Idenifier)
            {
                n.children.Add(Exp());
                n.children.Add(ConOp());
                n.children.Add(Exp());
            }
            return n;

        }
        public static Node ConditionStat()
        {
            Node n = new Node("ConditionStat");
            if (currenttokens[i].token_type == Token_Class.Idenifier)
            {
                n.children.Add(Condition());
                n.children.Add(BoolSec());
            }
            return n;
        }
        public static Node BoolSec()
        {
            Node n = new Node("BoolSec");
            if (currenttokens[i].token_type==Token_Class.And || currenttokens[i].token_type==Token_Class.OR)
            {
                n.children.Add(BoolOp());
                n.children.Add(Condition());
                n.children.Add(BoolSec());
            }
            return n;
        }
        
        public static Node WriteStat()
        {
            Node n = new Node("WriteStat");
            n.children.Add(match(Token_Class.Write));
            n.children.Add(WriteStatDash());

            return n;
        }
        public static Node WriteStatDash()
        {
            Node n = new Node("WriteStatDash");
            if (currenttokens[i].token_type == Token_Class.DoubleQoutes || currenttokens[i].token_type == Token_Class.Constant || currenttokens[i].token_type == Token_Class.Idenifier)
            {
                n.children.Add(Exp());
            }
            else if (currenttokens[i].token_type == Token_Class.Endline)
            {
                n.children.Add(match(Token_Class.Endline));
            }
            return n;

        }
        public static Node RepeatStat()
        {
            Node n = new Node("RepeatStat");
            n.children.Add(match(Token_Class.Repeat));
            n.children.Add(Statments());
            n.children.Add(match(Token_Class.Until));
            n.children.Add(Condition());
            return n;
        }
        public static Node Exp()
        {
            Node n = new Node("Exp");
            if (currenttokens[i].token_type == Token_Class.Idenifier || currenttokens[i].token_type == Token_Class.Constant)
            {
                n.children.Add(Term());
                n.children.Add(ExpDash());
                // n.children.Add(match(Token_Class.Semicolon));
            }

            return n;

        }
        public static Node ExpDash()
        {
            Node n = new Node("ExpDash");
            if (currenttokens[i].token_type == Token_Class.EqualOp || currenttokens[i].token_type == Token_Class.NotEqualOp || currenttokens[i].token_type == Token_Class.GreaterThanOp || currenttokens[i].token_type == Token_Class.LessThanOp) 
            {
                n.children.Add(ConOp());
                n.children.Add(Exp());
                n.children.Add(ExpDash());
            }
            
            else if (currenttokens[i].token_type == Token_Class.PlusOp || currenttokens[i].token_type == Token_Class.MinusOp || currenttokens[i].token_type == Token_Class.MultiplyOp || currenttokens[i].token_type == Token_Class.DivideOp)
            {
                n.children.Add((AriOp()));
                n.children.Add(Term());
                n.children.Add(ExpDash());
            }

            return n;
        }
        public static Node ConOp()
        {
            Node n = new Node("ConOp");
            if (currenttokens[i].token_type == Token_Class.GreaterThanOp)
            {
                n.children.Add(match(Token_Class.GreaterThanOp));
            }
            else if (currenttokens[i].token_type == Token_Class.LessThanOp)
            {
                n.children.Add(match(Token_Class.LessThanOp));
            }
            else if (currenttokens[i].token_type == Token_Class.NotEqualOp)
            {
                n.children.Add(match(Token_Class.NotEqualOp));
            }
            else if (currenttokens[i].token_type == Token_Class.EqualOp)
            {
                n.children.Add(match(Token_Class.EqualOp));
            }
            return n;

        }
        public static Node AriOp()
        {
            Node n = new Node("AriOp");
            if (currenttokens[i].token_type == Token_Class.PlusOp)
            {
                n.children.Add(match(Token_Class.PlusOp));
            }
            else if (currenttokens[i].token_type == Token_Class.MinusOp)
            {
                n.children.Add(match(Token_Class.MinusOp));
            }
            else if (currenttokens[i].token_type == Token_Class.MultiplyOp)
            {
                n.children.Add(match(Token_Class.MultiplyOp));
            }
            else if (currenttokens[i].token_type == Token_Class.DivideOp)
            {
                n.children.Add(match(Token_Class.DivideOp));
            }
            return n;

        }
        public static Node Term()
        {
            Node n = new Node("Term");
            n.children.Add(Factor());
            n.children.Add(TermDash());

            return n;
        }
        public static Node TermDash()
        {
            Node n = new Node("TermDash");
            if (currenttokens[i].token_type == Token_Class.PlusOp || currenttokens[i].token_type == Token_Class.MinusOp || currenttokens[i].token_type == Token_Class.MultiplyOp || currenttokens[i].token_type == Token_Class.DivideOp)
            {
                n.children.Add((AriOp()));
                n.children.Add(Factor());
                n.children.Add(TermDash());
            }

            return n;

        }
        public static Node Factor()
        {
            Node n = new Node("Factor");
            if (currenttokens[i].token_type == Token_Class.Idenifier)
            {
                if (currenttokens[i + 1].token_type == Token_Class.LParanthesis)
                {
                    n.children.Add(FunctionCall());

                }
                else
                {
                    n.children.Add(match(Token_Class.Idenifier));
                }
            }
            else if (currenttokens[i].token_type == Token_Class.Constant)
            {
                n.children.Add(match(Token_Class.Constant));
            }
            else if (currenttokens[i].token_type == Token_Class.LParanthesis)
            {
                n.children.Add(match(Token_Class.LParanthesis));
                n.children.Add(Exp());
                n.children.Add(match(Token_Class.RParanthesis));
            }
            return n;
        }
        public static Node ReturnStat()
        {
            Node n = new Node("ReturnStat");
            if (currenttokens[i].token_type == Token_Class.Return)
            {
                n.children.Add(match(Token_Class.Return));
                n.children.Add(Exp());
                n.children.Add(match(Token_Class.Semicolon));
            }
            return n;
        }

        //use this function to print the parse tree in TreeView Toolbox
        public static TreeNode PrintParseTree(Node root)
        {
            TreeNode tree = new TreeNode("Parse Tree");
            TreeNode treeRoot = PrintTree(root);
            if (treeRoot != null)
                tree.Nodes.Add(treeRoot);
            return tree;
        }
        static TreeNode PrintTree(Node root)
        {
            if (root == null || root.token == null)
                return null;
            TreeNode tree = new TreeNode(root.token.lex);
            if (root.children.Count == 0)
                return tree;
            foreach (Node child in root.children)
            {
                if (child == null)
                    continue;
                tree.Nodes.Add(PrintTree(child));
            }
            return tree;
        }
    }
}
