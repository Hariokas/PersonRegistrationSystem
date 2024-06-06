namespace Shared;

public class UserNotFoundException(string message) : Exception(message);

public class PersonNotFoundException(string message) : Exception(message);

public class ResidenceNotFoundException(string message) : Exception(message);

public class UserAlreadyExistsException(string message) : Exception(message);

public class InvalidCredentialsException(string message) : Exception(message);