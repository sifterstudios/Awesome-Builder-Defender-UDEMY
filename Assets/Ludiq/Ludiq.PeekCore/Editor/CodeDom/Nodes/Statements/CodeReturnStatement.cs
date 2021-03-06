// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace Ludiq.PeekCore.CodeDom
{
    public sealed class CodeReturnStatement : CodeStatement
    {
        public CodeReturnStatement() {}

        public CodeReturnStatement(CodeExpression expression)
        {
            Expression = expression;
        }

        public CodeExpression Expression { get; }

		public override bool IsTerminator => true;

		public override IEnumerable<CodeElement> Children
		{
			get
			{
				foreach(var child in base.Children) yield return child;
				if (Expression != null) yield return Expression;
			}
		}

		protected override void GenerateInner(CodeGenerator generator, CodeStatementEmitOptions emitOptions)
		{
			generator.WriteBlankLineIfJustExitedBlock();
            generator.Write(TokenType.Keyword, "return");
            if (Expression != null)
            {
                generator.Write(TokenType.Space, ' ');
                Expression.Generate(generator);
            }
			generator.WriteStatementEnd(emitOptions);
		}
    }
}
