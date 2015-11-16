setlocal disableextensions
set NUGET_SERVER=https://www.nuget.org/api/v2/package/
set NUGET_KEY=%NUGET_SERVER_API_KEY%

IF [%NUGET_KEY%]==[] GOTO key_not_exists
	
"%ProgramFiles(x86)%\MSBuild\14.0\bin\msbuild.exe" /v:n /p:Configuration=NuGet-Debug "..\..\Gaia.sln" /t:Clean
"%ProgramFiles(x86)%\MSBuild\14.0\bin\msbuild.exe" /v:n /p:Configuration=NuGet-Debug "..\..\Gaia.sln" 
GOTO exit

:key_not_exists
echo NUGET_SERVER_API_KEY environment variable is not set	

:exit