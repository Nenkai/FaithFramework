
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using SF = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace NenTools.ImGui.Generator;

public static class XmlComments
{
    private static TMember AddSimple<TMember>(this TMember member, XmlElementSyntax xmlElement) where TMember : MemberDeclarationSyntax
    {
        return member.WithLeadingTrivia(
                SF.TriviaList(
                    SF.Trivia(
                        SF.DocumentationComment(
                            xmlElement,
                            SF.XmlText().WithTextTokens(
                                SF.TokenList(
                                    SF.Token(
                                        SF.TriviaList(),
                                        SyntaxKind.XmlTextLiteralNewLineToken,
                                        Environment.NewLine,
                                        Environment.NewLine,
                                        SF.TriviaList()
                                    )
                                )
                            )
                        )
                    )
                )
            );
    }

    public static PropertyDeclarationSyntax AddValue(this PropertyDeclarationSyntax property, string value)
    {
        return property.AddSimple(
            SF.XmlValueElement(
                SF.XmlText(value)
            ));
    }
    public static TMember AddSummary<TMember>(this TMember member, IList<string> lines, int numTabs = 0) where TMember : MemberDeclarationSyntax
    {
        var list = new List<XmlNodeSyntax>();
        list.Add(SF.XmlText("\n"));
        for (int i = 0; i < lines.Count; i++)
        {
            string? line = lines[i];
            list.Add(SF.XmlText(new string(' ', numTabs * 4) + "///" + line.Replace("//", string.Empty)));
            list.Add(SF.XmlEmptyElement("br"));
            list.Add(SF.XmlText("\n"));
        }
        list.Add(SF.XmlText(new string(' ', numTabs * 4) + "///"));

        return member.AddSimple(
            SF.XmlSummaryElement(
                SyntaxFactory.List(list)
            ));
    }
}
