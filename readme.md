# PSStuntman

There are many times where I need fake user data to test my applications or random stuff that I build.

Previously I created hashtables containing fake user data. But, since I need it quite often, I deciced to create _PSStuntman_ to do the job for me.

PSStuntman is written in C# since it relies on several Nuget packages.

- https://github.com/bchavez/Bogus
- https://github.com/StackExchange/Dapper

## Table of contents

* [Prerequisites](#Prerequisites)
* [Installation](#Installation)
* [Using the PSStuntman module](#Using-the-PSStuntman-module)
* [Release Notes](Release-Notes)

## Prerequisites

- [ ] Windows PowerShell 5.1 https://www.microsoft.com/en-us/download/details.aspx?id=54616
- [ ] .NET Framework 4.7.2 https://dotnet.microsoft.com/download/dotnet-framework/thank-you/net472-web-installer

N.B. If you are using _Windows Server 2016 / Windows 10_ or higer, Windows PowerShell 5.1 is installed by default.

### Verify that Windows PowerShell 5.1 is installed

1. Open a new PowerShell console.

2. On the console, enter the code pasted below:

```powershell
$PSVersionTable
```

3. Verify that _Windows PowerShell 5.1_ is installed.

![image](https://github.com/mufana/PSStuntman/blob/main/assets/openWinPS.png)

N.B. _The Build number in the example above is '19041.1'_ That might be different on your computer.

## Installation

1. Download the files from the _releases/net472_ folder.

2. Copy the files to location that makes sense.

3. Import the module.

```powershell
Import-Module "C:\Users\_YourUserName_\Documents\PSStuntman\PSStuntman.dll"
```

4. Have fun!

## Using the PSStuntman module

The PSStuntman PSModule contains the following cmdlets:

| _Cmdlet_                       | _Description_                                                |
| ------------------------------ | ------------------------------------------------------------ |
| _New-Stuntman_                 | _Creates new stuntman and has the option to save them to a SQlite database_ |
| _Get-Stuntman_                 | _Retrieves the created stuntman from the SQlite database_    |
| _Remove-AllStuntmanFromSqlite_ | _Removes all the stuntman from the SQlite database_          |
---

## Cmdlet: New-Stuntman

Generates new stuntman. The generated stuntman can be saved to a SQlite database.

| _Parameter_        | _Description_                                                |
| ------------------ | ------------------------------------------------------------ |
| _Amount_           | _The amount of stuntman you want to create, e.g. 10_         |
| _ExternalIdPrefix_ | _The ExternalId prefix. e.g. 'Ext' When left empty, the default prefix will be set to 'STUNTMAN'_ |
| _UserIdRange_      | _The UserIdRange e.g. '1' This value will always increment with '1'. When left empty, the default UserIdRange will start from '0'_ |
| _CompanyName_      | _The CompanyName. e.g. 'Contoso'. When left empty, a random CompanyName will be picked_ |
| _DomainName_       | _The DomainName. e.g. 'contoso.com'. The default DomainName is set to 'enyoi'_ |
| _DomainSuffix_     | _The DomainSuffix e.g. 'com'_                                |
| _Locale_           | _The locale for the stuntman e.g. 'fr' (for French) or 'en' (for English). The default locale is set to 'nl'. To find more locales: https://github.com/bchavez/Bogus_ |
| _SaveToSqlite_     | _Saves the generated Stuntman to a SQlite database. You will find it in root folder from where this module is loaded_ |
---

### Example 1

```powershell
New-Stuntman -Amount 1 -ExternalIdPrefix DEMO -UserIdRange 100 -CompanyName DemoCompany -DomainName DemoDomainName -DomainSuffix com -Locale en
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

### Example 2

```powershell
New-Stuntman -Amount 1 -ExternalIdPrefix DEMO -UserIdRange 100 -CompanyName DemoCompany -DomainName DemoDomainName -DomainSuffix com -Locale en -SaveToSqlite
```

Creates a new stuntman with the provided parameter value's and saves it to the Sqlite database.

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

1. Open _DB Browser for Sqlite_
![dbMain](https://github.com/mufana/PSStuntman/blob/main/assets/dbbrowserMain.png)

2. Click _Open Database_ to open a Sqlite database file.
![dbOpen](https://github.com/mufana/PSStuntman/blob/main/assets/dbbrowserOpen.png)

3. Browse to the folder where the _PSStuntman_ module / *.dll files are located and open the _Stuntman.db_ file.

4. Click _Browse Data_ to view/edit the data.
![dbView](https://github.com/mufana/PSStuntman/blob/main/assets/dbbrowserBrowse.png)

## Cmdlet: Remove-AllStuntmanFromSqlite

Removes all data from the _Stuntman.db_ Sqlite database.

## Release Notes

| Version | Cmdlet       | Notes                                                        | Date       |
| ------- | ------------ | ------------------------------------------------------------ | ---------- |
| 1.0.1   | Get-Stuntman | Added the option to search a single stuntman based on the <UserId> | 2020-11-03 |
| 1.0.0   |              | Initial release                                              | 2020-11-01 |