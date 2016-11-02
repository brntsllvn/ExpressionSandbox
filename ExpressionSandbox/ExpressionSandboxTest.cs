using System;
using NUnit.Framework;
using Shouldly;
using System.Linq.Expressions;
using System.Reflection.Emit;

namespace ExpressionSandbox
{
    [TestFixture]
    public class ExpressionSandboxTest
    {
        [Test]
        public void ItWorks()
        {
            Func<int, int, int> function = (a, b) => a * b;
            function(2,3).ShouldBe(6);

            Expression<Func<int, int, int>> exp = (a, b) => a + b;

            exp.Parameters[0].Name.ShouldBe("a");
            exp.Parameters[0].IsByRef.ShouldBeFalse();
            exp.Parameters[0].NodeType.ShouldBeOfType<System.Linq.Expressions.ExpressionType>(); 
            exp.Parameters[0].CanReduce.ShouldBe(false); 
            exp.Parameters[1].Name.ShouldBe("b");
            exp.Parameters.Count.ShouldBe(2);

            // all string assertions below are due to expressions
            // being dynamic (i.e. uncompiled runtime expressions)
            var expReturnType = exp.ReturnType.ToString();
            expReturnType.ShouldBe("System.Int32"); // System.RuntimeType

            var body = (BinaryExpression) exp.Body;

            var bodyLiteral = exp.Body.ToString();
            bodyLiteral.ShouldBe("(a + b)"); // System.RuntimeType

            var bodyType = exp.Body.Type.ToString();
            bodyType.ShouldBe("System.Int32"); // System.RuntimeType

            var bodyLeftExpression = body.Left.ToString();
            bodyLeftExpression.ShouldBe("a");
            var bodyRightExpression = body.Right.ToString();
            bodyRightExpression.ShouldBe("b");

            body.NodeType.ShouldBe(ExpressionType.Add);

            //////////

            int result = exp.Compile()(3, 5);
            result.ShouldBe(8);

        }
    }
}
