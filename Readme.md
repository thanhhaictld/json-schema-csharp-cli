# JSON Schema to C# Class Generator

This tool converts JSON Schema definitions to C# classes with support for inheritance patterns.

## Installation

```powershell
dotnet tool install --global JsonSchemaToCSharpCli
```

## Usage

```powershell
jsonschematocsharp <schema-path> <output-path> [options]
```

Options:

- `--namespace` or `-n`: Specify the namespace for generated C# classes (default: "Generated")
- `--version` or `-v`: Display version information

## Creating JSON Schemas with Inheritance

JSON Schema allows you to create inheritance hierarchies using the `discriminator` property in combination with `oneOf`. This approach enables polymorphic behavior in your generated C# classes.

### Example: Fruit Inheritance Hierarchy

In this example, we'll define a `Fruit` base class with various fruit types (Apple, Banana, etc.) inheriting from it.

```json
{
  "$schema": "http://json-schema.org/draft-07/schema#",
  "title": "Fruit", // step 1: define the base class
  "type": "object",
  "discriminator": {
    // step 2: use to specify the property that determines the type during deserialization
    "propertyName": "fruitType"
  },
  "required": ["fruitType"],
  "properties": {
    "fruitType": {
      "type": "string"
    }
  },
  "definitions": {
    "Apple": {
      // step 3: define derived classes
      "type": "object",
      "title": "Apple",
      "allOf": [
        { "$ref": "#" }, // step 4. reference to base class Fruit
        {
          "type": "object",
          "properties": {
            "Seeds": {
              "type": "integer"
            }
          },
          "required": ["Seeds"]
        }
      ]
    },
    "Banana": {
      "type": "object",
      "title": "Banana",
      "allOf": [
        { "$ref": "#" },
        {
          "type": "object",
          "properties": {
            "Length": {
              "type": "number"
            }
          },
          "required": ["Length"]
        }
      ]
    }
  }
}
```

This schema creates:

1. A base `Fruit` class with `fruitType`
2. An `Apple` class inheriting from `Fruit` with additional properties
3. A `Banana` class inheriting from `Fruit` with an additional property

The `fruitType` property serves as the discriminator, determining which concrete class to instantiate when deserializing JSON.

## Key Components for Inheritance

1. **allOf**: Define derived classes using `allOf` to combine the base class with additional properties

```json
{
  "definitions": {
    "Apple": {
      "type": "object",
      "title": "Apple",
      "allOf": [
        { "$ref": "#" }
        // add more specific properties here...
      ]
    },
    "Banana": {
      "type": "object",
      "title": "Banana",
      "allOf": [
        { "$ref": "#" }
        // add more specific properties here for banana...
      ]
    }
  }
}
```

2. **Enum in Discriminator Property**: Each subtype must define its discriminator value

```json
   "fruitType": {
     "type": "string",
     "enum": ["Apple", "Banana" /* add other fruit types here */]
   }
```

1. **Discriminator**: Defines which property determines the concrete type
   ```json
   "discriminator": {
     "propertyName": "fruitType",
    "mapping": { //mapping discriminator values to schema definitions
        "Apple": "#/definitions/Apple",
        "Banana": "#/definitions/Banana"
    }
   }
   ```

## Handling json serialization and deserialization
When working with polymorphic types in C#, you need to configure the JSON serializer to handle the discriminator property correctly. Here's how you can do it using `System.Text.Json`:

```csharp
var apple = new Apple()
    {
        AppleProp1 = "Test",
    };
// explicitly serialize through Fruit base class with JsonConverter decorated
var json = JsonSerializer.Serialize<Fruit>(apple, options);

// explicitly deserialize using discriminator property through Fruit base class with decorated JsonConverter
var deserializedFruit = JsonSerializer.Deserialize<Fruit>(json, options);
```