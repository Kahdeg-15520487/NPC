mkdir "bin\publish\NPC.Editor"
dotnet publish -r win-x64 -c Release --self-contained true -o "bin\publish\NPC.Editor" -p:PublishSingleFile=true -p:IncludeAllContentForSelfExtract=true -p:PublishTrimmed=True -p:TrimMode=Link -p:DebugType=None -p:DebugSymbols=false
mkdir "bin\publish\NPC.Editor\Sample"
copy "..\Backend\NPC.Runtime\Sample\sample.npc" "bin\publish\NPC.Editor\Sample\sample.npc"

7z a NPC.Editor.zip .\bin\publish\NPC.Editor\