# Tax Calculator With API - Installation Guide

## 1. Installation Steps:

### SSH-Connect from PS

- **Command PS:**       `ssh-keygen -t rsa -b 4096 -C "IhreEmail@example.com"`

- **Command Server:**   `cd .ssh`

- **Command Server:**   `nano authorized_keys`

- **Command PS:**       `Set-Service -Name ssh-agent -StartupType Manual`

- **Command PS:**       `Start-Service ssh-agent`

- **Command PS:**       `ssh-add mein_ssh_schluessel`

- **Command PS:**       `ssh user@ip`

- **It will find the right connection automatically by the ssh-agent**

### Transfer
- **Command:** `scp TaxCalculatorWithAPI.zip <user>@<ip>:<Directory>`

### Connect
- **Command:** `ssh <user>@<ip>`

### Stop last version
- **Command:** `<user>@localhost:~/<dir>/TaxCalculatorWithAPI# docker-compose down`

### Build
- **Command:** `<user>@localhost:~/<dir>/TaxCalculatorWithAPI# docker-compose build --no-cache`

### Upload
- **Command:** `<user>@localhost:~/<dir>/TaxCalculatorWithAPI# docker-compose up -d`

### Check Containers
- **Command:** `<user>@localhost:~/<dir>/TaxCalculatorWithAPI# docker ps`

#### Expected Output:
```plaintext
CONTAINER ID   IMAGE                        COMMAND                  CREATED          STATUS          PORTS                                                                               NAMES
79df70368d67   taxcalculatorblazorserver   "dotnet TaxCalculator"   43 seconds ago   Up 41 seconds   0.0.0.0:80->80/tcp, :::80->80/tcp, 0.0.0.0:443->443/tcp, :::443->443/tcp             taxcalculatorwithapi_taxcalculatorblazorserver_1
b0f82e6c6cf6   taxcalculatorapi             "dotnet TaxCalculator"   43 seconds ago   Up 41 seconds                                                                                        taxcalculatorwithapi_taxcalculatorapi_1
```

### Check Logs
- **Command:** `<user>@localhost:~/<dir>/TaxCalculatorWithAPI# docker logs 79df70368d67`

### Access Container
- **Command:** `<user>@localhost:~/<dir>/TaxCalculatorWithAPI# docker exec -it b0f82e6c6cf6 bash`

### List Networks
- **Command:** `<user>@localhost:~/<dir>/TaxCalculatorWithAPI# docker network ls`

### Check Internal Network
- **Command:** `<user>@localhost:~/<dir>/TaxCalculatorWithAPI# docker network inspect taxnetwork`
