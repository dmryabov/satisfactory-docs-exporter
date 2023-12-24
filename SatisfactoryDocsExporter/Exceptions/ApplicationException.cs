namespace SatisfactoryDocsExporter.Exceptions;

public class ApplicationException(string message, Exception exception) : Exception(message, exception);