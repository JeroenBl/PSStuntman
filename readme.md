# PSStuntman
## _because every app needs a stuntman_

![image](https://raw.githubusercontent.com/JeroenBl/PSStuntman/main/assets/logo.png)

There are many times where I need fake user data to test my applications or random stuff that I build.

Previously I created hashtables containing fake user data. But, since I need it quite often, I created _PSStuntman_ to do the job for me.

## Used nuget packages

- https://github.com/bchavez/Bogus
- https://github.com/StackExchange/Dapper

## Table of contents

* [Prerequisites](#Prerequisites)
* [Installation](#Installation)
* [Using the PSStuntman module](#Using-the-PSStuntman-module)
* [Release Notes](Release-Notes)
* [Contributing](Contributing)

## Prerequisites

- [ ] Windows PowerShell 5.1 https://www.microsoft.com/en-us/download/details.aspx?id=54616
- [ ] .NET Framework 4.8 https://dotnet.microsoft.com/download/dotnet-framework/net48

N.B. If you are using _Windows Server 2016 / Windows 10_ or higher, Windows PowerShell 5.1 is installed by default.

## Installation

1. Download the latest release. https://github.com/JeroenBL/PSStuntman/releases

2. Copy the files to a sensible location

3. Import the module

```powershell
Import-Module "C:\Users\_YourUserName_\Documents\PSStuntman\PSStuntman.dll"
```

## Using the PSStuntman module

The PSStuntman PSModule contains the following cmdlets:

| _Cmdlet_                       | _Description_                                                |
| ------------------------------ | ------------------------------------------------------------ |
| New-Stuntman                | Creates new stuntman and has the option to save them to the database |
| _Get-Stuntman_                 | Retrieves the created stuntman from the database    |
| Get-Department   | Retrieves the departments from the database                  |
| ConvertTo-Person | Converts a (or multiple) stuntman to a person |

> The cmdlet: _Remove-Stuntman_ has been removed from version 2.0.0. 

> From version 2.0.0, the cmdlet: _New-Stuntman_ also created departments by using the _-CreateDepartment_ switch. 

---

## Cmdlet: New-Stuntman

Generates new stuntman. The generated stuntman can be saved to a SQlite database.

| _Parameter_        | _Description_                                                |
| ------------------ | ------------------------------------------------------------ |
| Amount           | The amount of stuntman you want to create, e.g. 10         |
| CompanyName      | The CompanyName. e.g. 'Contoso'. When left empty, a random CompanyName will be picked |
| DomainName       | The DomainName. e.g. 'contoso'. The default DomainName is set to 'enyoi' |
| DomainSuffix     | The DomainSuffix e.g. '.com'. The default suffix is set to '.local' |
| Locale           | The locale for the stuntman e.g. 'fr' (for French) or 'en' (for English). The default locale is set to 'nl'. To find more locales: https://github.com/bchavez/Bogus |
| SaveToDatabase     | Saves the generated Stuntman to a SQlite database. You will find the database in root folder from where this module is loaded |
| CreateDepartments | Creates departments based on the value of the _department_ property for a stuntman.<br /><br />Note, this switch also adds a manager to each department and promotes a stuntman to a manager. So, if you create 10 stuntman, all 10 are randomly assigned to a department. For each department, a stuntman will be selected and added as manager for that particular department. The _IsMananger_ property for a stuntman will then be set to  _'1'_ |

---

### Example 1

```powershell
New-Stuntman -Amount 1 -CompanyName MyCoolCompany -DomainName ASP -DomainSuffix NET -Locale en
```

Creates a new stuntman with the provided parameter value's.

```powershell
UserId               : 101
ExternalId           : DEMO101
GivenName            : Lila
FamilyName           : Streich
DisplayName          : Lila Streich
UserName             : L.Streich@ASP.NET
Initials             : L.S
PersonalEmailAddress : Lila.Streich84@hotmail.com
PersonalPhoneNumber  : 844.420.7169
BusinessEmailAddress : L.Streich@MyCoolCompany.com
BusinessPhoneNumber  : 1-850-778-2427
BirthDate            : 14-1-1954 03:55:29
BirthPlace           : Kelsiberg
Language             : en
City                 : Nikoshire
Street               : Freddy Square
HouseNumber          : 131
ZipCode              : 16943
IsActive             : 1
UserGuid             : a5a38b82-56d0-4018-b8fd-edd59f97575e
Title                : Central Interactions Supervisor
IsManager            : 0
StartDate            : 25-11-2016 02:42:37
EndDate              : 15-5-2021 01:05:27
HoursPerWeek         : 40
Company              : MyCoolCompany
Department           : HR
CostCenter           : HR
ContractGuid         : 859bd885-3eb7-4acc-957a-07bf8054ec7f
```

## Cmdlet: Get-Stuntman

Retrieves the created stuntman from the database.

| _Parameter_ | _Description_                        |
| ----------- | ------------------------------------ |
| UserId      | Retrieve a single stuntman by UserID |

### Example 1

```powershell
Get-Stuntman
```

Retrieves all stuntman from the database:

### Example 2

```powershell
Get-Stuntman -UserId 101
```

Retrieves a stuntman with userId '101' from the database

```powershell
UserId               : 101
ExternalId           : DEMO101
GivenName            : Lila
FamilyName           : Streich
DisplayName          : Lila Streich
UserName             : L.Streich@DemoDomainName.com
Initials             : L.S
PersonalEmailAddress : Lila.Streich84@hotmail.com
PersonalPhoneNumber  : 844.420.7169
BusinessEmailAddress : L.Streich@DemoCompany.com
BusinessPhoneNumber  : 1-850-778-2427
BirthDate            : 14-1-1954 03:55:29
BirthPlace           : Kelsiberg
Language             : en
City                 : Nikoshire
Street               : Freddy Square
HouseNumber          : 131
ZipCode              : 16943
IsActive             : 1
UserGuid             : a5a38b82-56d0-4018-b8fd-edd59f97575e
Title                : Central Interactions Supervisor
IsManager            : 0
StartDate            : 25-11-2016 02:42:37
EndDate              : 15-5-2021 01:05:27
HoursPerWeek         : 40
Company              : DemoCompany
Department           : HR
CostCenter           : HR
ContractGuid         : 859bd885-3eb7-4acc-957a-07bf8054ec7f
```

If you want to edit the stuntman or view the data with a Sql brower, make sure to download 'DB Browser for Sqlite' https://sqlitebrowser.org/


## Cmdlet: Get-Department

Gets the departments from the database.

### Example 1

```powershell
Get-Department
```

Retrieves all departments from the database.

```powershell
ExternalId DisplayName ManagerExternalId
---------- ----------- -----------------
         2 Engineering STUNTMAN1        
         3 HR          STUNTMAN2        
         4 Accounting  STUNTMAN3        
         5 Recruitment STUNTMAN4        
         6 IT          STUNTMAN5        
         7 Management  STUNTMAN7        
         8 Marketing   STUNTMAN9        
         9 Finance     STUNTMAN10 
```

## Cmdlet: ConvertTo-Person

Converts a stuntman to a HelloID person object. 

## Release history

### Version 2.0.0

- Added departments and manager info
- Updated Get-Stuntman cmdlet so that it also creates department using the -CreateDepartments switch
- Added cmdlet: Get-Department
- Removed cmdlet: Remove-Stuntman

### Version 1.0.3

1.0.3  - Added cmdlet 'ConvertTo-Person'

### Version 1.0.2

1.0.2  - Initial release

## Contributing

Find a bug or have an idea! Open an issue or submit a pull request!

**Enjoy!**