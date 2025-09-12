using System.Text.Json;
using AnimalSchema;
using DiscriminatorSchema;

public class SerializerTests
{
    
    // Add your test methods here
    [Fact]
    public void TestAppleSerialization()
    {
        // Arrange
        var apple = new Apple { Foo = "Test" };
        var options = new JsonSerializerOptions { WriteIndented = true };
        var outputDirectory = Path.Combine(Directory.GetCurrentDirectory(), "TestOutputs");
        if (!Directory.Exists(outputDirectory))
        {
            Directory.CreateDirectory(outputDirectory);
        }

        // Act
        var json = JsonSerializer.Serialize(apple, options);
        // write to file
        File.WriteAllText(Path.Combine(outputDirectory, "apple.json"), json);
        var deserialized = JsonSerializer.Deserialize<Fruit>(json, options);

        // Assert
        Assert.IsType<Apple>(deserialized);
        Assert.Equal("Test", ((Apple)deserialized).Foo);
    }

    [Fact]
    public void TestOrangeSerialization()
    {
        // Arrange
        var orange = new Orange { Bar = "Test" };
        var options = new JsonSerializerOptions { WriteIndented = true };

        // Act
        var json = JsonSerializer.Serialize(orange, options);
        var deserialized = JsonSerializer.Deserialize<Fruit>(json, options);

        // Assert
        Assert.IsType<Orange>(deserialized);
        Assert.Equal("Test", ((Orange)deserialized).Bar);
    }
}