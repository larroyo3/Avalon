// Aspect de gestion des erreurs
using PostSharp.Aspects;
using PostSharp.Serialization;

[PSerializable]
public class ErrorHandlingAspect : OnExceptionAspect
{
    public override void OnException(MethodExecutionArgs args)
    {
        var exception = args.Exception;
        Console.WriteLine($"Une erreur s'est produite le {DateTime.Now}: {exception.Message}");

        // Facultatif : Rethrow l'exception pour la propager après le traitement
        throw exception;
    }
}
