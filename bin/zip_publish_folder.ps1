$Source = "./bin/Debug/netcoreapp1.1/publish/"
$destination = "./bin/Debug/netcoreapp1.1/artifact.zip"

If(Test-path $destination) {Remove-item $destination}

Add-Type -assembly "system.io.compression.filesystem"

[io.compression.zipfile]::CreateFromDirectory($Source, $destination) 
