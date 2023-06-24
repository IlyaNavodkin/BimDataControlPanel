namespace BimDataControlPanel.DAL.Exeptions;

public class NotFoundEntityException : Exception
{
    public NotFoundEntityException(string entityName)
        : base($"Entity \"{entityName}\" not found in data base") { }
}