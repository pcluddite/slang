[Reflection.Assembly]::LoadFile((Join-Path $PWD 'tbasic.dll')) | Out-Null

function Invoke-TScript($scriptPath)
{
    $runtime = New-Object Tbasic.Runtime.TRuntime
    $runtime.Global.LoadStandardLibrary()
    
    $reader = New-Object System.IO.StreamReader($scriptPath)

    $runtime.Execute($reader)

    $reader.Dispose()
}