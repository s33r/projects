# Mass Effect Editor - Command Line Interface

The command line follows this pattern:

``bash
./me.exe <command> [arguments]
``
Each command has its own arguments. Generally these are best to set in the configuration file.

| Command | Description
|---------|--------------
| config  | Configures me.exe 
| save    | Do things related to save files
| option  | Do things released to Coalesced.bin files


## me.exe option

| Command  | Description
|----------|--------------
| get      | Gets one or more values from the coalesced.bin file
| set      | Sets a value in the coalesced.bin file
| backup   | Saves the coalesced.bin file to a separate location
| expand   | Extracts all the files out of the coalesced.bin file and saves them to the specified location
| compress | Compresses all the files in a directory into a coalesced.bin file

### me.exe option get

Searches the coalesced.bin file and returns everything that matches

| Parameter | Alias | Description
|-----------|-------|----------------
| --game    | -g    | The game to search. One of me1, me2, me3
| --file    | -f    | The name of the file to find within coalesced.bin
| --section | -s    | The name of the section to find within coalesced.bin
| --entry   | -e    | The name of the entry to find within coalesced.bin
| --index   | -i    | The index to find within coalesced.bin
| --path    | -p    | Find the values at the provided path within coalesced.bin. Can only be used with the `--type` or `--display` option, other params are ignored.
| --type    | -t    | The kind of match to use when searching.
| --display | -d    | Controls how the output is displayed

When searching, you can use one or more of the file, section, entry or index to return narrow down what your looking for. The `--type` parameter can be used to set how the search is carried out. There are three options:

* __literal__ - The name must match the value exactly (with the exception that all searches are not case sensitive)
* __simple__  - The name must match the simple pattern. Stars (*) are used as placeholders for missing text. This is the default option.
* __regex__   - The name must match the regex (C# style)

All matching results will be returned.

The `--path` parameter treated the coalesced.bin file as a file system where each level is folder. 

The `--display` parameter controls how the results get outputed.

* __json__ - Results are output as a JSON array.
* __csv__ Results are output as a comma separated values with one result per line.
* __fancy__ - Results are output in a pleasant format for the console. This is the default option

### me.exe option get






## me.exe config

The config command makes it easy to configure me.exe so you don't have to provide the same arguments to every command.

### Config file
By default, the config file is called "me.json" it is automaticlly used if it is loacted next

### Parameters

config requires two parameters

| Parameter | Alias | Description
|-----------|-------|----------------
| --key     | -k    | The name of the option
| --value   | -v    | The value of the option

Optionally, you can specify a config file. By default, this is me.json

| Parameter | Alias | Description
|-----------|-------|----------------
| --config  | -c    | The path to the config file to update. If this file doesn't exist, it will be created.



