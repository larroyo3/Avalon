using PostSharp.Aspects;
using PostSharp.Serialization;
using System.Diagnostics;
using System.Reflection;

[PSerializable]
public class TimerAspect : OnMethodBoundaryAspect
{
    // Cannot be serialized
    [NonSerialized]
    private Stopwatch stopwatch;

    public override void RuntimeInitialize(MethodBase method)
    {
        stopwatch = new Stopwatch();
    }

    public override void OnEntry(MethodExecutionArgs args)
    {
        stopwatch.Start();
    }

    public override void OnSuccess(MethodExecutionArgs args)
    {
        stopwatch.Stop();

        Console.WriteLine($"PostSharp - Method '{args.Method.Name}': duration={stopwatch.Elapsed}");
    }
}