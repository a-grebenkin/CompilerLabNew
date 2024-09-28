//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.13.2
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from BASIC.g4 by ANTLR 4.13.2

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

using Antlr4.Runtime.Misc;
using IParseTreeListener = Antlr4.Runtime.Tree.IParseTreeListener;
using IToken = Antlr4.Runtime.IToken;

/// <summary>
/// This interface defines a complete listener for a parse tree produced by
/// <see cref="BASICParser"/>.
/// </summary>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.13.2")]
[System.CLSCompliant(false)]
public interface IBASICListener : IParseTreeListener {
	/// <summary>
	/// Enter a parse tree produced by <see cref="BASICParser.program"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterProgram([NotNull] BASICParser.ProgramContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="BASICParser.program"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitProgram([NotNull] BASICParser.ProgramContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="BASICParser.condition"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterCondition([NotNull] BASICParser.ConditionContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="BASICParser.condition"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitCondition([NotNull] BASICParser.ConditionContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="BASICParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterExpression([NotNull] BASICParser.ExpressionContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="BASICParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitExpression([NotNull] BASICParser.ExpressionContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="BASICParser.term"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterTerm([NotNull] BASICParser.TermContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="BASICParser.term"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitTerm([NotNull] BASICParser.TermContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="BASICParser.operand"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterOperand([NotNull] BASICParser.OperandContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="BASICParser.operand"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitOperand([NotNull] BASICParser.OperandContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="BASICParser.identifier"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterIdentifier([NotNull] BASICParser.IdentifierContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="BASICParser.identifier"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitIdentifier([NotNull] BASICParser.IdentifierContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="BASICParser.number"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterNumber([NotNull] BASICParser.NumberContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="BASICParser.number"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitNumber([NotNull] BASICParser.NumberContext context);
}
