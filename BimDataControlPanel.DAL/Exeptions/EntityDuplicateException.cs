namespace BimDataControlPanel.DAL.Exeptions;

public class EntityDuplicateException : Exception
{
    public EntityDuplicateException(string entityName, string propertyName, string propertyValue)
        : base($"Entity \"{entityName}\" with [{propertyName}]=[{propertyValue}] is already created in database:\n" +
               $"[{propertyName}] must be unique")
    { }
}