# COMMON PATHS

$buildFolder = (Get-Item -Path "./" -Verbose).FullName
$slnFolder = Join-Path $buildFolder "../"
$outputFolder = Join-Path $buildFolder "outputs"
$soucepath =Join-Path $slnFolder "build/outputs/Host/wwwroot/dist/*"
$destinationpath = Join-Path $slnFolder "build/outputs/Host/wwwroot"

$webHostFolder = Join-Path $slnFolder "src\Autumn.Web.Host"

## CLEAR ######################################################################

Remove-Item $outputFolder -Force -Recurse -ErrorAction Ignore
New-Item -Path $outputFolder -ItemType Directory

## RESTORE NUGET PACKAGES #####################################################

Set-Location $slnFolder
dotnet restore

## PUBLISH WEB Host PROJECT ###################################################

Set-Location $webHostFolder
yarn
dotnet publish --output (Join-Path $outputFolder "Host")
#######Dist folder out out of ####################################


copy-Item -path $soucepath  -Destination $destinationpath -Force -Recurse

## CREATE DOCKER IMAGES #######################################################
	


# Host
Set-Location (Join-Path $outputFolder "Host")

docker rmi samples/autumn1 -f
docker build -t samples/autumn1 .

## DOCKER COMPOSE FILES #######################################################

Copy-Item (Join-Path $slnFolder "docker/host/*.*") $outputFolder

## FINALIZE ###################################################################

Set-Location $outputFolder