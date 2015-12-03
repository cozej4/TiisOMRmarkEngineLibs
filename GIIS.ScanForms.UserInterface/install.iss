; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName "TIIS DIVO Scanning Tools"
#define MyAppVersion "0.8.11"
#define MyAppPublisher "Path"
#define MyAppURL "http://path.org"

[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{CC635DE6-82E8-4E0C-8352-8D09D4EFD380}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
;AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={pf}\DIVO Tools
DefaultGroupName={#MyAppName}
OutputDir=.\dist
OutputBaseFilename=setup
Compression=lzma
SolidCompression=yes
WizardSmallImageFile=.\logo.bmp
[Files]
Source: ".\Forms\*.mxml"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\bin\release\GIIS.DataLayer.Contract.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\bin\release\GIIS.DataLayer.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\bin\release\GIIS.ScanForms.UserInterface.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\bin\release\Npgsql.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\bin\release\Mono.Security.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\bin\release\Npgsql.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\Project Items\omr-installer.exe"; DestDir: {tmp} ; Flags: dontcopy

[Run]
Filename: "{tmp}\omr-installer.exe"; Parameters: "/silent /dir=""{app}"" /group=""DIVO Tools"""; Flags: waituntilterminated 

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

