using System.Diagnostics;
using Newtonsoft.Json;

namespace Shrak.Utils;

public static class ExecuteUtils
{
    public static async Task<T> ExecuteCommandAsync<T>(string programPath, string arguments) where T : new()
    {
            var resultString = await ExecuteCommandAsync(programPath, arguments);
            var result = JsonConvert.DeserializeObject<T>(resultString);
            return result ?? new T();
    }
    
    public static async Task<string> ExecuteCommandAsync(string programPath, string arguments)
    {
        // create the ProcessStartInfo using "cmd" as the program to be run, and "/c " as the parameters.
        // Incidentally, /c tells cmd that we want it to execute the command that follows, and then exit.
        //Create process
        Process pProcess = new Process();

        string programFullPath = Path.GetFullPath(programPath);
//strCommand is path and file name of command to run
        pProcess.StartInfo.FileName = programFullPath;

//strCommandParameters are parameters to pass to program
        pProcess.StartInfo.Arguments = arguments;

        pProcess.StartInfo.UseShellExecute = false;
        pProcess.StartInfo.CreateNoWindow = true;

//Set output of program to be written to process output stream
        pProcess.StartInfo.RedirectStandardOutput = true;   

//Optional
        pProcess.StartInfo.WorkingDirectory = Path.GetDirectoryName(programFullPath);

//Start the process
        pProcess.Start();

        var readTask = pProcess.StandardOutput.ReadToEndAsync();

        return await readTask;
    }
}