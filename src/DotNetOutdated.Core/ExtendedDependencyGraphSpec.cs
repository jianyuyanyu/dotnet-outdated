using System.Text.Json;
using NuGet.ProjectModel;

namespace DotNetOutdated
{
    public class ExtendedDependencyGraphSpec : DependencyGraphSpec
    {
        public ExtendedDependencyGraphSpec(string json)
            : base()
        {
            // Parse the JSON and initialize the object.
            using var jsonDocument = JsonDocument.Parse(json);

            if (jsonDocument.RootElement.TryGetProperty("projects", out JsonElement projects))
            {
                foreach (var project in projects.EnumerateObject())
                {
                    var projectName = project.Value.GetProperty("restore").GetProperty("projectName").GetString();
                    var packageSpec = JsonPackageSpecReader.GetPackageSpec(project.Value.GetRawText(), projectName, project.Name);
                    this.AddProject(packageSpec);
                    this.AddRestore(packageSpec.RestoreMetadata.ProjectUniqueName);
                }
            }
        }
    }
}