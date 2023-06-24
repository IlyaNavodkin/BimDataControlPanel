namespace BimDataControlPanel.DAL.Exeptions;

public class EntityMultiDuplicateException : Exception
{
    public EntityMultiDuplicateException(string entityName, string[] propertyNames, string[] propertyValues)
        : base(GenerateExceptionMessage(entityName, propertyNames, propertyValues))
    {
        
    }

    private static string GenerateExceptionMessage(string entityName, string[] propertyNames, string[] propertyValues)
    {
        if (propertyNames.Length != propertyValues.Length)
        {
            throw new ArgumentException("The lengths of propertyNames and propertyValues arrays must match.");
        }

        var properties = new List<string>();
        for (int i = 0; i < propertyNames.Length; i++)
        {
            properties.Add($"[{propertyNames[i]}]=[{propertyValues[i]}]");
        }

        return $"Entity \"{entityName}\" with {string.Join(", ", properties)} is already created in the database.\n" +
               $"[{string.Join(", ", propertyNames)}] must be unique.";
    }
}