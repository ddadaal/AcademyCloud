& "C:\Program Files\Docker Toolbox\docker-machine.exe" env default | Invoke-Expression

[Environment]::SetEnvironmentVariable("DOCKER_TLS_VERIFY", $env:DOCKER_TLS_VERIFY, [System.EnvironmentVariableTarget]::User)
[Environment]::SetEnvironmentVariable("DOCKER_HOST", $env:DOCKER_HOST, [System.EnvironmentVariableTarget]::User)
[Environment]::SetEnvironmentVariable("DOCKER_CERT_PATH", $env:DOCKER_CERT_PATH, [System.EnvironmentVariableTarget]::User)
[Environment]::SetEnvironmentVariable("DOCKER_MACHINE_NAME", $env:DOCKER_MACHINE_NAME, [System.EnvironmentVariableTarget]::User)
[Environment]::SetEnvironmentVariable("COMPOSE_CONVERT_WINDOWS_PATHS", $env:COMPOSE_CONVERT_WINDOWS_PATHS, [System.EnvironmentVariableTarget]::User)
