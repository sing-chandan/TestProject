CLS
@echo off

SET custid=%1

ECHO %custid%

CD\
CD "C:\inetpub\w_livemonitoring\App_Data\Data"
SET PATH=%PATH%;"C:\Program Files (x86)\NSIS";"C:\Windows\Microsoft.NET\Framework64\v4.0.30319"

DEL "LiveMonitoringInstaller\Data\*.dll"
DEL "LiveMonitoringInstaller\Data\*.exe"
DEL "LiveMonitoringInstaller\LiveMonitoringSetup.exe"

MSBuild "RenameCustomer\RenameCustomer.csproj" /p:Configuration=Release /p:OutputPath=bin\Release
COPY "RenameCustomer\bin\Release\RenameCustomer.exe" .
RenameCustomer.exe %custid% 0

MSBuild "LiveMonitoring\LiveMonitoring.csproj" /p:Configuration=Release /p:OutputPath=bin\Release
MSBuild "LiveMonitoringService\LiveMonitoringService.csproj" /p:Configuration=Release /p:OutputPath=bin\Release

COPY "LiveMonitoringService\bin\Release\*.dll" "LiveMonitoringInstaller\Data" 
COPY "LiveMonitoringService\bin\Release\*.exe" "LiveMonitoringInstaller\Data" 

COPY "LiveMonitoring\bin\Release\*.dll" "LiveMonitoringInstaller\Data" 
COPY "LiveMonitoring\bin\Release\*.exe" "LiveMonitoringInstaller\Data" 

Makensis.exe "LiveMonitoringInstaller\LiveScriptwithrename.nsi"

RenameCustomer.exe %custid% 1

DEL "LiveMonitoringService\bin\Release\*.dll"
DEL "LiveMonitoringService\bin\Release\*.exe"

DEL "LiveMonitoring\bin\Release\*.exe"
DEL "LiveMonitoring\bin\Release\*.dll"

DEL "LiveMonitoringInstaller\Data\*.dll"
DEL "LiveMonitoringInstaller\Data\*.exe"
