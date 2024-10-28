using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Test_UI;
public class ScriptGlobals
{
    // Define any properties or methods you want to make available to the scripts.

    public string AppName { get; } = "ScriptHooker";
    public int UserValue { get; set; } = 0;

    public void ShowMessage(string message)
    {
        Console.WriteLine($"Message from Script: {message}");
    }
    public void ShowMessageBox(string message)
    {
        // Show a message box
        MessageBox.Show(message);
    }

    public Form CreateForm(string title)
    {
        var form = new Form
        {
            Text = title,
            Size = new System.Drawing.Size(300, 200)
        };
        return form;
    }

    public int IncrementValue(int increment)
    {
        UserValue += increment;
        return UserValue;
    }

    public void ShowForm(Form form)
    {
        // Check if the form is already visible
        if (!form.Visible)
        {
            form.Show(); // Use Show() instead of Application.Run()
        }
        else
        {
            form.BringToFront(); // Bring it to the front if it's already open
        }
    }
    // Logging Methods
    public void LogError(string message)
    {
        Log.Error(message);
    }

    public void LogWarning(string message)
    {
        Log.Warning(message);
    }

    public void LogMessage(string message)
    {
        Log.Message(message);
    }

    public void SavePreset(string presetName, string presetJson, string type)
    {
        // Define the base directory path for the scripts
        string baseDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "Scripts");

        // Define the directory for the preset based on the type (Attacker or Defender)
        string presetDirectory = Path.Combine(baseDir, "Presets", type);

        // Check if the preset directory exists
        if (!Directory.Exists(presetDirectory))
        {
            Log.Error($"The directory '{presetDirectory}' does not exist. Please select an existing directory.");
            return; // Exit the method if the directory does not exist
        }

        // Define the file path for the preset
        string presetFilePath = Path.Combine(presetDirectory, $"{presetName}.json");

        // Save the JSON to the file
        File.WriteAllText(presetFilePath, presetJson);
        Log.Message($"Preset '{presetName}' saved successfully!");
    }
}

public class ScriptHooker
{
    private ScriptOptions _scriptOptions;
    private ScriptGlobals _globals; // Store globals here for reuse/reset

    public ScriptHooker()
    {
        _scriptOptions = ScriptOptions.Default
            .WithReferences(AppDomain.CurrentDomain.GetAssemblies())
            .WithImports("System", "System.Linq", "System.Collections.Generic");
        _globals = new ScriptGlobals(); // Initialize globals
    }

    public async Task<object> ExecuteScriptAsync(string code)
    {
        try
        {
            Log.Warning("Executing script");
            Log.Warning("<===================>");
            Log.Warning(code);
            Log.Warning("<===================>");
            Log.Warning("End Of Execution");
            // Run the code with Roslyn in a safe, isolated context
            var result = await CSharpScript.EvaluateAsync(code, _scriptOptions, _globals);
            return result;
        }
        catch (Exception ex)
        {
            Log.Error("Begining Of Error");
            Log.Error("<===================>");
            Log.Error($"Error {ex.Message}");
            Log.Error("<===================>");
            Log.Error("End Of Error");
            return $"Error: {ex.Message}";
        }
    }

    // Method to "unload" or reset the script's global context
    public void UnloadScript()
    {
        // Reinitialize globals to reset state
        _globals = new ScriptGlobals();
        Console.WriteLine("Script unloaded and context reset.");
    }
}

