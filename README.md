# ExportVars.dll - Rainmeter Plugin

`ExportVars.dll` is a Rainmeter plugin that allows you to copy variables from a source `.ini` file to a destination `.ini` file. The plugin will update the destination file by replacing existing variables with the values from the source file and adding new variables from the source file if they do not already exist in the destination file.

## Features

- Copy variables from one `.ini` file to another.
- Replace existing variables in the destination file with values from the source file.
- Add missing variables to the destination file if they do not exist.
- Execute an action after the export is completed (e.g., activate a Rainmeter config).

## Installation

1. Download the `ExportVars.dll` plugin file.
2. Place the `ExportVars.dll` file in the `Plugins` folder of your Rainmeter installation (usually `C:\Program Files\Rainmeter\Plugins`).
3. Restart Rainmeter to load the plugin.

## Usage

To use the plugin in your Rainmeter skin, follow these steps:

1. Create a new `.ini` file or use an existing one in your Rainmeter skin.
2. Add a `[mExportVars]` measure in your skin's `.ini` file to configure the plugin.

### Example INI Configuration

```ini
[Rainmeter]
Update=1000

[mExportVars]
Measure=Plugin
Plugin=ExportVars.dll
SourceFile="C:\Path\To\Your\Source.ini"
DestinationFile="C:\Path\To\Your\Destination.ini"
OnCompleteAction=[!ActivateConfig "illustro\Main"]
```

### Parameters:

- `SourceFile`: The full path to the source `.ini` file from which variables will be copied.
- `DestinationFile`: The full path to the destination `.ini` file where variables will be copied to.
- `OnCompleteAction`: The action to be executed once the variable export is complete. For example, you can activate a Rainmeter config or execute any other Rainmeter command.

### LeftMouseUpAction:

To trigger the export when a user clicks on the widget, you can bind it to a `LeftMouseUpAction`:

```ini
LeftMouseUpAction=[!RainmeterPluginBang "mExportVars ExecuteBatch 1"]
```

This will execute the export when the user clicks on the widget.

## How it Works

1. **Source and Destination Files**: The plugin reads the source `.ini` file and the destination `.ini` file.
2. **Variable Matching**: The plugin matches variables from the source file and checks if they exist in the destination file.
3. **Updating Variables**: The plugin replaces any existing variables in the destination file with the values from the source file and appends missing variables from the source file.
4. **Execution of Action**: After the export is complete, the plugin can execute a custom action specified in `OnCompleteAction`.

### Example of Source File (`Source.ini`):

```ini
[Variables]
Var1=Value1
Var2=Value2
Var3=Value3
```

### Example of Destination File (`Destination.ini`):

```ini
[Variables]
Var1=OldValue1
Var4=Value4
```

After running the export, the destination file will be updated as follows:

```ini
[Variables]
Var1=Value1  ; Replaced value from source
Var2=Value2  ; Added from source
Var3=Value3  ; Added from source
Var4=Value4  ; Unchanged
```

## Building from Source

If you want to build the plugin from source, follow these steps:

1. Clone or download the repository.
2. Open the solution in Visual Studio.
3. Build the solution in Visual Studio to generate the `ExportVars.dll` file.
4. Copy the built `ExportVars.dll` file to your Rainmeter `Plugins` folder.

## Known Issues

- The plugin currently does not handle complex file structures (e.g., sections inside sections). It only supports simple key-value pairs.
- Ensure the paths to the source and destination files are correct. If the files are missing, an error will be logged in the Rainmeter logs.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Contact

For any questions or issues, feel free to open an issue on GitHub or reach out via the project's page.

