using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Moq;
using Services;

namespace Tests.ServicesTests;

[TestClass]
public class TokenServiceTests
{
    private Mock<IConfiguration> _configurationMock;
    private TokenService _tokenService;

    [TestInitialize]
    public void Setup()
    {
        _configurationMock = new Mock<IConfiguration>();

        // Mock configuration settings
        _configurationMock.SetupGet(x => x["Jwt:Key"]).Returns("SuperSecretKeyShouldGoHereAndBeQuiteLongOne");
        _configurationMock.SetupGet(x => x["Jwt:Issuer"]).Returns("Issuer");
        _configurationMock.SetupGet(x => x["Jwt:Audience"]).Returns("Audience");
        _configurationMock.SetupGet(x => x["Jwt:ExpireMinutes"]).Returns("60");

        _tokenService = new TokenService(_configurationMock.Object);
    }

    [TestMethod]
    public void GenerateToken_ShouldReturnToken_WhenInputsAreValid()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var username = "testuser";

        // Act
        var token = _tokenService.GenerateToken(userId, username);

        // Assert
        Assert.IsNotNull(token);
        Assert.IsInstanceOfType(token, typeof(string));

        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

        Assert.IsNotNull(jwtToken);
        Assert.AreEqual("Issuer", jwtToken.Issuer);
        Assert.AreEqual("Audience", jwtToken.Audiences.First());
        Assert.AreEqual(userId.ToString(), jwtToken.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
        Assert.AreEqual(username, jwtToken.Claims.First(c => c.Type == ClaimTypes.Name).Value);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void GenerateToken_ShouldThrowException_WhenConfigurationIsMissing()
    {
        // Arrange
        _configurationMock.SetupGet(x => x["Jwt:Key"]).Returns((string)null);

        var userId = Guid.NewGuid();
        var username = "testuser";

        // Act
        _tokenService.GenerateToken(userId, username);
    }
}