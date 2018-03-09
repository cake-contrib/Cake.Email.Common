#load nuget:https://www.myget.org/F/cake-contrib/api/v2?package=Cake.Recipe&prerelease

Environment.SetVariableNames();

BuildParameters.SetParameters(context: Context,
                            buildSystem: BuildSystem,
                            sourceDirectoryPath: "./Source",
                            title: "Cake.Email.Common",
                            repositoryOwner: "cake-contrib",
                            repositoryName: "Cake.Email.Common",
                            shouldRunDotNetCorePack: true,
                            shouldRunDupFinder: true,
                            shouldRunInspectCode: false,
                            shouldRunCodecov: false,
                            appVeyorAccountName: "cakecontrib");

BuildParameters.PrintParameters(Context);

ToolSettings.SetToolSettings(context: Context);

Build.RunDotNetCore();
