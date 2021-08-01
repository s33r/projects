// Copyright (C) 2021 Aaron C. Willows (aaron@aaronwillows.com)
// 
// This program is free software; you can redistribute it and/or modify it under the terms of the
// GNU Lesser General Public License as published by the Free Software Foundation; either version
// 3 of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY;
// without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See
// the GNU General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License along with this
// program; if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston,
// MA 02111-1307 USA

using System.Collections.Generic;
using System.Linq;
using Aaron.Core.CommandLine.Tokens;

namespace Aaron.Core.CommandLine.Syntax
{
    public static class Parser
    {
        public static ParsedCommandLine Parse(IEnumerable<IToken> tokens,
                                              Dictionary<string, Command> commands,
                                              List<CommandLineError> errors)
        {
            Queue<IToken> tokenQueue = new Queue<IToken>(tokens);

            IToken commandToken = tokenQueue.Dequeue();
            Command command = commands[commandToken.Name];
            Dictionary<string, Parameter> supportedParameters
                = command.Parameters.ToDictionary();

            List<Parameter> parameters = ParseParameters(
                tokenQueue,
                supportedParameters,
                BuildAliasMap(supportedParameters.Values),
                errors,
                out string leftover);

            CheckRequired(tokens, command, errors);

            foreach (Parameter parameter in parameters)
            {
                command.Parameters.SetValue(parameter.Name, parameter.Value);
            }

            ParsedCommandLine commandLine = new ParsedCommandLine
            {
                Command = command, Leftover = leftover, Parameters = new List<Parameter>(),
            };

            commandLine.Errors.AddRange(errors);

            return commandLine;
        }

        public static ParsedCommandLine Parse(IEnumerable<IToken> tokens,
                                              Dictionary<string, Parameter> supportedParameters,
                                              List<CommandLineError> errors)
        {
            if (tokens == null)
            {
                return new ParsedCommandLine
                {
                    Leftover = string.Empty, Command = null, Parameters = new List<Parameter>(),
                };
            }

            Queue<IToken> tokenQueue = new Queue<IToken>(tokens);
            List<Parameter> parameters = ParseParameters(
                tokenQueue,
                supportedParameters,
                BuildAliasMap(supportedParameters.Values),
                errors,
                out string leftover);

            CheckRequired(tokens, supportedParameters.Values.ToList(), errors);

            ParsedCommandLine commandLine = new ParsedCommandLine {Leftover = leftover, Parameters = parameters};

            commandLine.Errors.AddRange(errors);

            return commandLine;
        }

        private static Dictionary<string, Parameter> BuildAliasMap(IEnumerable<Parameter> parameters)
        {
            Dictionary<string, Parameter> aliasMap = new Dictionary<string, Parameter>();

            foreach (Parameter parameter in parameters)
            {
                if (string.IsNullOrEmpty(parameter.Alias)) { continue; }

                aliasMap.Add(parameter.Alias, parameter);
            }


            return aliasMap;
        }

        private static void CheckRequired(IEnumerable<IToken> tokens, Command command, List<CommandLineError> errors)
        {
            CheckRequired(tokens, command.Parameters.ToList(), errors, command.Name);
        }

        private static void CheckRequired(IEnumerable<IToken> tokens,
                                          List<Parameter> parameters,
                                          List<CommandLineError> errors)
        {
            CheckRequired(tokens, parameters, errors, null);
        }

        private static void CheckRequired(IEnumerable<IToken> tokens,
                                          List<Parameter> parameters,
                                          List<CommandLineError> errors,
                                          string commandName)
        {
            HashSet<string> tokenSet = new HashSet<string>(tokens.Select(t => t.Name));

            List<Parameter> requiredParams = parameters.FindAll(p => p.Required);

            foreach (Parameter requiredParam in requiredParams)
            {
                if (!tokenSet.Contains(requiredParam.Name))
                {
                    CommandLineError error = new CommandLineError
                    {
                        CommandName = commandName ?? string.Empty,
                        ParameterName = requiredParam.Name,
                        Fatal = true,
                        Message = commandName == null
                            ? $"Missing required parameter '{requiredParam.Name}'."
                            : $"Missing required parameter '{requiredParam.Name}' for command '{commandName}'.",
                    };


                    errors.Add(error);
                }
            }
        }

        private static List<Parameter> ParseParameters(Queue<IToken> tokens,
                                                       Dictionary<string, Parameter> supportedParameters,
                                                       Dictionary<string, Parameter> aliasMap,
                                                       List<CommandLineError> errors,
                                                       out string leftover)
        {
            leftover = string.Empty;

            List<Parameter> result = new List<Parameter>();

            while (tokens.Count > 0)
            {
                IToken currentToken = tokens.Dequeue();

                if (currentToken.TokenType == TokenTypes.Parameter)
                {
                    Parameter parameter = null;

                    if (supportedParameters.ContainsKey(currentToken.Name))
                    {
                        parameter = supportedParameters[currentToken.Name];
                    }
                    else if (aliasMap.ContainsKey(currentToken.Name)) { parameter = aliasMap[currentToken.Name]; }

                    if (parameter == null)
                    {
                        errors.Add(new CommandLineError
                        {
                            ParameterName = currentToken.Name,
                            Message = $"Unknown parameter '{currentToken.Name}' with value '{currentToken.Value}'",
                        });

                        continue;
                    }

                    parameter.Value = currentToken.Value;

                    result.Add(parameter);
                }
                else if (currentToken.TokenType == TokenTypes.Break) { leftover = currentToken.Value; }
                else
                {
                    errors.Add(new CommandLineError
                    {
                        Fatal = true,
                        ParameterName = currentToken.Name,
                        Message =
                            $"Unknown token ({currentToken.TokenType}) '{currentToken.Name}' with value '{currentToken.Value} - Actually, this probably means the tokenizer is broken.'",
                    });
                }
            }

            return result;
        }
    }
}