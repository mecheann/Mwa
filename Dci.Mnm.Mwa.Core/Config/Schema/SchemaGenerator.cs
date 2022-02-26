using System;
using System.IO;
using NJsonSchema;
using NJsonSchema.Generation;
using JsonSchemaGenerator = NJsonSchema.Generation.JsonSchemaGenerator;

namespace Dci.Mnm.Mwa.Core.Config.Schema
{
    public class SchemaGenerator
    {
        public void GenerateAppSettings(string path = "./appsettings.schema")
        {
            var settings = NewMethod();

            var schema = JsonSchema.FromType<Appsettings>(settings);
            clearRequired(schema);

            var schemaData = schema.ToJson();
            File.WriteAllText(path, schemaData);
        }

        private static JsonSchemaGeneratorSettings NewMethod()
        {
            var settings = new JsonSchemaGeneratorSettings();
            settings.AllowReferencesWithProperties = true;
            settings.AlwaysAllowAdditionalObjectProperties = true;
            settings.FlattenInheritanceHierarchy = true;
            settings.GenerateAbstractProperties = false;
            settings.SchemaType = SchemaType.OpenApi3;
            return settings;
        }

        public string GenerateAppSettingsSchemaToStrings()
        {
            // JSchemaGenerator generator = new JSchemaGenerator();
            // generator.GenerationProviders.Add(new StringEnumGenerationProvider());
            // var schema = generator.Generate(typeof(Appsettings));
            var settings = NewMethod();
            var schema = JsonSchema.FromType<Appsettings>(settings);
            clearRequired(schema);
            var schemaData = schema.ToJson();
            return schemaData;
        }

        private void clearRequired(JsonSchema schema)
        {
            schema.RequiredProperties.Clear();
            foreach (var item in schema.Properties)
            {
                clearRequired(item.Value);
            }
        }
    }
}









