setlocal disableextensions
set NUGET_SERVER=http://cloud17nugetserver.azurewebsites.net/ 
set NUGET_KEY=%NUGET_SERVER_API_KEY%

IF [%NUGET_KEY%]==[] GOTO key_not_exists
	
"%ProgramFiles(x86)%\MSBuild\14.0\bin\msbuild.exe" /v:n /p:Configuration=NuGet-Debug "..\..\Gaia.Core.sln" /t:Clean
"%ProgramFiles(x86)%\MSBuild\14.0\bin\msbuild.exe" /v:n /p:Configuration=NuGet-Debug "..\..\Gaia.Core.sln" 
GOTO exit

:key_not_exists
echo NUGET_SERVER_API_KEY environment variable is not set	

:exit