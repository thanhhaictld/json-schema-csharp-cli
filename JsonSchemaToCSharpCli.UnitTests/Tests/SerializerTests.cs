using System.Text.Json;
using FruitSchema;

public class SerializerTests
{
    // Add your test methods here
    [Fact]
    public void TestAppleSerialization()
    {
        // Arrange
        var options = new JsonSerializerOptions { WriteIndented = true };

        Fruit fruit = new Apple()
        {
            AppleProp1 = "Test",
        };

        var outputDirectory = CreateTestOutputDirectory();
        // Act
        var json = JsonSerializer.Serialize(fruit, options);
        // write to file
        File.WriteAllText(Path.Combine(outputDirectory, "apple.json"), json);
        var deserialized = JsonSerializer.Deserialize<Fruit>(json, options);

        // Assert
        Assert.IsType<Apple>(deserialized);
        Assert.Equal("Test", ((Apple)deserialized).AppleProp1);
    }

    [Fact]
    public void TestOrangeSerialization()
    {
        // Arrange
        var orange = new Orange { OrangeProp1 = "Test" };
        var options = new JsonSerializerOptions { WriteIndented = true };

        var outputDirectory = CreateTestOutputDirectory();
        // Act
        var json = JsonSerializer.Serialize<Fruit>(orange, options);
        File.WriteAllText(Path.Combine(outputDirectory, "orange.json"), json);
        var deserialized = JsonSerializer.Deserialize<Fruit>(json, options);

        // Assert
        Assert.IsType<Orange>(deserialized);
        Assert.Equal("Test", ((Orange)deserialized).OrangeProp1);
    }

    private string CreateTestOutputDirectory()
    {
        var outputDirectory = Path.Combine(Directory.GetCurrentDirectory(), "TestOutputs");
        if (!Directory.Exists(outputDirectory))
        {
            Directory.CreateDirectory(outputDirectory);
        }

        return outputDirectory;
    }
}