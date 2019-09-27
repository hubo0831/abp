# COMMON PATHS

Set-Location "D:\My Documents\Projects\abp"

$rootFolder = (Get-Item -Path "./" -Verbose).FullName

# List of solutions

$solutionPaths = (
    #"framework",
    #"modules/users",
    #"modules/permission-management",
    #"modules/setting-management",
    #"modules/feature-management",
    #"modules/identity",
    #"modules/identityserver",
    "modules/tenant-management",
    #"modules/account",
    "modules/audit-logging"
    #"modules/background-jobs"
    #"modules/docs",
    #"modules/blogging",
    #"modules/client-simulation"
    #"templates/mvc-module",
    #"templates/mvc",
    #"samples/MicroserviceDemo",
    #"abp_io/AbpIoLocalization"
)

# Build all solutions
$outputFolder = Join-Path $rootFolder "lib"
foreach ($solutionPath in $solutionPaths) {    
    $solutionAbsPath = (Join-Path $rootFolder $solutionPath)
    Set-Location $solutionAbsPath
    #dotnet build -c Release -o "$outputFolder" -v d --no-restore --no-dependencies 
    dotnet build -c Release -o "$outputFolder"
    if (-Not $?) {
        Write-Host ("Build failed for the solution: " + $solutionPath)
        #Set-Location $rootFolder
        #exit $LASTEXITCODE
    }
}

Set-Location $outputFolder
#Remove-Item -Recurse "pages"
Remove-Item -Recurse "refs"
Remove-Item "abp*"
Remove-Item "Simple*"
Remove-Item "System.*"
Remove-Item "xunit.*"
Remove-Item "*.json"
Remove-Item "*.config"
Remove-Item "*.Tests*.dll"
Remove-Item "*.Tests*.pdb"
Remove-Item "*.DemoApp*.dll"
Remove-Item "*.DemoApp*.pdb"
Remove-Item "*.TestApp.dll"
Remove-Item "*.TestApp.pdb"
#Remove-Item "*.TestBase.dll"
#Remove-Item "*.TestBase.pdb"
