using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using System.Linq;

namespace somelib
{
    public static class Generator
    {
        private static IEnumerable<MetadataReference> GetGlobalReferences()
        {
            var assemblies = new[]
                {
            typeof(System.Linq.Enumerable).Assembly, 
            typeof(Generator).Assembly,
            typeof(object).Assembly
        };

            var refs = from a in assemblies
                       select MetadataReference.CreateFromFile(a.Location);
            var returnList = refs.ToList();

            //The location of the .NET assemblies
            var assemblyPath = Path.GetDirectoryName(typeof(object).Assembly.Location);

            /* 
                * Adding some necessary .NET assemblies
                * These assemblies couldn't be loaded correctly via the same construction as above,
                * in specific the System.Runtime.
                */
            returnList.Add(MetadataReference.CreateFromFile(Path.Combine(assemblyPath, "mscorlib.dll")));
            returnList.Add(MetadataReference.CreateFromFile(Path.Combine(assemblyPath, "System.dll")));
            returnList.Add(MetadataReference.CreateFromFile(Path.Combine(assemblyPath, "System.Core.dll")));
            returnList.Add(MetadataReference.CreateFromFile(Path.Combine(assemblyPath, "System.Runtime.dll")));

            return returnList;
        }

        public static ISomeThing GenerateInstance(String codeFragment)
        {
            var code = GenerateCode(codeFragment);

            var tree = SyntaxFactory.ParseSyntaxTree(
                code, CSharpParseOptions.Default.WithLanguageVersion(LanguageVersion.Latest));

            string fileName = Path.GetRandomFileName();

            // A single, immutable invocation to the compiler
            // to produce a library
            var compilation = CSharpCompilation.Create(fileName)
              .WithOptions(
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
              .AddReferences(GetGlobalReferences())
              .AddSyntaxTrees(tree);

            using (var ms = new MemoryStream())
            {
                EmitResult compilationResult = compilation.Emit(ms);
                if (compilationResult.Success)
                {
                    ms.Seek(0, SeekOrigin.Begin);

                    // Load the assembly
                    Assembly assembly = Assembly.Load(ms.ToArray());

                    var assemblyType = assembly.GetType("somelib.Instance");

                    ISomeThing instance = (ISomeThing)Activator.CreateInstance(assemblyType);

                    return instance;
                }
                else
                {
                    foreach (Diagnostic codeIssue in compilationResult.Diagnostics)
                    {
                        string issue = $"ID: {codeIssue.Id}, Message: {codeIssue.GetMessage()}, Location: { codeIssue.Location.GetLineSpan()}, Severity: { codeIssue.Severity}";
                        Console.WriteLine(issue);
                    }
                }
            }

            return null;
        }

        private static string GenerateCode(String codeFragment)
        {
            var @namespace = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName("somelib")).NormalizeWhitespace();

            @namespace = @namespace.AddUsings(
                SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("System")),
                SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("System.Linq"))
                );

            var classDeclaration = SyntaxFactory.ClassDeclaration("Instance");

            // Add the public modifier: (public class Order)
            classDeclaration = classDeclaration.AddModifiers(
                SyntaxFactory.Token(SyntaxKind.PublicKeyword),
                SyntaxFactory.Token(SyntaxKind.SealedKeyword)
                );

            // Inherit BaseEntity<T> and implement IHaveIdentity: (public class Order : BaseEntity<T>, IHaveIdentity)
            classDeclaration = classDeclaration.AddBaseListTypes(
                SyntaxFactory.SimpleBaseType(SyntaxFactory.ParseTypeName("ISomeThing")));

            // Create a stament with the body of a method.
            var syntax = SyntaxFactory.ParseStatement(codeFragment);
            
            // Create a method
            var methodDeclaration = SyntaxFactory.MethodDeclaration(SyntaxFactory.ParseTypeName("SomeDelegates.DoStuff"), "DoOneThing")
                .AddModifiers(
                    SyntaxFactory.Token(SyntaxKind.PublicKeyword)
                    )
                .WithBody(SyntaxFactory.Block(syntax));

            // Add the field, the property and method to the class.
            classDeclaration = classDeclaration.AddMembers(methodDeclaration);

            // Add the class to the namespace.
            @namespace = @namespace.AddMembers(classDeclaration);

            // Normalize and get code as string.
            var code = @namespace
                .NormalizeWhitespace()
                .ToFullString();

            return code;
        }
    }
}
