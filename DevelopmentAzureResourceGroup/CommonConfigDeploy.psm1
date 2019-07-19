
Param(
[string] $execRegion
)
function setVariables($execRegionParam)
{
 Write-host $execRegionParam
	$execRegion=$execRegionParam
}

function CreateSignInCertificateSetting(){
   Write-host $execRegion
return "value from module"
	
}



Export-ModuleMember -Function CreateSignInCertificateSetting
Export-ModuleMember -Function setVariables

