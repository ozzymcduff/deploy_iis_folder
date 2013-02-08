using NUnit.Framework.Constraints;

// ReSharper disable CheckNamespace
namespace NUnit.Framework
// ReSharper restore CheckNamespace
{

    public class Be : Is { public Be() { } }
    public class Have : Has { public Have() { } }
    public class Contain : Contains { public Contain() { } }

    public static partial class ShouldExtensions
    {
        public static void Should(this object o, IResolveConstraint constraint)
        {
            Assert.That(o, constraint);
        }
        public static void ShouldNot(this object o, Constraint constraint)
        {
            Assert.That(o, new NotOperator().ApplyPrefix(constraint));
        }
    }
}