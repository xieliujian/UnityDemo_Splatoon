
rem working directory
set workdir=%~dp0
set plugindir=%workdir%\Client\Assets\Plugins
set corelibdir=%workdir%\ClientCode\Common\Core\bin\Debug
set excelconfigexportdir=%workdir%\ClientCode\Tools\ExcelConfigExport\bin\Debug
set toolsdir=%workdir%\Tools
set pdb2mdb=%workdir%\Tools\pdb2mdb.exe

echo "[client]: generate *mdb debug files for mono"
pushd %corelibdir%
for /r %%i in (*.pdb) do (
  %pdb2mdb% %%~dpni.dll
)
popd
echo done. & echo.

echo "copy dll to unity3d's plugin directory"
xcopy %corelibdir%\Core.dll %plugindir%
xcopy %corelibdir%\*.mdb %plugindir% /y /q

echo "copy tool to tools directory"
xcopy %excelconfigexportdir%\ExcelConfigExport.exe %toolsdir%