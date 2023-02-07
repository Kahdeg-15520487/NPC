mkdir "bin\publish\NPC.Editor" -Force
#dotnet publish -r win-x64 -c Release --self-contained true -o "bin\publish\NPC.Editor" -p:PublishSingleFile=true -p:IncludeAllContentForSelfExtract=true -p:PublishTrimmed=True -p:TrimMode=Link -p:DebugType=None -p:DebugSymbols=false
#dotnet publish -r win-x64 -c Release --self-contained true -o "bin\publish\NPC.Editor" -p:PublishSingleFile=true -p:IncludeAllContentForSelfExtract=true -p:DebugType=None -p:DebugSymbols=false
#dotnet publish -r win-x64 -c Release --self-contained false -o "bin\publish\NPC.Editor" -p:DebugType=None -p:DebugSymbols=false
dotnet publish -r win-x64 -c Release --self-contained false -o "bin\publish\NPC.Editor" -p:PublishSingleFile=true -p:IncludeAllContentForSelfExtract=true -p:DebugType=None -p:DebugSymbols=false
mkdir "bin\publish\NPC.Editor\Sample" -Force
Copy-Item -Path ..\Backend\NPC.Runtime\Sample\sample.npc -Destination bin\publish\NPC.Editor\Sample\sample.npc -Force

#7z a NPC.Editor.zip .\bin\publish\NPC.Editor\
$compress = @{
  Path = ".\bin\publish\NPC.Editor\"
  CompressionLevel = "Optimal"
  DestinationPath = "NPC.Editor.zip"
}
Compress-Archive @compress -Force