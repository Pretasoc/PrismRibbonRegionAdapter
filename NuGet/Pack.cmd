del /q ..\Releases\*.nupkg
nuget pack ..\Prism.RibbonRegionAdapter\Prism.RibbonRegionAdapter.csproj  -Prop Configuration=Release -OutputDirectory ..\Releases -Build -NonInteractive
pause