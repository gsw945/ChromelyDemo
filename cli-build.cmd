rem https://docs.microsoft.com/zh-cn/dotnet/core/tools/dotnet-publish
rem https://docs.microsoft.com/zh-cn/dotnet/core/rid-catalog#windows-rids
rem https://github.com/dotnet/runtime/blob/main/src/libraries/Microsoft.NETCore.Platforms/src/runtime.json
set buildPlatform=x86
set p_runtime=win7-%buildPlatform%
set p_framework=netcoreapp3.1
set p_build=Release
set p_publish=%~dp0build

set pR=--runtime %p_runtime%
set pF=--framework %p_framework%
set pC=--configuration %p_build%
set pSC=--self-contained true
rem set pO=--output %p_publish%
set pO=
set pSF=-p:PublishSingleFile=false
set pRTR=-p:PublishReadyToRun=true -p:PublishReadyToRunShowWarning=true
set pT=-p:PublishTrimmed=true
set pP=-p:Platform=%buildPlatform%

dotnet publish %pF% %pR% %pC% %pSC% %pO% %pSF% %pRTR% %pT% %pP% "ChromelyDemo"