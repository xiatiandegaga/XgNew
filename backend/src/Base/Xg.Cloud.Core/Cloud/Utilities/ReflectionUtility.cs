using System.Diagnostics;

namespace Cloud.Utilities
{
    public class ReflectionUtility
    {
        public static string GetCurrentMethodFullName(string prefix)
        {
            var currentMethod = new StackFrame(1).GetMethod();
            return $"{prefix}.{currentMethod.DeclaringType.FullName}.{currentMethod.Name}";
        }
    }
}
