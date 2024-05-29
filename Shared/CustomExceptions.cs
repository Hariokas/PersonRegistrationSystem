namespace Shared;

public class UserNotFoundException(string message) : Exception(message);

public class UserAlreadyExistsException(string message) : Exception(message);

public class InvalidCredentialsException(string message) : Exception(message);

public class UserValidationException(string message) : Exception(message);