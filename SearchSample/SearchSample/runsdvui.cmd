cd /d "C:\Infusion\POC\SearchSample\SearchSample" &msbuild "SearchSample.csproj" /t:sdv /p:configuration="Debug" /p:platform=Any CPU /p:inputs="/devenv /view"
exit %errorlevel% 