using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SpaceEngineers2DCodeGeneration
{
    public static class QuantityGenerator
    {
        public static void Generate(string directory, string codeNamespace, IEnumerable<Quantity> quantities, IEnumerable<Convertion> convertions)
        {
            var convertionsMappedByQuantityName = convertions.GroupBy(convertion => convertion.QuantityA).ToDictionary(group => group.Key);

            foreach (var quantity in quantities)
            {
                var convertionsForQuantity = new List<Convertion>();
                if (convertionsMappedByQuantityName.TryGetValue(quantity.Name, out var convertionGroup))
                {
                    convertionsForQuantity = convertionGroup.ToList();
                }

                GenerateFile(directory, codeNamespace, quantity.Name, quantity.Accuracy, quantity.Units, convertionsForQuantity);
            }
        }

        private static void GenerateFile(string directory, string codeNamespace, string name, string accuracy, List<Unit> units, List<Convertion> convertions)
        {
            var content = GenerateFileContent(codeNamespace, name, accuracy, units, convertions).ToString();
            File.WriteAllText($"{directory}\\{name}.cs", content);
        }

        private static CodeBuilder GenerateFileContent(string codeNamespace ,string name, string accuracy, List<Unit> units, List<Convertion> convertions)
        {
            var lcName = name.UnCapitalize();

            var code = new CodeBuilder();

            code.WriteLine("using System;");
            code.WriteLine("using System.Collections.Generic;");
            code.WriteLine("using System.Linq;");
            code.WriteLine();

            code.WriteLine($"namespace {codeNamespace}");
            code.OpenBrackets();

            code.WriteLine($"public struct {name}");
            code.OpenBrackets();

            code.WriteLine($"private const double Accuracy = {accuracy};");
            code.WriteLine();

            code.WriteLine("public static IReadOnlyList<Unit> Units = new List<Unit>");
            code.WriteLine("{");
            code.Indent();
            foreach (var unit in units)
            {
                code.WriteLine($"new Unit({unit.Factor}, \"{unit.Symbol}\"),");
            }
            code.UnIndent();
            code.WriteLine("};");
            code.WriteLine();

            code.WriteLine($"public static readonly {name} Zero = new {name}(0);");
            code.WriteLine();

            foreach (var unit in units)
            {
                code.WriteLine($"public static {name} From{unit.Name}(double value)");
                code.OpenBrackets();
                code.WriteLine($"return new {name}(value * {unit.Factor});");
                code.CloseBrackets();
                code.WriteLine();
            }

            code.WriteLine($"public static {name} Sum(IEnumerable<{name}> items)");
            code.OpenBrackets();
            code.WriteLine($"return new {name}(items.Sum(item => item.Value));");
            code.CloseBrackets();
            code.WriteLine();

            code.WriteLine($"public static {name} Average(IEnumerable<{name}> items)");
            code.OpenBrackets();
            code.WriteLine($"return new {name}(items.Average(item => item.Value));");
            code.CloseBrackets();
            code.WriteLine();

            code.WriteLine($"public static {name} Min(IEnumerable<{name}> items)");
            code.OpenBrackets();
            code.WriteLine($"return new {name}(items.Min(item => item.Value));");
            code.CloseBrackets();
            code.WriteLine();

            code.WriteLine($"public static {name} Min(params {name}[] items)");
            code.OpenBrackets();
            code.WriteLine($"return new {name}(items.Min(item => item.Value));");
            code.CloseBrackets();
            code.WriteLine();

            code.WriteLine($"public static {name} Max(IEnumerable<{name}> items)");
            code.OpenBrackets();
            code.WriteLine($"return new {name}(items.Max(item => item.Value));");
            code.CloseBrackets();
            code.WriteLine();

            code.WriteLine($"public static {name} Max(params {name}[] items)");
            code.OpenBrackets();
            code.WriteLine($"return new {name}(items.Max(item => item.Value));");
            code.CloseBrackets();
            code.WriteLine();

            code.WriteLine($"public static bool operator ==({name} a, {name} b)");
            code.OpenBrackets();
            code.WriteLine("return a.Equals(b);");
            code.CloseBrackets();
            code.WriteLine();

            code.WriteLine($"public static bool operator !=({name} a, {name} b)");
            code.OpenBrackets();
            code.WriteLine("return !a.Equals(b);");
            code.CloseBrackets();
            code.WriteLine();

            code.WriteLine($"public static bool operator <({name} a, {name} b)");
            code.OpenBrackets();
            code.WriteLine("return a.Value < b.Value;");
            code.CloseBrackets();
            code.WriteLine();

            code.WriteLine($"public static bool operator <=({name} a, {name} b)");
            code.OpenBrackets();
            code.WriteLine("return a.Value <= b.Value;");
            code.CloseBrackets();
            code.WriteLine();

            code.WriteLine($"public static bool operator >({name} a, {name} b)");
            code.OpenBrackets();
            code.WriteLine("return a.Value > b.Value;");
            code.CloseBrackets();
            code.WriteLine();

            code.WriteLine($"public static bool operator >=({name} a, {name} b)");
            code.OpenBrackets();
            code.WriteLine("return a.Value >= b.Value;");
            code.CloseBrackets();
            code.WriteLine();

            code.WriteLine($"public static {name} operator +({name} a, {name} b)");
            code.OpenBrackets();
            code.WriteLine($"return new {name}(a.Value + b.Value);");
            code.CloseBrackets();
            code.WriteLine();

            code.WriteLine($"public static {name} operator -({name} a)");
            code.OpenBrackets();
            code.WriteLine($"return new {name}(-a.Value);");
            code.CloseBrackets();
            code.WriteLine();

            code.WriteLine($"public static {name} operator -({name} a, {name} b)");
            code.OpenBrackets();
            code.WriteLine($"return new {name}(a.Value - b.Value);");
            code.CloseBrackets();
            code.WriteLine();

            code.WriteLine($"public static double operator /({name} a, {name} b)");
            code.OpenBrackets();
            code.WriteLine("return a.Value / b.Value;");
            code.CloseBrackets();
            code.WriteLine();

            code.WriteLine($"public static {name} operator *({name} a, double b)");
            code.OpenBrackets();
            code.WriteLine($"return new {name}(a.Value * b);");
            code.CloseBrackets();
            code.WriteLine();

            code.WriteLine($"public static {name} operator *(double a, {name} b)");
            code.OpenBrackets();
            code.WriteLine($"return new {name}(a * b.Value);");
            code.CloseBrackets();
            code.WriteLine();

            code.WriteLine($"public static {name} operator /({name} a, double b)");
            code.OpenBrackets();
            code.WriteLine($"return new {name}(a.Value / b);");
            code.CloseBrackets();
            code.WriteLine();

            foreach (var convertion in convertions)
            {
                code.WriteLine($"public static {convertion.Result} operator {convertion.Operator}({convertion.QuantityA} {convertion.QuantityA.UnCapitalize()}, {convertion.QuantityB} {convertion.QuantityB.UnCapitalize()})");
                code.OpenBrackets();
                code.WriteLine(convertion.Formula);
                code.CloseBrackets();
                code.WriteLine();
            }

            code.WriteLine("public readonly double Value;");
            code.WriteLine();

            code.WriteLine("public bool IsZero => Value.Equals(0);");
            code.WriteLine();

            foreach (var unit in units)
            {
                code.WriteLine($"public double In{unit.Name} => Value / {unit.Factor};");
                code.WriteLine();
            }

            code.WriteLine($"public {name}(double value)");
            code.OpenBrackets();
            code.WriteLine("Value = value;");
            code.CloseBrackets();
            code.WriteLine();

            code.WriteLine($"public {name} Abs()");
            code.OpenBrackets();
            code.WriteLine($"return new {name}(Math.Abs(Value));");
            code.CloseBrackets();
            code.WriteLine();

            code.WriteLine("public override bool Equals(object obj)");
            code.OpenBrackets();
            code.WriteLine("if (ReferenceEquals(null, obj))");
            code.OpenBrackets();
            code.WriteLine("return false;");
            code.CloseBrackets();
            code.WriteLine();
            code.WriteLine($"return obj is {name} {lcName} && Equals({lcName});");
            code.CloseBrackets();
            code.WriteLine();

            code.WriteLine($"public bool Equals({name} other)");
            code.OpenBrackets();
            code.WriteLine("return Math.Abs(Value - other.Value) < Accuracy;");
            code.CloseBrackets();
            code.WriteLine();

            code.WriteLine("public override int GetHashCode()");
            code.OpenBrackets();
            code.WriteLine("return Value.GetHashCode();");
            code.CloseBrackets();
            code.WriteLine();

            code.WriteLine("public override string ToString()");
            code.OpenBrackets();
            code.WriteLine("return UnitUtility.Format(Units, Value);");
            code.CloseBrackets();
            code.WriteLine();

            code.WriteLine("public class Unit : IUnit");
            code.OpenBrackets();
            code.WriteLine("public double Factor { get; }");
            code.WriteLine();
            code.WriteLine("public string Symbol { get; }");
            code.WriteLine();
            code.WriteLine("public Unit(double factor, string symbol)");
            code.OpenBrackets();
            code.WriteLine("Factor = factor;");
            code.WriteLine("Symbol = symbol;");
            code.CloseBrackets();
            code.CloseBrackets();
 
            code.CloseBrackets();
            code.CloseBrackets();

            return code;
        }

        public class Quantity
        {
            public string Name { get; }

            public string Accuracy { get; }

            public List<Unit> Units { get; }

            public Quantity(string name, string accuracy, List<Unit> units)
            {
                Name = name;
                Accuracy = accuracy;
                Units = units;
            }
        }

        public class Unit
        {
            public string Symbol { get; }

            public string Name { get; }

            public string Factor { get; }

            public Unit(string symbol, string name, string factor)
            {
                if (string.IsNullOrWhiteSpace(factor))
                {
                    factor = "1";
                }

                Symbol = symbol;
                Name = name;
                Factor = factor;
            }
        }

        public class Convertion
        {
            public Convertion(string quantityA, string @operator, string quantityB, string result, string formula)
            {
                Operator = @operator;
                QuantityA = quantityA;
                QuantityB = quantityB;
                Result = result;
                Formula = formula;
            }

            public string Operator { get; }

            public string QuantityA { get; }

            public string QuantityB { get; }

            public string Result { get; }

            public string Formula { get; }
        }
    }
}
