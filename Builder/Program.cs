using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace Builder
{
    class Program
    {
        static void Main(string[] args)
        {
            var variables = new Variables();
            variables.className = "ManufacturerDetails";
            variables.members.Add(new Member("string", "ManufacturerName"));
            variables.members.Add(new Member("int?", "ManufacturerId"));
            variables.members.Add(new Member("int", "source"));
            variables.members.Add(new Member("bool", "IsPushFTP"));
            variables.members.Add(new Member("DateTime", "LastReceivedDate"));

            var printer = new Printer(variables);
            printer.PrintBuilderClass();
        }
    }

    public class Variables
    {
        public string className { get; set; }
        
        public List<Member> members = new List<Member>();

        public Variables()
        {
            className = "";
        }
    }

    public class Printer
    {
        private Variables v;

        public Printer(Variables v)
        {
            this.v = v;
        }

        public void PrintBuilderClassHeader()
        {
            WriteLine($"public class {v.className}Builder");
            WriteLine("{");
        }

        public void PrintMembersDeclarations()
        {
            foreach (Member item in v.members)
            {
                WriteLine($"\tprivate {item.type} {item.Variable.Privatize()};");
            }
            WriteLine();
        }

        public void PrintWithDefaulValues()
        {
            WriteLine($"\tpublic {v.className}Builder WithDefaultValues()");
            WriteLine("\t{");

            foreach (Member m in v.members)
            {
                WriteLine($"\t\tthis.{m.Variable.Privatize()} = ,");
            }

            WriteLine();
            WriteLine($"\t\treturn this;");
            WriteLine("\t}");
            WriteLine();
        }

        public void PrintWithMethods()
        {
            foreach (Member item in v.members)
            {
                WriteLine($"\tpublic {v.className}Builder With{item.Variable}({item.type} {item.Variable})");
                WriteLine("\t{");
                WriteLine($"\t\tthis.{item.Variable.Privatize()} = {item.Variable};");
                WriteLine($"\t\treturn this;");
                WriteLine("\t}");
                WriteLine();
            }
        }

        public void PrintBuildMethod()
        {
            WriteLine($"\tpublic {v.className} Build()");
            WriteLine("\t{");
            WriteLine($"\t\treturn new {v.className}");
            WriteLine("\t\t{");

            foreach (Member m in v.members)
            {
                WriteLine($"\t\t\t{m.Variable} = {m.Variable.Privatize()},");
            }

            WriteLine("\t\t};");
            WriteLine("\t}");
        }

        public void PrintLastLine()
        {
            WriteLine("}");
        }

        internal void PrintBuilderClass()
        {
            PrintBuilderClassHeader();
            PrintMembersDeclarations();
            PrintWithDefaulValues();
            PrintWithMethods();
            PrintBuildMethod();
            PrintLastLine();
        }
    }

    public class Member
    {
        public Member(string type, string variable)
        {
            this.type = type;
            this.Variable = variable;
        }

        public string type { get; set; }
        public string Variable { get; set; }
    }

    public static class Extensions
    {
        public static string Privatize(this string s)
        {
            string camelCase = "_" + char.ToLower(s[0]) + s.Substring(1);
            return camelCase;
        }
    }
}
