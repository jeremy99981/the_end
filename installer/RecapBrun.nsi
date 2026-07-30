Unicode true
!include "MUI2.nsh"

!ifndef PROJECT_ROOT
  !define PROJECT_ROOT ".."
!endif
!ifndef APP_VERSION
  !define APP_VERSION "1.4.0"
!endif

Name "Récap Brun"
Caption "Installation de Récap Brun"
OutFile "${PROJECT_ROOT}/artifacts/RecapBrun-Setup-${APP_VERSION}.exe"
InstallDir "$LOCALAPPDATA\Programs\Récap Brun"
InstallDirRegKey HKCU "Software\Récap Brun" "InstallDir"
RequestExecutionLevel user
BrandingText "Récap Brun"
Icon "${PROJECT_ROOT}/assets/recap-brun.ico"
UninstallIcon "${PROJECT_ROOT}/assets/recap-brun.ico"
SetCompressor /SOLID lzma

VIProductVersion "${APP_VERSION}.0"
VIAddVersionKey /LANG=1036 "ProductName" "Récap Brun"
VIAddVersionKey /LANG=1036 "CompanyName" "Récap Brun"
VIAddVersionKey /LANG=1036 "FileDescription" "Installateur de Récap Brun"
VIAddVersionKey /LANG=1036 "FileVersion" "${APP_VERSION}"
VIAddVersionKey /LANG=1036 "ProductVersion" "${APP_VERSION}"
VIAddVersionKey /LANG=1036 "LegalCopyright" "© 2026 Récap Brun"

!insertmacro MUI_PAGE_WELCOME
!insertmacro MUI_PAGE_DIRECTORY
!insertmacro MUI_PAGE_INSTFILES
!insertmacro MUI_PAGE_FINISH
!insertmacro MUI_UNPAGE_CONFIRM
!insertmacro MUI_UNPAGE_INSTFILES

!insertmacro MUI_LANGUAGE "French"

Section "Récap Brun" SecMain
  SectionIn RO
  SetOutPath "$INSTDIR"
  File "${PROJECT_ROOT}/artifacts/RecapBrun-Windows-x64/RecapBrun.exe"
  File "${PROJECT_ROOT}/assets/recap-brun.ico"
  WriteUninstaller "$INSTDIR\Uninstall.exe"

  SetShellVarContext current
  CreateDirectory "$SMPROGRAMS\Récap Brun"
  CreateShortCut "$DESKTOP\Récap Brun.lnk" "$INSTDIR\RecapBrun.exe" "" "$INSTDIR\recap-brun.ico" 0
  CreateShortCut "$SMPROGRAMS\Récap Brun\Récap Brun.lnk" "$INSTDIR\RecapBrun.exe" "" "$INSTDIR\recap-brun.ico" 0
  CreateShortCut "$SMPROGRAMS\Récap Brun\Désinstaller Récap Brun.lnk" "$INSTDIR\Uninstall.exe"

  WriteRegStr HKCU "Software\Récap Brun" "InstallDir" "$INSTDIR"
  WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\Récap Brun" "DisplayName" "Récap Brun"
  WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\Récap Brun" "DisplayVersion" "${APP_VERSION}"
  WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\Récap Brun" "Publisher" "Récap Brun"
  WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\Récap Brun" "DisplayIcon" "$INSTDIR\recap-brun.ico"
  WriteUninstaller "$INSTDIR\Uninstall.exe"
SectionEnd

Section "Uninstall"
  SetShellVarContext current
  Delete "$DESKTOP\Récap Brun.lnk"
  Delete "$SMPROGRAMS\Récap Brun\Récap Brun.lnk"
  Delete "$SMPROGRAMS\Récap Brun\Désinstaller Récap Brun.lnk"
  RMDir "$SMPROGRAMS\Récap Brun"
  Delete "$INSTDIR\RecapBrun.exe"
  Delete "$INSTDIR\recap-brun.ico"
  Delete "$INSTDIR\Uninstall.exe"
  RMDir "$INSTDIR"
  DeleteRegKey HKCU "Software\Récap Brun"
  DeleteRegKey HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\Récap Brun"
SectionEnd
