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

## Prerequisites

- [ ] Windows PowerShell 5.1 https://www.microsoft.com/en-us/download/details.aspx?id=54616
- [ ] .NET Framework 4.8 https://dotnet.microsoft.com/download/dotnet-framework/net48

N.B. If you are using _Windows Server 2016 / Windows 10_ or higer, Windows PowerShell 5.1 is installed by default.

## Installation

1. Download the latest release. https://github.com/mufana/PSStuntman/releases/tag/v1.0.2

2. Copy the files to location that makes sense.

3. Import the module.

```powershell
Import-Module "C:\Users\_YourUserName_\Documents\PSStuntman\PSStuntman.dll"
```

## Using the PSStuntman module

The PSStuntman PSModule contains the following cmdlets:

| _Cmdlet_                       | _Description_                                                |
| ------------------------------ | ------------------------------------------------------------ |
| _New-Stuntman_                 | _Creates new stuntman and has the option to save them to a SQlite database_ |
| _Get-Stuntman_                 | _Retrieves the created stuntman from the SQlite database_    |
| _Remove-Stuntman_ | _Removes all the stuntman from the SQlite database_          |

---

## Cmdlet: New-Stuntman

Generates new stuntman. The generated stuntman can be saved to a SQlite database.

| _Parameter_        | _Description_                                                |
| ------------------ | ------------------------------------------------------------ |
| _Amount_           | _The amount of stuntman you want to create, e.g. 10_         |
| _CompanyName_      | _The CompanyName. e.g. 'Contoso'. When left empty, a random CompanyName will be picked_ |
| _DomainName_       | _The DomainName. e.g. 'contoso.com'. The default DomainName is set to 'enyoi'_ |
| _DomainSuffix_     | _The DomainSuffix e.g. '.com'_                                |
| _Locale_           | _The locale for the stuntman e.g. 'fr' (for French) or 'en' (for English). The default locale is set to 'nl'. To find more locales: https://github.com/bchavez/Bogus_ |
| _SaveToDatabase_     | _Saves the generated Stuntman to a SQlite database. You will find the database in root folder from where this module is loaded_ |

---

### Example 1

```powershell
New-Stuntman -Amount 1 -CompanyName DemoCompany -DomainName DemoDomainName -DomainSuffix .com -Locale en
```

Creates a new stuntman with the provided parameter value's.

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

## Cmdlet: Get-Stuntman

Retrieves the created stuntman from the Sqlite database.

| _Parameter_ | _Description_                          |
| ----------- | -------------------------------------- |
| _UserId_    | _Retrieve a single stuntman by UserID_ |

### Example 1

```powershell
Get-Stuntman
```

Retrieves all stuntman from the Sqlite database:

### Example 2

```powershell
Get-Stuntman -UserId 101
```

Retrieves a single stuntman from the Sqlite database

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


## Cmdlet: Remove-Stuntman

Removes all data from the _Stuntman.db_ Sqlite database.

| _Parameter_ | _Description_                          |
| ----------- | -------------------------------------- |
| _UserId_    | _Remove a stuntman by UserID_ |
| _EmptyDatabase_    | _Removes all stuntman from the database_ |
|


## Release history

### Version 1.0.2

1.0.2  - Initial release